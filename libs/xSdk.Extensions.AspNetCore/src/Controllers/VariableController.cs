using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using xSdk.Data.Models;
using xSdk.Extensions.Variable;
using xSdk.Extensions.Web;

namespace xSdk.Controllers;

[ApiVersion(1)]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[SwaggerTag("Shows information about all configured and loaded variables given from Environment, FileSystem, Options and Commandline")]
public class VariableController(IVariableService variableSvc, ILogger<HealthController> logger) : ControllerBase
{
    [HttpGet("definition")]
    [MapToApiVersion(1)]
    [Authorize]
    [SwaggerOperation(Summary = "Gets all configured variables", Description = "Returns all configured and loaded variables from Environment, FileSystem, Options and Commandline")]
    [SwaggerResponse(StatusCodes.Status200OK, Description = "Array of variables", Type = typeof(IEnumerable<VariableModel>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IEnumerable<VariableModel>>> GetVariables(CancellationToken token = default)
    {
        try
        {
            logger.LogDebug("Get all variable definitions");

            await Task.Yield();

            var result = new List<VariableModel>();
            foreach (var variable in variableSvc.Variables)
            {
                result.Add(new VariableModel(variable));
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Variables could not loaded");
            return this.BadRequestAsProblem(ex);
        }
    }

    [HttpGet("definition/{name}")]
    [MapToApiVersion(1)]
    [Authorize]
    [SwaggerOperation(Summary = "Get a specific variable", Description = "Returns a variable by name")]
    [SwaggerResponse(StatusCodes.Status200OK, Description = "A specific variable", Type = typeof(VariableModel))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IEnumerable<VariableModel>>> GetVariable(
        [SwaggerParameter("The name of the variable")] string name,
        CancellationToken token = default)
    {
        try
        {
            logger.LogDebug("Get all variable definitions");

            await Task.Yield();

            var result = new List<VariableModel>();
            foreach (var variable in variableSvc.Variables)
            {
                result.Add(new VariableModel(variable));
            }

            return Ok(result.Single(x => x.Name == name));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Variables could not loaded");
            return this.BadRequestAsProblem(ex);
        }
    }

    [HttpGet("resource")]
    [MapToApiVersion(1)]
    [Authorize]
    [SwaggerOperation(
        Summary = "Gets resource names",
        Description = "Returns a dictionary of resource names for open telemetry. You need to be authenticated to get resource names"
    )]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        Description = "Dictionary of resource names",
        Type = typeof(IDictionary<string, object>)
    )]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IDictionary<string, object>>> GetResourceNames(CancellationToken token = default)
    {
        try
        {
            logger.LogDebug("Get all variable resource names");

            await Task.Yield();

            var resourceNames = variableSvc.BuildResources();

            return Ok(resourceNames);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Variable resource names not loaded");
            return this.BadRequestAsProblem(ex);
        }
    }

    [HttpGet("value")]
    [MapToApiVersion(1)]
    [Authorize]
    [SwaggerOperation(Summary = "Gets values of all variables", Description = "Returns a dictionary of values for variables. You need to be authenticated to get resource names")]
    [SwaggerResponse(StatusCodes.Status200OK, Description = "Dictionary of resource names", Type = typeof(IDictionary<string, object>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<IDictionary<string, object>>> GetValues(CancellationToken token = default)
    {
        try
        {
            logger.LogDebug("Get all variable with values");

            await Task.Yield();

            var variables = variableSvc.ToDictionary();

            return Ok(variables);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Variables with values could not loaded");
            return this.BadRequestAsProblem(ex);
        }
    }
}
