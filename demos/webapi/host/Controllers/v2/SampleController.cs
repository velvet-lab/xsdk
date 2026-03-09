using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Web;

namespace xSdk.Demos.Controllers.V2;

[ApiVersion(2)]
[AdvertiseApiVersions("2")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
public sealed class SampleController(ILogger<SampleController> logger) : ControllerBase
{
    /// <summary>
    /// Sends a Hello World in Version 2 back
    /// </summary>
    [HttpGet()]
    [MapToApiVersion(2)]
    [Authorize]
    public async Task<ActionResult> GetSampleAsync(CancellationToken token = default)
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
