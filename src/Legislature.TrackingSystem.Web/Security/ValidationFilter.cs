using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Security;

/// <summary>
/// Endpoint filter that validates request-body arguments against their DataAnnotations and returns
/// a 400 validation problem when any property is invalid.
/// </summary>
public sealed class ValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        foreach (object? argument in context.Arguments)
        {
            if (argument is null)
            {
                continue;
            }

            var validationContext = new ValidationContext(argument);
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(argument, validationContext, results, validateAllProperties: true))
            {
                continue;
            }

            var errors = results
                .Where(r => r.MemberNames.Any())
                .GroupBy(r => r.MemberNames.First())
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(r => r.ErrorMessage ?? "Invalid value.").ToArray());

            return Results.ValidationProblem(errors);
        }

        return await next(context);
    }
}
