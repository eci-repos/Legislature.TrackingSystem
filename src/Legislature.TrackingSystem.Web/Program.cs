using Legislature.TrackingSystem.Application.Connectors;
using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.Authentication;
using Legislature.TrackingSystem.Application.Persistence;
using Legislature.TrackingSystem.Application.Readiness;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Infrastructure.DependencyInjection;
using Legislature.TrackingSystem.Infrastructure.Persistence;
using Legislature.TrackingSystem.Infrastructure.WorkItems;
using Legislature.TrackingSystem.Web.Api.WorkItems;
using Legislature.TrackingSystem.Web.Auth;
using Legislature.TrackingSystem.Web.Client.Pages;
using Legislature.TrackingSystem.Web.Components;
using Legislature.TrackingSystem.Web.Observability;
using Legislature.TrackingSystem.Web.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.RateLimiting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddLtsApplication();
string? connectionString = builder.Configuration.GetConnectionString("Default");
PersistenceMode persistenceMode = PersistencePolicy.ResolveMode(connectionString);

// Enforce the persistence policy: production requires PostgreSQL. The in-memory fallback is a
// supported local/offline development mode only, so a production deployment cannot silently run on
// it. Non-production in-memory runs log a warning after the app is built.
if (!PersistencePolicy.IsAllowed(persistenceMode, builder.Environment.EnvironmentName))
{
    throw new InvalidOperationException(
        "The persistence policy requires PostgreSQL in Production. Configure 'ConnectionStrings:Default'; "
        + "the in-memory fallback is a local/offline development mode only.");
}

builder.Services.AddLtsInfrastructure(connectionString);

var connectorOptions = builder.Configuration.GetSection("Connectors").Get<ConnectorOptions>() ?? new ConnectorOptions();
builder.Services.AddLtsConnectors(connectorOptions);
builder.Services.AddSingleton<ConnectorOptionsValidator>();

// Fail fast on a misconfigured external connector so a bad live endpoint surfaces at startup with
// actionable errors instead of opaque HTTP failures at runtime.
IReadOnlyList<string> connectorErrors = new ConnectorOptionsValidator().Validate(connectorOptions);
if (connectorErrors.Count > 0)
{
    throw new InvalidOperationException(
        "The Connectors (external connector) configuration is invalid:\n  - "
        + string.Join("\n  - ", connectorErrors));
}

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider, ServerAuthenticationStateProvider>();

// Structured JSON console logging for production observability.
builder.Logging.AddJsonConsole();

// Health checks: /health (liveness) and /health/ready (readiness, verifies the database when
// PostgreSQL is configured).
var healthChecks = builder.Services.AddHealthChecks();
if (!string.IsNullOrWhiteSpace(connectionString))
{
    builder.Services.AddSingleton<DatabaseHealthCheck>(sp =>
        new DatabaseHealthCheck(sp, checkDatabase: true));
    healthChecks.AddCheck<DatabaseHealthCheck>("postgres", tags: ["ready"]);
}

// Connector readiness: verifies connectivity to each configured external connector. When none is
// configured the check reports healthy (dev-boundary fakes in use).
builder.Services.AddSingleton<ConnectorHealthCheck>(_ =>
    new ConnectorHealthCheck(connectorOptions, new HttpClient { Timeout = TimeSpan.FromSeconds(5) }));
healthChecks.AddCheck<ConnectorHealthCheck>("connectors", tags: ["ready"]);

// Rate limiting for the versioned API endpoints: a fixed-window policy of 100 requests per minute
// per client, rejecting excess requests with 429. The partition is the authenticated user's key when
// present, otherwise the client IP, so each user and each anonymous client gets its own limit.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("api", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? context.Connection.RemoteIpAddress?.ToString()
                ?? "anonymous",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
            }));
});

// Security headers: default values applied to every response, with per-route overrides available.
builder.Services.AddSingleton(new SecurityHeadersOptions());

// OpenTelemetry tracing, metrics, and logging with an OTLP exporter. Enabled only when an OTLP
// endpoint is configured so the app runs without telemetry infrastructure by default.
string? otlpEndpoint = builder.Configuration["Otlp:Endpoint"];
if (!string.IsNullOrWhiteSpace(otlpEndpoint))
{
    Uri endpoint = new(otlpEndpoint);
    builder.Services.AddOpenTelemetry()
        .ConfigureResource(resource => resource.AddService(Observability.ServiceName))
        .WithTracing(tracing => tracing
            .AddSource(Observability.ServiceName)
            .AddOtlpExporter(options => options.Endpoint = endpoint))
        .WithMetrics(metrics => metrics
            .AddMeter(Observability.ServiceName)
            .AddOtlpExporter(options => options.Endpoint = endpoint));
}

var entraOptions = builder.Configuration.GetSection("AzureAd").Get<EntraAuthOptions>() ?? new EntraAuthOptions();
builder.Services.AddSingleton(entraOptions);
builder.Services.AddSingleton<EntraClaimsMapper>();
builder.Services.AddSingleton<EntraAuthOptionsValidator>();

if (entraOptions.IsConfigured)
{
    // Fail fast on a misconfigured Entra boundary so a bad tenant/app registration surfaces at
    // startup with actionable errors instead of opaque token-validation failures at runtime.
    IReadOnlyList<string> entraErrors = new EntraAuthOptionsValidator().Validate(entraOptions);
    if (entraErrors.Count > 0)
    {
        throw new InvalidOperationException(
            "The AzureAd (Entra/OpenID Connect) configuration is invalid:\n  - "
            + string.Join("\n  - ", entraErrors));
    }

    // Real Microsoft Entra ID / OpenID Connect boundary: validate bearer tokens against the Entra
    // tenant authority (issuer, audience, and signing keys from the OIDC metadata) and map Entra
    // claims to the LTS role-to-permission matrix on token validation.
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = entraOptions.Authority;
            options.Audience = entraOptions.ClientId;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = entraOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = entraOptions.ClientId,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5),
            };
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    if (context.Principal is not null)
                    {
                        var mapper = context.HttpContext.RequestServices.GetRequiredService<EntraClaimsMapper>();
                        context.Principal = mapper.ApplyRoleClaim(context.Principal);
                    }

                    return Task.CompletedTask;
                },
            };
        });
}
else
{
    // Local/offline development boundary: symmetric-key JWT bearer tokens issued by the dev
    // /api/v1/auth/token endpoint.
    string jwtKey = builder.Configuration["Jwt:Key"]
        ?? throw new InvalidOperationException("Jwt:Key is not configured.");
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1),
            };
        });
}
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequirePrepare", p => p.Requirements.Add(new PermissionRequirement(Permission.Prepare)));
    options.AddPolicy("RequireApprove", p => p.Requirements.Add(new PermissionRequirement(Permission.Approve)));
    options.AddPolicy("RequireDeliver", p => p.Requirements.Add(new PermissionRequirement(Permission.Deliver)));
    options.AddPolicy("RequireReadOnly", p => p.Requirements.Add(new PermissionRequirement(Permission.ReadOnly)));
    options.AddPolicy("RequireAdminister", p => p.Requirements.Add(new PermissionRequirement(Permission.Administer)));
    options.AddPolicy("RequireManageAccess", p => p.Requirements.Add(new PermissionRequirement(Permission.ManageAccess)));
    options.AddPolicy("RequireMigrate", p => p.Requirements.Add(new PermissionRequirement(Permission.Migrate)));
    options.AddPolicy("RequireViewHistorical", p => p.Requirements.Add(new PermissionRequirement(Permission.ViewHistorical)));
});
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();
builder.Services.AddScoped<JwtTokenService>();

builder.Services.AddScoped(sp =>
{
    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
    HttpRequest? request = httpContextAccessor.HttpContext?.Request;
    string baseAddress = request is null
        ? "http://localhost/"
        : $"{request.Scheme}://{request.Host}{request.PathBase}/";

    string defaultUserKey = builder.Configuration["Jwt:DefaultUserKey"] ?? "admin";
    var handler = new JwtAuthorizationMessageHandler(defaultUserKey)
    {
        InnerHandler = new HttpClientHandler(),
    };
    return new HttpClient(handler) { BaseAddress = new Uri(baseAddress) };
});

var app = builder.Build();

if (persistenceMode == PersistenceMode.InMemory)
{
    app.Logger.LogWarning(
        "Running with the in-memory persistence fallback (no PostgreSQL connection string). "
        + "This is a supported local/offline development mode; production requires PostgreSQL.");
}

using (IServiceScope scope = app.Services.CreateScope())
{
    if (!string.IsNullOrWhiteSpace(connectionString))
    {
        var db = scope.ServiceProvider.GetRequiredService<LtsDbContext>();
        await db.Database.MigrateAsync();
    }

    var seeder = scope.ServiceProvider.GetRequiredService<SeedDataInitializer>();
    await seeder.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Show the SPA /not-found page for page (non-API) requests that produce an error status. API
// requests under /api are left untouched so their real status codes (e.g. 429 from the rate
// limiter, 400 from validation) are preserved instead of being masked by a re-executed POST to the
// antiforgery-protected /not-found page.
app.UseStatusCodePages(async statusCodeContext =>
{
    if (statusCodeContext.HttpContext.Request.Path.StartsWithSegments("/api"))
    {
        return;
    }

    var originalPath = statusCodeContext.HttpContext.Request.Path;
    statusCodeContext.HttpContext.Request.Path = "/not-found";
    try
    {
        await statusCodeContext.Next(statusCodeContext.HttpContext);
    }
    finally
    {
        statusCodeContext.HttpContext.Request.Path = originalPath;
    }
});
app.UseHttpsRedirection();

app.UseSecurityHeaders();
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

// Antiforgery is required by the Blazor interactive components (e.g. the /not-found
// page carries antiforgery metadata). The JSON API group below opts out via
// DisableAntiforgery() because it is secured by JWT bearer tokens and the .NET 8+
// antiforgery middleware would otherwise reject its non-form content types
// (application/json) with a 400 "The request has an incorrect Content-type."
app.UseAntiforgery();

app.MapStaticAssets();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    // Liveness: report healthy as long as the process is up; exclude readiness-tagged checks.
    Predicate = check => !check.Tags.Contains("ready"),
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
});
app.MapGet("/api/v1/readiness", (ISprintReadinessService readinessService, ILogger<Program> logger) =>
{
    Observability.ReadinessRequests.Add(1);
    using System.Diagnostics.Activity? activity = Observability.ActivitySource.StartActivity("readiness");
    logger.LogInformation("Readiness requested at {Timestamp}.", DateTimeOffset.UtcNow);
    return Results.Ok(readinessService.GetReadiness());
})
    .WithName("GetSprintReadiness");

// Versioned API group: rate-limited, request-body validated, and exempt from
// antiforgery (JWT-secured JSON API; antiforgery is reserved for Blazor forms).
var api = app.MapGroup("")
    .RequireRateLimiting("api")
    .AddEndpointFilter<ValidationFilter>()
    .DisableAntiforgery();

api.MapPost("/api/v1/auth/token", async (TokenRequest request, JwtTokenService tokenService, CancellationToken cancellationToken) =>
{
    try
    {
        JwtTokenResult result = await tokenService.IssueTokenAsync(request.UserKey, cancellationToken);
        return Results.Ok(new { token = result.Token, userKey = result.UserKey, role = result.Role, expiresAt = result.ExpiresAt });
    }
    catch (InvalidOperationException)
    {
        return Results.Unauthorized();
    }
}).WithName("IssueToken");

api.MapPost("/api/v1/work-tasks", async (CreateWorkTaskRequest request, IWorkTaskService workTaskService, CancellationToken cancellationToken) =>
{
    var command = new CreateWorkTaskCommand(
        request.Type,
        request.Title,
        request.Description,
        request.DueDate,
        request.Priority,
        request.Status,
        request.Owner,
        request.StoryId,
        request.RequirementId,
        request.RequirementType,
        request.SourceDocument);

    WorkTaskDto dto = await workTaskService.CreateAsync(command, cancellationToken);
    return Results.Created($"/api/v1/work-tasks/{dto.Id}", dto);
}).WithName("CreateWorkTask");

api.MapGet("/api/v1/work-tasks/{id:guid}", async (Guid id, IWorkTaskService workTaskService, CancellationToken cancellationToken) =>
{
    WorkTaskDto? dto = await workTaskService.GetByIdAsync(id, cancellationToken);
    return dto is null ? Results.NotFound() : Results.Ok(dto);
}).WithName("GetWorkTask");

api.MapPost("/api/v1/work-tasks/{id:guid}/identifier-override", async (Guid id, OverrideIdentifierRequest request, IWorkTaskService workTaskService, CancellationToken cancellationToken) =>
{
    var command = new OverrideWorkItemIdentifierCommand(id, request.NewIdentifier);
    try
    {
        WorkTaskDto dto = await workTaskService.OverrideIdentifierAsync(command, cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { error = ex.Message });
    }
}).WithName("OverrideWorkItemIdentifier");

api.MapPost("/api/v1/work-tasks/{id:guid}/assignments", async (Guid id, AssignTaskRequest request, IWorkTaskAssignmentService assignmentService, CancellationToken cancellationToken) =>
{
    var command = new AssignWorkTaskCommand(id, request.AssigneeKey, request.Role, request.DueDate, request.AssignedByKey);
    try
    {
        TaskAssignmentDto dto = await assignmentService.AssignAsync(command, cancellationToken);
        return Results.Created($"/api/v1/work-queue/{request.AssigneeKey}", dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("AssignWorkTask");

api.MapPost("/api/v1/work-tasks/{id:guid}/assignments/reassign", async (Guid id, ReassignTaskRequest request, IWorkTaskAssignmentService assignmentService, CancellationToken cancellationToken) =>
{
    var command = new ReassignWorkTaskCommand(
        id,
        request.PriorAssigneeKey,
        request.NewAssigneeKey,
        request.Role,
        request.DueDate,
        request.AssignedByKey);
    try
    {
        TaskAssignmentDto dto = await assignmentService.ReassignAsync(command, cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("ReassignWorkTask");

api.MapGet("/api/v1/work-queue/{assigneeKey}", async (string assigneeKey, IWorkTaskAssignmentService assignmentService, CancellationToken cancellationToken) =>
{
    WorkQueueDto queue = await assignmentService.GetWorkQueueAsync(assigneeKey, cancellationToken);
    return Results.Ok(queue);
}).WithName("GetUserWorkQueue");

api.MapPost("/api/v1/work-tasks/{id:guid}/categorization", async (Guid id, SetCategorizationRequest request, IWorkTaskService workTaskService, CancellationToken cancellationToken) =>
{
    var command = new SetWorkTaskCategorizationCommand(id, request.IsConfidential, request.IsExecutiveReview);
    try
    {
        WorkTaskDto dto = await workTaskService.SetCategorizationAsync(command, cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("SetWorkTaskCategorization");

api.MapPost("/api/v1/work-items/{id:guid}/submit-for-review", async (Guid id, SubmitWorkItemForReviewRequest request, IWorkflowService workflowService, CancellationToken cancellationToken) =>
{
    var command = new SubmitWorkItemForReviewCommand(id, request.ReviewerKeys, request.SubmittedByKey);
    try
    {
        WorkTaskDto dto = await workflowService.SubmitForReviewAsync(command, cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("SubmitWorkItemForReview");

api.MapPost("/api/v1/work-items/{id:guid}/reviews", async (Guid id, RecordWorkflowReviewRequest request, IWorkflowService workflowService, CancellationToken cancellationToken) =>
{
    var command = new RecordWorkflowReviewCommand(id, request.ReviewerKey, request.Decision, request.Comment);
    try
    {
        WorkTaskDto dto = await workflowService.RecordReviewAsync(command, cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("RecordWorkflowReview");

api.MapPost("/api/v1/work-items/{id:guid}/finalize", async (Guid id, IWorkflowService workflowService, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await workflowService.FinalizeAsync(new FinalizeWorkItemCommand(id), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("FinalizeWorkItem");

api.MapPost("/api/v1/work-items/{id:guid}/executive-review/start", async (Guid id, StartExecutiveReviewRequest request, IExecutiveReviewService executiveReviewService, CancellationToken cancellationToken) =>
{
    var command = new StartExecutiveReviewCommand(id, request.ReviewerKeys, request.StartedByKey);
    try
    {
        WorkTaskDto dto = await executiveReviewService.StartExecutiveReviewAsync(command, cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("StartExecutiveReview");

api.MapPost("/api/v1/work-items/{id:guid}/executive-review/begin", async (Guid id, BeginExecutiveReviewStepRequest request, IExecutiveReviewService executiveReviewService, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await executiveReviewService.BeginExecutiveReviewStepAsync(new BeginExecutiveReviewStepCommand(id, request.ReviewerKey), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("BeginExecutiveReviewStep");

api.MapPost("/api/v1/work-items/{id:guid}/executive-review/adjust", async (Guid id, AdjustExecutiveReviewRequest request, IExecutiveReviewService executiveReviewService, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await executiveReviewService.AdjustExecutiveReviewAsync(new AdjustExecutiveReviewCommand(id, request.ReviewerKey, request.Note), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("AdjustExecutiveReview");

api.MapPost("/api/v1/work-items/{id:guid}/executive-review/complete", async (Guid id, CompleteExecutiveReviewStepRequest request, IExecutiveReviewService executiveReviewService, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await executiveReviewService.CompleteExecutiveReviewStepAsync(new CompleteExecutiveReviewStepCommand(id, request.ReviewerKey, request.Comment), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("CompleteExecutiveReviewStep");

api.MapPost("/api/v1/work-items/{id:guid}/priority", async (Guid id, SetWorkTaskPriorityRequest request, IExecutiveReviewService executiveReviewService, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await executiveReviewService.SetPriorityAsync(new SetWorkTaskPriorityCommand(id, request.Priority), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("SetWorkTaskPriority");

api.MapPost("/api/v1/work-items/{id:guid}/steps", async (Guid id, AddWorkflowStepRequest request, IExecutiveReviewService executiveReviewService, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await executiveReviewService.AddStepAsync(new AddWorkflowStepCommand(id, request.Name, request.DueDate), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("AddWorkflowStep");

api.MapPut("/api/v1/work-items/{id:guid}/steps/{stepId:guid}/due-date", async (Guid id, Guid stepId, SetWorkflowStepDueDateRequest request, IExecutiveReviewService executiveReviewService, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await executiveReviewService.SetStepDueDateAsync(new SetWorkflowStepDueDateCommand(id, stepId, request.DueDate), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("SetWorkflowStepDueDate");

api.MapPost("/api/v1/work-items/{id:guid}/steps/{stepId:guid}/complete", async (Guid id, Guid stepId, IExecutiveReviewService executiveReviewService, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await executiveReviewService.CompleteStepAsync(new CompleteWorkflowStepCommand(id, stepId), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("CompleteWorkflowStep");

api.MapPut("/api/v1/work-items/{id:guid}/content", async (Guid id, SetWorkItemContentRequest request, IContentService contentService, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await contentService.SetContentAsync(new SetWorkItemContentCommand(id, request.Content), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("SetWorkItemContent");

api.MapPost("/api/v1/work-items/{id:guid}/attachments", async (Guid id, AddAttachmentRequest request, IContentService contentService, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await contentService.AddAttachmentAsync(new AddAttachmentCommand(id, request.FileName, request.ContentType, request.SizeBytes, request.AddedByKey), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("AddAttachment");

api.MapDelete("/api/v1/work-items/{id:guid}/attachments/{attachmentId:guid}", async (Guid id, Guid attachmentId, IContentService contentService, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await contentService.RemoveAttachmentAsync(new RemoveAttachmentCommand(id, attachmentId), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("RemoveAttachment");

api.MapGet("/api/v1/templates", async (ITemplateService templateService, CancellationToken cancellationToken) =>
{
    IReadOnlyList<DocumentTemplateDto> templates = await templateService.ListTemplatesAsync(cancellationToken);
    return Results.Ok(templates);
}).WithName("ListTemplates");

api.MapPost("/api/v1/templates", async (CreateDocumentTemplateRequest request, ITemplateService templateService, CancellationToken cancellationToken) =>
{
    try
    {
        DocumentTemplateDto dto = await templateService.CreateTemplateAsync(
            new CreateDocumentTemplateCommand(request.Name, request.ApplicableWorkType, request.Body, request.IsShared),
            cancellationToken);
        return Results.Ok(dto);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("CreateTemplate");

api.MapPut("/api/v1/templates/{id:guid}", async (Guid id, UpdateDocumentTemplateRequest request, ITemplateService templateService, CancellationToken cancellationToken) =>
{
    try
    {
        DocumentTemplateDto dto = await templateService.UpdateTemplateAsync(new UpdateDocumentTemplateCommand(id, request.Body), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("UpdateTemplate");

api.MapPut("/api/v1/templates/{id:guid}/shared", async (Guid id, SetDocumentTemplateSharedRequest request, ITemplateService templateService, CancellationToken cancellationToken) =>
{
    try
    {
        DocumentTemplateDto dto = await templateService.SetTemplateSharedAsync(new SetDocumentTemplateSharedCommand(id, request.IsShared), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("SetTemplateShared");

api.MapPost("/api/v1/work-items/{id:guid}/documents/generate", async (Guid id, GenerateDocumentRequest request, ITemplateService templateService, CancellationToken cancellationToken) =>
{
    try
    {
        GeneratedDocumentDto dto = await templateService.GenerateDocumentAsync(
            new GenerateDocumentCommand(id, request.TemplateId, request.GeneratedByKey),
            cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("GenerateDocument");

api.MapGet("/api/v1/work-items/{id:guid}/documents", async (Guid id, ITemplateService templateService, CancellationToken cancellationToken) =>
{
    IReadOnlyList<GeneratedDocumentDto> documents = await templateService.ListGeneratedDocumentsAsync(id, cancellationToken);
    return Results.Ok(documents);
}).WithName("ListGeneratedDocuments");

api.MapPost("/api/v1/work-items/{id:guid}/reuse", async (Guid id, ReuseContentRequest request, IReuseService reuseService, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await reuseService.ReuseContentAsync(new ReuseContentCommand(id, request.SourceWorkItemId), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("ReuseContent");

api.MapPut("/api/v1/work-items/{id:guid}", async (Guid id, UpdateWorkTaskRequest request, ITaskMaintenanceService maintenance, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await maintenance.UpdateAsync(
            new UpdateWorkTaskCommand(id, request.Title, request.Description, request.DueDate, request.Priority, request.ByKey),
            cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("UpdateWorkTask");

api.MapPost("/api/v1/work-items/{id:guid}/cancel", async (Guid id, CancelWorkTaskRequest request, ITaskMaintenanceService maintenance, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await maintenance.CancelAsync(new CancelWorkTaskCommand(id, request.ByKey), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("CancelWorkTask");

api.MapPost("/api/v1/work-items/{id:guid}/duplicate", async (Guid id, DuplicateWorkTaskRequest request, ITaskMaintenanceService maintenance, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await maintenance.DuplicateAsync(new DuplicateWorkTaskCommand(id, request.Title, request.ByKey), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("DuplicateWorkTask");

api.MapPut("/api/v1/work-items/{id:guid}/customer-due-date", async (Guid id, SetCustomerDueDateRequest request, ITaskMaintenanceService maintenance, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await maintenance.SetCustomerDueDateAsync(new SetCustomerDueDateCommand(id, request.CustomerDueDate), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("SetCustomerDueDate");

api.MapPost("/api/v1/work-items/{id:guid}/comments", async (Guid id, AddWorkTaskCommentRequest request, ITaskMaintenanceService maintenance, CancellationToken cancellationToken) =>
{
    try
    {
        WorkTaskDto dto = await maintenance.AddCommentAsync(new AddWorkTaskCommentCommand(id, request.AuthorKey, request.Body), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("AddWorkTaskComment");

api.MapGet("/api/v1/notifications", async (string userKey, INotificationService notificationService, CancellationToken cancellationToken) =>
{
    try
    {
        IReadOnlyList<NotificationDto> notifications = await notificationService.ListForUserAsync(userKey, cancellationToken);
        return Results.Ok(notifications);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("ListNotifications");

api.MapPut("/api/v1/notifications/{id:guid}/read", async (Guid id, INotificationService notificationService, CancellationToken cancellationToken) =>
{
    try
    {
        NotificationDto dto = await notificationService.MarkReadAsync(id, cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("MarkNotificationRead");

api.MapPost("/api/v1/bills/ingest", async (IngestBillUpdateRequest request, ILegislativeIngestionService ingestion, CancellationToken cancellationToken) =>
{
    try
    {
        BillDto dto = await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand(request.BillNumber, request.Title, request.VersionLabel, request.Language, request.Source, request.Year, request.Biennium),
            cancellationToken);
        return Results.Ok(dto);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("IngestBillUpdate");

api.MapPost("/api/v1/bills/status", async (IngestBillStatusRequest request, ILegislativeIngestionService ingestion, CancellationToken cancellationToken) =>
{
    try
    {
        BillDto dto = await ingestion.IngestBillStatusAsync(new IngestBillStatusCommand(request.BillNumber, request.Status), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("IngestBillStatus");

api.MapPost("/api/v1/bills/amendments", async (IngestAmendmentRequest request, ILegislativeIngestionService ingestion, CancellationToken cancellationToken) =>
{
    try
    {
        BillDto dto = await ingestion.IngestAmendmentAsync(new IngestAmendmentCommand(request.BillNumber, request.AmendmentNumber, request.Language, request.Source), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("IngestAmendment");

api.MapGet("/api/v1/bills/{id:guid}/versions", async (Guid id, IVersionService versionService, CancellationToken cancellationToken) =>
{
    try
    {
        IReadOnlyList<BillVersionDto> versions = await versionService.ListBillVersionsAsync(id, cancellationToken);
        return Results.Ok(versions);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("ListBillVersions");

api.MapGet("/api/v1/bills/history", async (string billNumber, IVersionService versionService, CancellationToken cancellationToken) =>
{
    IReadOnlyList<BillDto> history = await versionService.GetBillHistoryAsync(billNumber, cancellationToken);
    return Results.Ok(history);
}).WithName("GetBillHistory");

api.MapPost("/api/v1/bills/{id:guid}/versions/compare", async (Guid id, CompareBillVersionsRequest request, IVersionService versionService, CancellationToken cancellationToken) =>
{
    try
    {
        BillComparisonDto comparison = await versionService.CompareBillVersionsAsync(
            new CompareBillVersionsCommand(id, request.LeftVersionId, request.RightVersionId),
            cancellationToken);
        return Results.Ok(comparison);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("CompareBillVersions");

api.MapGet("/api/v1/work-items/{id:guid}/versions", async (Guid id, IVersionService versionService, CancellationToken cancellationToken) =>
{
    try
    {
        IReadOnlyList<WorkTaskVersionDto> versions = await versionService.ListWorkTaskVersionsAsync(id, cancellationToken);
        return Results.Ok(versions);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("ListWorkTaskVersions");

api.MapPost("/api/v1/search", async (SearchRequest request, ISearchService searchService, CancellationToken cancellationToken) =>
{
    SearchResultDto result = await searchService.SearchAsync(new SearchCommand(request.Query), cancellationToken);
    return Results.Ok(result);
}).WithName("Search");

api.MapPost("/api/v1/reports/standard", async (RunStandardReportRequest request, IReportingService reportingService, CancellationToken cancellationToken) =>
{
    try
    {
        ReportResultDto result = await reportingService.RunStandardReportAsync(new RunStandardReportCommand(request.ReportName), cancellationToken);
        return Results.Ok(result);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("RunStandardReport");

api.MapPost("/api/v1/reports/custom", async (CreateCustomReportRequest request, IReportingService reportingService, CancellationToken cancellationToken) =>
{
    try
    {
        CustomReportDto dto = await reportingService.CreateCustomReportAsync(new CreateCustomReportCommand(request.Name, request.OwnerKey, request.Query), cancellationToken);
        return Results.Created($"/api/v1/reports/custom/{dto.Id}", dto);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("CreateCustomReport");

api.MapGet("/api/v1/reports/custom", async (string ownerKey, IReportingService reportingService, CancellationToken cancellationToken) =>
{
    IReadOnlyList<CustomReportDto> reports = await reportingService.ListCustomReportsAsync(ownerKey, cancellationToken);
    return Results.Ok(reports);
}).WithName("ListCustomReports");

api.MapPost("/api/v1/reports/custom/run", async (RunCustomReportRequest request, IReportingService reportingService, CancellationToken cancellationToken) =>
{
    try
    {
        ReportResultDto result = await reportingService.RunCustomReportAsync(new RunCustomReportCommand(request.ReportId), cancellationToken);
        return Results.Ok(result);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("RunCustomReport");

api.MapPost("/api/v1/work-items/{id:guid}/extract", async (Guid id, ExtractWorkProductRequest request, IReportingService reportingService, CancellationToken cancellationToken) =>
{
    try
    {
        ExtractResultDto result = await reportingService.ExtractWorkProductAsync(new ExtractWorkProductCommand(id, request.Format), cancellationToken);
        return Results.Ok(result);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("ExtractWorkProduct");

api.MapGet("/api/v1/fiscal-data", async (FiscalDataCategory? category, IFiscalDataService fiscalDataService, CancellationToken cancellationToken) =>
{
    IReadOnlyList<FiscalDataDto> data = await fiscalDataService.ListAsync(category, cancellationToken);
    return Results.Ok(data);
}).WithName("ListFiscalData");

api.MapPost("/api/v1/fiscal-data", async (UpsertFiscalDataRequest request, IFiscalDataService fiscalDataService, CancellationToken cancellationToken) =>
{
    try
    {
        FiscalDataDto dto = await fiscalDataService.UpsertAsync(
            new UpsertFiscalDataCommand(request.Category, request.Name, request.Value, request.Unit, request.Source),
            cancellationToken);
        return Results.Ok(dto);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("UpsertFiscalData");

api.MapPost("/api/v1/work-items/{id:guid}/fiscal-note/calculate", async (Guid id, IFiscalDataService fiscalDataService, CancellationToken cancellationToken) =>
{
    try
    {
        FiscalCalculationDto result = await fiscalDataService.CalculateFiscalNoteAsync(new CalculateFiscalNoteCommand(id), cancellationToken);
        return Results.Ok(result);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("CalculateFiscalNote");

api.MapPost("/api/v1/work-items/{id:guid}/work-papers", async (Guid id, AddFiscalWorkPaperRequest request, IFiscalWorkPaperService workPaperService, CancellationToken cancellationToken) =>
{
    try
    {
        FiscalWorkPaperDto dto = await workPaperService.AddAsync(
            new AddFiscalWorkPaperCommand(id, request.Title, request.Content, request.CreatedByKey),
            cancellationToken);
        return Results.Created($"/api/v1/work-items/{id}/work-papers/{dto.Id}", dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("AddFiscalWorkPaper");

api.MapGet("/api/v1/work-items/{id:guid}/work-papers", async (Guid id, IFiscalWorkPaperService workPaperService, CancellationToken cancellationToken) =>
{
    IReadOnlyList<FiscalWorkPaperDto> papers = await workPaperService.ListForTaskAsync(id, cancellationToken);
    return Results.Ok(papers);
}).WithName("ListFiscalWorkPapers");

api.MapPost("/api/v1/bills/{id:guid}/budget-flag", async (Guid id, FlagBudgetBillRequest request, IBudgetBillService budgetBillService, CancellationToken cancellationToken) =>
{
    try
    {
        BillDto dto = await budgetBillService.FlagAsync(new FlagBudgetBillCommand(id, request.IsBudgetBill), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("FlagBudgetBill");

api.MapGet("/api/v1/bills/budget", async (IBudgetBillService budgetBillService, CancellationToken cancellationToken) =>
{
    IReadOnlyList<BillDto> bills = await budgetBillService.ListFlaggedAsync(cancellationToken);
    return Results.Ok(bills);
}).WithName("ListBudgetBills");

api.MapPost("/api/v1/bills/{id:guid}/fiscal-notes", async (Guid id, LinkFiscalNoteRequest request, IBudgetBillService budgetBillService, CancellationToken cancellationToken) =>
{
    try
    {
        BillFiscalNoteLinkDto dto = await budgetBillService.LinkFiscalNoteAsync(
            new LinkFiscalNoteCommand(id, request.WorkTaskId, request.LinkedByKey),
            cancellationToken);
        return Results.Created($"/api/v1/bills/{id}/fiscal-notes", dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("LinkFiscalNote");

api.MapGet("/api/v1/bills/{id:guid}/fiscal-notes", async (Guid id, IBudgetBillService budgetBillService, CancellationToken cancellationToken) =>
{
    IReadOnlyList<WorkTaskDto> notes = await budgetBillService.GetFiscalNotesForBillAsync(id, cancellationToken);
    return Results.Ok(notes);
}).WithName("GetFiscalNotesForBill");

api.MapPost("/api/v1/demographics", async (UpsertDemographicDataRequest request, IDemographicDataService demographicDataService, CancellationToken cancellationToken) =>
{
    try
    {
        DemographicDataDto dto = await demographicDataService.UpsertAsync(
            new UpsertDemographicDataCommand(request.Session, request.Category, request.Value, request.Year),
            cancellationToken);
        return Results.Ok(dto);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("UpsertDemographicData");

api.MapGet("/api/v1/demographics", async (string session, IDemographicDataService demographicDataService, CancellationToken cancellationToken) =>
{
    IReadOnlyList<DemographicDataDto> data = await demographicDataService.ListForSessionAsync(session, cancellationToken);
    return Results.Ok(data);
}).WithName("ListDemographicData");

api.MapGet("/api/v1/demographics/sessions", async (IDemographicDataService demographicDataService, CancellationToken cancellationToken) =>
{
    IReadOnlyList<string> sessions = await demographicDataService.ListSessionsAsync(cancellationToken);
    return Results.Ok(sessions);
}).WithName("ListDemographicSessions");

api.MapPost("/api/v1/work-items/{id:guid}/templates/populate", async (Guid id, PopulateTemplateRequest request, IProductivityIntegrationService productivity, CancellationToken cancellationToken) =>
{
    try
    {
        string body = await productivity.PopulateTemplateAsync(new PopulateTemplateCommand(id, request.TemplateId), cancellationToken);
        return Results.Ok(new { body });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("PopulateTemplate");

api.MapPost("/api/v1/email", async (EmailOutputRequest request, IProductivityIntegrationService productivity, CancellationToken cancellationToken) =>
{
    try
    {
        EmailDispatchDto dto = await productivity.EmailOutputAsync(
            new EmailOutputCommand(request.Recipient, request.Subject, request.Body, request.SentByKey),
            cancellationToken);
        return Results.Created($"/api/v1/email/{dto.Id}", dto);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("EmailOutput");

api.MapGet("/api/v1/email", async (IProductivityIntegrationService productivity, CancellationToken cancellationToken) =>
{
    IReadOnlyList<EmailDispatchDto> dispatches = await productivity.ListEmailDispatchesAsync(cancellationToken);
    return Results.Ok(dispatches);
}).WithName("ListEmailDispatches");

api.MapPost("/api/v1/expense-estimates", async (UpsertExpenseEstimateElementRequest request, IExpenseEstimateService expenseEstimateService, CancellationToken cancellationToken) =>
{
    try
    {
        ExpenseEstimateElementDto dto = await expenseEstimateService.UpsertAsync(
            new UpsertExpenseEstimateElementCommand(request.Name, request.Kind, request.Value, request.EffectiveDate, request.UpdatedByKey),
            cancellationToken);
        return Results.Ok(dto);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("UpsertExpenseEstimateElement");

api.MapGet("/api/v1/expense-estimates", async (IExpenseEstimateService expenseEstimateService, CancellationToken cancellationToken) =>
{
    IReadOnlyList<ExpenseEstimateElementDto> elements = await expenseEstimateService.ListAsync(cancellationToken);
    return Results.Ok(elements);
}).WithName("ListExpenseEstimateElements");

api.MapPost("/api/v1/work-items/{id:guid}/expense-estimate/calculate", async (Guid id, IExpenseEstimateService expenseEstimateService, CancellationToken cancellationToken) =>
{
    try
    {
        ExpenseEstimateCalculationDto result = await expenseEstimateService.CalculateAsync(new CalculateExpenseEstimateCommand(id), cancellationToken);
        return Results.Ok(result);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("CalculateExpenseEstimate");

api.MapPost("/api/v1/users", async (RegisterUserRequest request, Legislature.TrackingSystem.Application.WorkItems.IAuthorizationService authorization, CancellationToken cancellationToken) =>
{
    try
    {
        UserAccountDto dto = await authorization.RegisterUserAsync(
            new RegisterUserCommand(request.UserKey, request.DisplayName, request.Role),
            cancellationToken);
        return Results.Created($"/api/v1/users/{dto.UserKey}", dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("RegisterUser").RequireAuthorization("RequireAdminister");

api.MapGet("/api/v1/users", async (Legislature.TrackingSystem.Application.WorkItems.IAuthorizationService authorization, CancellationToken cancellationToken) =>
{
    IReadOnlyList<UserAccountDto> users = await authorization.ListUsersAsync(cancellationToken);
    return Results.Ok(users);
}).WithName("ListUsers").RequireAuthorization("RequireAdminister");

api.MapGet("/api/v1/users/{key}/permissions", async (string key, Legislature.TrackingSystem.Application.WorkItems.IAuthorizationService authorization, CancellationToken cancellationToken) =>
{
    IReadOnlyList<Permission> permissions = await authorization.ListPermissionsForRoleAsync(
        (await authorization.ListUsersAsync(cancellationToken)).FirstOrDefault(u => string.Equals(u.UserKey, key, StringComparison.OrdinalIgnoreCase))?.Role ?? UserRole.ReadOnly,
        cancellationToken);
    return Results.Ok(permissions);
}).WithName("ListUserPermissions").RequireAuthorization("RequireAdminister");

api.MapPost("/api/v1/work-items/{id:guid}/access-restrictions", async (Guid id, RestrictAccessRequest request, IAccessControlService accessControl, CancellationToken cancellationToken) =>
{
    try
    {
        AccessRestrictionDto dto = await accessControl.RestrictAsync(
            new RestrictAccessCommand(id, request.DataType, request.RestrictedUserType, request.Note, request.SetByKey),
            cancellationToken);
        return Results.Created($"/api/v1/work-items/{id}/access-restrictions/{dto.Id}", dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("RestrictAccess").RequireAuthorization("RequireManageAccess");

api.MapGet("/api/v1/work-items/{id:guid}/access-restrictions", async (Guid id, IAccessControlService accessControl, CancellationToken cancellationToken) =>
{
    IReadOnlyList<AccessRestrictionDto> restrictions = await accessControl.ListForTaskAsync(id, cancellationToken);
    return Results.Ok(restrictions);
}).WithName("ListAccessRestrictions").RequireAuthorization("RequireManageAccess");

api.MapPost("/api/v1/migrations", async (ImportLegacyDataRequest request, IMigrationService migrationService, CancellationToken cancellationToken) =>
{
    try
    {
        var rows = request.Rows.Select(r => new LegacyImportRow(r.SourceKey, r.Title, r.Type, r.Year, r.Biennium, r.StoryId, r.RequirementId)).ToList();
        LegacyMigrationBatchDto dto = await migrationService.ImportAsync(
            new ImportLegacyDataCommand(request.Source, request.ImportedByKey, rows),
            cancellationToken);
        return Results.Created($"/api/v1/migrations/{dto.Id}", dto);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("ImportLegacyData").RequireAuthorization("RequireMigrate");

api.MapGet("/api/v1/migrations", async (IMigrationService migrationService, CancellationToken cancellationToken) =>
{
    IReadOnlyList<LegacyMigrationBatchDto> batches = await migrationService.ListBatchesAsync(cancellationToken);
    return Results.Ok(batches);
}).WithName("ListMigrationBatches").RequireAuthorization("RequireMigrate");

api.MapPost("/api/v1/migrations/{id:guid}/rollback", async (Guid id, IMigrationService migrationService, CancellationToken cancellationToken) =>
{
    try
    {
        LegacyMigrationBatchDto dto = await migrationService.RollbackAsync(new RollbackMigrationCommand(id), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("RollbackMigration").RequireAuthorization("RequireMigrate");

api.MapGet("/api/v1/historical", async (int years, IHistoricalReferenceService historical, CancellationToken cancellationToken) =>
{
    try
    {
        IReadOnlyList<HistoricalWorkProductDto> products = await historical.ListWorkProductsAsync(years, cancellationToken);
        return Results.Ok(products);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("ListHistoricalWorkProducts").RequireAuthorization("RequireViewHistorical");

api.MapPost("/api/v1/correspondence", async (RecordCorrespondenceRequest request, ICorrespondenceService correspondence, CancellationToken cancellationToken) =>
{
    try
    {
        CorrespondenceDto dto = await correspondence.RecordAsync(
            new RecordCorrespondenceCommand(request.BillId, request.WorkTaskId, request.Recipient, request.Subject, request.Body, request.SentByKey),
            cancellationToken);
        return Results.Created($"/api/v1/correspondence/{dto.Id}", dto);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("RecordCorrespondence");

api.MapPost("/api/v1/correspondence/{id:guid}/response", async (Guid id, ICorrespondenceService correspondence, CancellationToken cancellationToken) =>
{
    try
    {
        CorrespondenceDto dto = await correspondence.MarkResponseReceivedAsync(new MarkResponseReceivedCommand(id), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("MarkCorrespondenceResponse");

api.MapGet("/api/v1/correspondence", async (ICorrespondenceService correspondence, CancellationToken cancellationToken) =>
{
    IReadOnlyList<CorrespondenceDto> items = await correspondence.ListAsync(cancellationToken);
    return Results.Ok(items);
}).WithName("ListCorrespondence");

api.MapPost("/api/v1/implementation-tasks", async (Guid billId, AssignImplementationTaskRequest request, IImplementationTaskService implementation, CancellationToken cancellationToken) =>
{
    try
    {
        ImplementationTaskDto dto = await implementation.AssignAsync(
            new AssignImplementationTaskCommand(billId, request.Title, request.AssignedTo, request.Division, request.RequiredWork, request.DueDate, request.AssignedByKey),
            cancellationToken);
        return Results.Created($"/api/v1/implementation-tasks/{dto.Id}", dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("AssignImplementationTask");

api.MapPut("/api/v1/implementation-tasks/{id:guid}/reassign", async (Guid id, ReassignImplementationTaskRequest request, IImplementationTaskService implementation, CancellationToken cancellationToken) =>
{
    try
    {
        ImplementationTaskDto dto = await implementation.ReassignAsync(
            new ReassignImplementationTaskCommand(id, request.AssignedTo, request.Division, request.DueDate),
            cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("ReassignImplementationTask");

api.MapPost("/api/v1/implementation-tasks/{id:guid}/complete", async (Guid id, IImplementationTaskService implementation, CancellationToken cancellationToken) =>
{
    try
    {
        ImplementationTaskDto dto = await implementation.CompleteAsync(new CompleteImplementationTaskCommand(id), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("CompleteImplementationTask");

api.MapPost("/api/v1/implementation-tasks/{id:guid}/documents", async (Guid id, ShareImplementationDocumentRequest request, IImplementationTaskService implementation, CancellationToken cancellationToken) =>
{
    try
    {
        ImplementationTaskDto dto = await implementation.ShareDocumentAsync(
            new ShareImplementationDocumentCommand(id, request.FileName, request.ContentType, request.SizeBytes, request.SharedByKey),
            cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("ShareImplementationDocument");

api.MapGet("/api/v1/implementation-tasks", async (IImplementationTaskService implementation, CancellationToken cancellationToken) =>
{
    IReadOnlyList<ImplementationTaskDto> tasks = await implementation.ListAsync(cancellationToken);
    return Results.Ok(tasks);
}).WithName("ListImplementationTasks");

api.MapGet("/api/v1/implementation-tasks/status-report", async (IImplementationTaskService implementation, CancellationToken cancellationToken) =>
{
    IReadOnlyList<ImplementationStatusReportRowDto> report = await implementation.GenerateStatusReportAsync(cancellationToken);
    return Results.Ok(report);
}).WithName("ImplementationStatusReport");

api.MapPost("/api/v1/bills/{id:guid}/implementation-flag", async (Guid id, MarkBillRequiresImplementationRequest request, IImplementationTaskService implementation, CancellationToken cancellationToken) =>
{
    try
    {
        BillDto dto = await implementation.MarkBillRequiresImplementationAsync(
            new MarkBillRequiresImplementationCommand(id, request.RequiresImplementation, request.ByKey),
            cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("MarkBillRequiresImplementation");

api.MapGet("/api/v1/bills/{id:guid}/executive-view", async (Guid id, IExecutiveBillViewService executiveView, CancellationToken cancellationToken) =>
{
    try
    {
        ExecutiveBillViewDto dto = await executiveView.GetBillViewAsync(id, cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("GetExecutiveBillView");

api.MapPost("/api/v1/bills/{id:guid}/discussions", async (Guid id, PostExecutiveQuestionRequest request, IExecutiveDiscussionService discussion, CancellationToken cancellationToken) =>
{
    try
    {
        ExecutiveDiscussionDto dto = await discussion.PostQuestionAsync(
            new PostExecutiveQuestionCommand(id, request.AuthorKey, request.Question, request.AssociatedWorkTaskId),
            cancellationToken);
        return Results.Created($"/api/v1/bills/{id}/discussions/{dto.Id}", dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("PostExecutiveQuestion");

api.MapPost("/api/v1/discussions/{id:guid}/answer", async (Guid id, PostExecutiveAnswerRequest request, IExecutiveDiscussionService discussion, CancellationToken cancellationToken) =>
{
    try
    {
        ExecutiveDiscussionDto dto = await discussion.PostAnswerAsync(
            new PostExecutiveAnswerCommand(id, request.Answer, request.AnsweredByKey),
            cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("PostExecutiveAnswer");

api.MapGet("/api/v1/bills/{id:guid}/discussions", async (Guid id, IExecutiveDiscussionService discussion, CancellationToken cancellationToken) =>
{
    IReadOnlyList<ExecutiveDiscussionDto> items = await discussion.ListForBillAsync(id, cancellationToken);
    return Results.Ok(items);
}).WithName("ListExecutiveDiscussions");

api.MapPost("/api/v1/work-items/{id:guid}/relationships", async (Guid id, LinkWorkItemsRequest request, IWorkItemRelationshipService relationshipService, CancellationToken cancellationToken) =>
{
    var command = new LinkWorkItemsCommand(id, request.TargetItemId, request.Type, request.CreatedByKey);
    try
    {
        WorkItemRelationshipDto dto = await relationshipService.LinkAsync(command, cancellationToken);
        return Results.Created($"/api/v1/work-items/{id}/relationships", dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("LinkWorkItems");

api.MapDelete("/api/v1/work-items/{id:guid}/relationships/{relationshipId:guid}", async (Guid id, Guid relationshipId, IWorkItemRelationshipService relationshipService, CancellationToken cancellationToken) =>
{
    try
    {
        await relationshipService.UnlinkAsync(new UnlinkWorkItemsCommand(id, relationshipId), cancellationToken);
        return Results.NoContent();
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("UnlinkWorkItems");

api.MapGet("/api/v1/work-items/{id:guid}/relationships", async (Guid id, IWorkItemRelationshipService relationshipService, CancellationToken cancellationToken) =>
{
    IReadOnlyList<WorkItemRelationshipDto> dtos = await relationshipService.GetForItemAsync(id, cancellationToken);
    return Results.Ok(dtos);
}).WithName("GetWorkItemRelationships");

api.MapPost("/api/v1/packages", async (CreatePackageRequest request, IPackageService packageService, CancellationToken cancellationToken) =>
{
    var command = new CreatePackageCommand(request.Name, request.Description, request.CreatedByKey);
    PackageDto dto = await packageService.CreateAsync(command, cancellationToken);
    return Results.Created($"/api/v1/packages/{dto.Id}", dto);
}).WithName("CreatePackage");

api.MapGet("/api/v1/packages", async (IPackageService packageService, CancellationToken cancellationToken) =>
{
    IReadOnlyList<PackageDto> dtos = await packageService.GetAllAsync(cancellationToken);
    return Results.Ok(dtos);
}).WithName("GetPackages");

api.MapGet("/api/v1/packages/{id:guid}", async (Guid id, IPackageService packageService, CancellationToken cancellationToken) =>
{
    PackageDto? dto = await packageService.GetByIdAsync(id, cancellationToken);
    return dto is null ? Results.NotFound() : Results.Ok(dto);
}).WithName("GetPackage");

api.MapPost("/api/v1/packages/{id:guid}/work-products", async (Guid id, AddWorkProductToPackageRequest request, IPackageService packageService, CancellationToken cancellationToken) =>
{
    var command = new AddWorkProductToPackageCommand(id, request.WorkItemId, request.AddedByKey);
    try
    {
        PackageDto dto = await packageService.AddWorkProductAsync(command, cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("AddWorkProductToPackage");

api.MapDelete("/api/v1/packages/{id:guid}/work-products/{workItemId:guid}", async (Guid id, Guid workItemId, IPackageService packageService, CancellationToken cancellationToken) =>
{
    try
    {
        PackageDto dto = await packageService.RemoveWorkProductAsync(new RemoveWorkProductFromPackageCommand(id, workItemId), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("RemoveWorkProductFromPackage");

api.MapPost("/api/v1/packages/{id:guid}/deliver", async (Guid id, IPackageService packageService, CancellationToken cancellationToken) =>
{
    try
    {
        PackageDto dto = await packageService.DeliverAsync(new DeliverPackageCommand(id), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithName("DeliverPackage");

api.MapPost("/api/v1/packages/{id:guid}/recipients", async (Guid id, AddPackageRecipientRequest request, IPackageService packageService, CancellationToken cancellationToken) =>
{
    var command = new AddPackageRecipientCommand(id, request.Name, request.Kind, request.AddedByKey);
    try
    {
        PackageDto dto = await packageService.AddRecipientAsync(command, cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("AddPackageRecipient");

api.MapPost("/api/v1/packages/{id:guid}/finalize", async (Guid id, IPackageService packageService, CancellationToken cancellationToken) =>
{
    try
    {
        PackageDto dto = await packageService.FinalizePackageAsync(new FinalizePackageCommand(id), cancellationToken);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
}).WithName("FinalizePackage");

api.MapGet("/api/v1/work-items", async (
    bool? confidential,
    bool? executiveReview,
    WorkTaskStatus? status,
    WorkItemType? type,
    Guid? packageId,
    WorkItemSortField? sortBy,
    bool? sortDescending,
    WorkItemGroupBy? groupBy,
    IWorkItemQueryService queryService,
    CancellationToken cancellationToken) =>
{
    var query = new WorkItemQuery(
        confidential,
        executiveReview,
        status,
        type,
        packageId,
        sortBy ?? WorkItemSortField.CreatedAt,
        sortDescending ?? false,
        groupBy ?? WorkItemGroupBy.None);
    WorkItemQueryResultDto result = await queryService.QueryAsync(query, cancellationToken);
    return Results.Ok(result);
}).WithName("QueryWorkItems");

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Legislature.TrackingSystem.Web.Client._Imports).Assembly);

// Unit-of-work: flush tracked EF Core changes after each successful request. The in-memory
// adapters persist mutations by reference, but EF Core requires an explicit save. This middleware
// resolves the scoped DbContext (present only when a connection string is configured) and saves
// pending changes so service-level mutations to loaded aggregates persist.
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
    {
        LtsDbContext? db = context.RequestServices.GetService<LtsDbContext>();
        if (db is not null)
        {
            await db.SaveChangesAsync(context.RequestAborted);
        }
    }
});

app.Run();
