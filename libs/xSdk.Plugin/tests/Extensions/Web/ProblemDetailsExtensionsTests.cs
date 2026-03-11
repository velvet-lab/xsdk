using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using xSdk.Extensions.Web;

namespace xSdk.Plugin.Tests.Extensions.Web;

file class TestController : ControllerBase
{
    public TestController(HttpContext httpContext)
    {
        ControllerContext = new ControllerContext
        {
            HttpContext = httpContext,
            RouteData = new RouteData(),
            ActionDescriptor = new ControllerActionDescriptor()
        };
    }
}

public class ProblemDetailsExtensionsTests
{
    [Fact]
    public void AsProblem_Default_Returns500WithMessage()
    {
        var ex = new InvalidOperationException("Something went wrong");

        var result = ex.AsProblem();

        Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        Assert.Equal("Something went wrong", result.Title);
    }

    [Fact]
    public void AsProblem_WithStatus_ReturnsGivenStatus()
    {
        var ex = new Exception("Bad input");

        var result = ex.AsProblem(StatusCodes.Status400BadRequest);

        Assert.Equal(StatusCodes.Status400BadRequest, result.Status);
    }

    [Fact]
    public void AsProblem_WithInnerException_UseInnerMessage()
    {
        var inner = new ArgumentException("inner message");
        var ex = new Exception("outer message", inner);

        var result = ex.AsProblem();

        Assert.Equal("inner message", result.Title);
    }

    [Fact]
    public void AsProblem_WithPath_SetsInstance()
    {
        var ex = new Exception("error");

        var result = ex.AsProblem(500, "/api/test");

        Assert.Equal("/api/test", result.Instance);
    }

    [Fact]
    public void AsProblem_WithNullPath_DoesNotSetInstance()
    {
        var ex = new Exception("error");

        var result = ex.AsProblem(500, null);

        Assert.Null(result.Instance);
    }

    [Fact]
    public void AsProblem_WithEmptyPath_DoesNotSetInstance()
    {
        var ex = new Exception("error");

        var result = ex.AsProblem(500, string.Empty);

        Assert.Null(result.Instance);
    }

    [Fact]
    public void AsProblem_SetsDetailToStackTrace()
    {
        Exception ex;
        try
        {
            throw new Exception("test");
        }
        catch (Exception e)
        {
            ex = e;
        }

        var result = ex.AsProblem();

        Assert.NotNull(result.Detail);
    }

    [Fact]
    public void BadRequestAsProblem_WithException_Returns400WithMessage()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Path = "/api/resource";
        var controller = new TestController(httpContext);
        var ex = new InvalidOperationException("Invalid state");

        var result = controller.BadRequestAsProblem(ex);

        Assert.IsType<BadRequestObjectResult>(result);
        var problem = (ProblemDetails)result.Value!;
        Assert.Equal(StatusCodes.Status400BadRequest, problem.Status);
        Assert.Equal("/api/resource", problem.Instance);
    }

    [Fact]
    public void BadRequestAsProblem_WithMessage_Returns400()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Path = "/api/resource";
        var controller = new TestController(httpContext);

        var result = controller.BadRequestAsProblem("Validation failed");

        Assert.IsType<BadRequestObjectResult>(result);
        var problem = (ProblemDetails)result.Value!;
        Assert.Equal("Validation failed", problem.Title);
        Assert.Equal(StatusCodes.Status400BadRequest, problem.Status);
    }

    [Fact]
    public void BadRequestAsProblem_WithMessageAndDetails_SetsDetail()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Path = "/api/resource";
        var controller = new TestController(httpContext);

        var result = controller.BadRequestAsProblem("Error", "detailed info");

        var problem = (ProblemDetails)result.Value!;
        Assert.Equal("detailed info", problem.Detail);
    }

    [Fact]
    public void BadRequestAsProblem_WithNullDetails_DoesNotSetDetail()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Path = "/api/resource";
        var controller = new TestController(httpContext);

        var result = controller.BadRequestAsProblem("Error", null);

        var problem = (ProblemDetails)result.Value!;
        Assert.Null(problem.Detail);
    }

    [Fact]
    public void NotFoundAsProblem_WithMessage_Returns404()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Path = "/api/resource/1";
        var controller = new TestController(httpContext);

        var result = controller.NotFoundAsProblem("Resource not found");

        Assert.IsType<NotFoundObjectResult>(result);
        var problem = (ProblemDetails)result.Value!;
        Assert.Equal(StatusCodes.Status404NotFound, problem.Status);
        Assert.Equal("Resource not found", problem.Title);
    }

    [Fact]
    public void NotFoundAsProblem_WithMessageAndDetails_SetsDetail()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Path = "/api/resource/1";
        var controller = new TestController(httpContext);

        var result = controller.NotFoundAsProblem("Resource not found", "The item with ID 1 does not exist");

        var problem = (ProblemDetails)result.Value!;
        Assert.Equal("The item with ID 1 does not exist", problem.Detail);
    }
}
