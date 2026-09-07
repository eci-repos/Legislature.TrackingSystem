using Legislature.TrackingSystem.Web.Api.WorkItems;
using Legislature.TrackingSystem.Web.Security;
using Microsoft.AspNetCore.Http;

namespace Legislature.TrackingSystem.Web.Tests.Security;

public sealed class ValidationFilterTests
{
    [Fact]
    public async Task InvokeAsync_WithInvalidArgument_ReturnsValidationProblem()
    {
        var filter = new ValidationFilter();
        var context = new DefaultHttpContext();
        var invocationContext = new TestInvocationContext(context, new object[] { new SearchRequest("") });

        object? result = await filter.InvokeAsync(invocationContext, _ => ValueTask.FromResult<object?>(Results.Ok()));

        var problem = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, problem.StatusCode);
        var details = Assert.IsType<Microsoft.AspNetCore.Http.HttpValidationProblemDetails>(problem.ProblemDetails);
        Assert.True(details.Errors.ContainsKey("Query"));
    }

    [Fact]
    public async Task InvokeAsync_WithValidArgument_InvokesNext()
    {
        var filter = new ValidationFilter();
        var context = new DefaultHttpContext();
        var invocationContext = new TestInvocationContext(context, new object[] { new SearchRequest("HB 1001") });
        var nextCalled = false;

        object? result = await filter.InvokeAsync(invocationContext, _ =>
        {
            nextCalled = true;
            return ValueTask.FromResult<object?>(Results.Ok());
        });

        Assert.True(nextCalled);
        Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok>(result);
    }

    private sealed class TestInvocationContext : EndpointFilterInvocationContext
    {
        public TestInvocationContext(HttpContext httpContext, IList<object?> arguments)
        {
            HttpContext = httpContext;
            Arguments = arguments;
        }

        public override HttpContext HttpContext { get; }

        public override IList<object?> Arguments { get; }

        public override T GetArgument<T>(int index) => (T)Arguments[index]!;
    }
}
