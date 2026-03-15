using System.ComponentModel;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using xSdk.Data.Models;
using xSdk.Extensions.Variable;
using xSdk.Extensions.Web;

namespace xSdk.Controllers;

[
    ApiVersion(1),
    ApiController,
    Route("api/v{version:apiVersion}/[controller]"),
    Produces("application/json"),
    ProducesErrorResponseType(typeof(ProblemDetails))
]
public class VariableController(IVariableService variableSvc, ILogger<HealthController> logger) : ControllerBase
{
    [
        HttpGet("definition"),
        MapToApiVersion(1),
        Authorize,
        EndpointName(nameof(GetVariables)),
        EndpointSummary("Gets all configured variables"),
        EndpointDescription("Returns all configured and loaded variables from Environment, FileSystem, Options and Commandline"),
        ProducesResponseType<IEnumerable<VariableModel>>(StatusCodes.Status200OK, "application/json", Description = "Array of variables"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Variable"),
    ]
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

    [
        HttpGet("definition/{name}"),
        MapToApiVersion(1),
        Authorize,
        EndpointName(nameof(GetVariable)),
        EndpointSummary("Get a specific variable"),
        EndpointDescription("Returns a variable by name"),
        ProducesResponseType<VariableModel>(StatusCodes.Status200OK, "application/json", Description = "A specific variable"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Variable"),
    ]
    public async Task<ActionResult<IEnumerable<VariableModel>>> GetVariable(
        [Description("The name of the variable")] string name,
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

    [
        HttpGet("resource"),
        MapToApiVersion(1),
        Authorize,
        EndpointName(nameof(GetResourceNames)),
        EndpointSummary("Gets resource names"),
        EndpointDescription("Returns a dictionary of resource names for open telemetry. You need to be authenticated to get resource names"),
        ProducesResponseType<IDictionary<string, object>>(StatusCodes.Status200OK, "application/json", Description = "Dictionary of resource names"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Variable"),
    ]
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

    [
        HttpGet("value"),
        MapToApiVersion(1),
        Authorize,
        EndpointName(nameof(GetValues)),
        EndpointSummary("Gets values of all variables"),
        EndpointDescription("Returns a dictionary of values for variables. You need to be authenticated to get variable values"),
        ProducesResponseType<IDictionary<string, object>>(StatusCodes.Status200OK, "application/json", Description = "Dictionary of variable values"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Variable"),
    ]
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
