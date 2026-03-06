using Asp.Versioning;
using xSdk.Demos.Builders;
using xSdk.Extensions.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace xSdk.Demos.Controllers
{
    [ApiVersion(1, Deprecated = true)]
    [AdvertiseApiVersions("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public sealed class SampleController(ILogger<SampleController> logger) : ControllerBase
    {
        /// <summary>
        /// Sends a Hello World back
        /// </summary>
        [HttpGet()]
        [MapToApiVersion(1)]
        [Authorize]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetSampleAsync(CancellationToken token = default)
        {
            try
            {
                logger.LogDebug("Call Hello World from v1");

                await Task.Yield();

                return Ok("Hello World from v1");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Hello World could not returned.");
                return this.BadRequestAsProblem(ex);
            }
        }


        [HttpGet("read")]
        [MapToApiVersion(1)]
        [Authorize(Policy = AuthenticationPluginBuilder.Policy_OnlyRead)]
        [SwaggerOperation(
            Summary = "Loads data for readonly users",
            Description = "Requires authentication",
            OperationId = nameof(GetOnlyReadAsync),
            Tags = new[] { "Sample" }
        )]
        public async Task<ActionResult> GetOnlyReadAsync(CancellationToken token = default)
        {
            try
            {
                logger.LogDebug("Demostrate Read");

                await Task.Yield();

                foreach (var claim in this.HttpContext.User.Claims)
                {
                    await Console.Out.WriteLineAsync(claim.ToString());
                }

                return Ok("Read was allowed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while read.");
                return this.BadRequestAsProblem(ex);
            }
        }

        [HttpGet("write")]
        [MapToApiVersion(1)]
        [Authorize(Policy = AuthenticationPluginBuilder.Policy_ReadAndWrite)]
        [SwaggerOperation(
            Summary = "Writes data",
            Description = "Requires authentication",
            OperationId = nameof(GetReadAndWriteAsync),
            Tags = new[] { "Sample" }
        )]
        public async Task<ActionResult> GetReadAndWriteAsync(CancellationToken token = default)
        {
            try
            {
                logger.LogDebug("Demostrate Read and Write");

                foreach (var claim in this.HttpContext.User.Claims)
                {
                    await Console.Out.WriteLineAsync(claim.ToString());
                }

                await Task.Yield();

                return Ok("Read and Write are allowed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "API: Error while read or write.");
                return this.BadRequestAsProblem(ex);
            }
        }
    }
}
