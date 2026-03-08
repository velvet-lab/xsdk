using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using xSdk.Data.Models;
using xSdk.Extensions.Web;

namespace xSdk.Controllers;

[ApiVersion(1)]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("text/plain")]
[SwaggerTag("Health Checks for the API without authentication")]
public class HealthController(ILogger<HealthController> logger) : ControllerBase
{
    [HttpGet()]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Gets current status of API", Description = "It could be used for Health Checks")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(HealthResponseExample))]
    [SwaggerResponse(StatusCodes.Status200OK, Description = "OK, if everything is fine", Type = typeof(string))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> GetStatus(CancellationToken token = default)
    {
        try
        {
            logger.LogDebug("Get status for API");

            await Task.Yield();

            return Ok("ok");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Status could not loaded");
            return this.BadRequestAsProblem(ex);
        }
    }

    [HttpGet("ping")]
    [MapToApiVersion(1)]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Sends a ping", Description = "Sends a ping and await a pong. Could be used for Health Checks")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PingResponseExample))]
    [SwaggerResponse(StatusCodes.Status200OK, Description = "Returns Pong, if everything is fine", Type = typeof(string))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> GetPong(CancellationToken token = default)
    {
        try
        {
            logger.LogDebug("Pong requested");

            await Task.Yield();

            return Ok("pong");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Pong could not sent");
            return this.BadRequestAsProblem(ex);
        }
    }
}
