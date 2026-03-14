using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Web;

namespace xSdk.Demos.Controllers.V2;

[
    ApiController,
    Route("api/v{version:apiVersion}/[controller]"),
    ApiVersion(2),
    AdvertiseApiVersions("2"),
    Produces("application/json"),
    Consumes("application/json"),
    ProducesErrorResponseType(typeof(ProblemDetails))
]
public sealed class SampleController(ILogger<SampleController> logger) : ControllerBase
{
    [
        HttpGet(),
        MapToApiVersion(2),
        AllowAnonymous,
        //Authorize
        EndpointName(nameof(GetSampleAsyncV2)),
        EndpointSummary("Sends a Hello World in Version 2 back"),
        EndpointDescription("Demostrates a hello world in version 2"),
        ProducesResponseType<string>(StatusCodes.Status200OK, "text/plain", Description = "Sends a Hello World back in Version 2"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Sample"),
    ]
    public async Task<ActionResult> GetSampleAsyncV2(CancellationToken token = default)
    {
        try
        {
            logger.LogDebug("Call Hello World from v2");

            await Task.Yield();

            return Ok("Hello Universum from v2");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Hello World v2 could not returned.");
            return this.BadRequestAsProblem(ex);
        }
    }
}
