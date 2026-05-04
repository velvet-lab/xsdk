/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using xSdk.Demos.Builders;
using xSdk.Extensions.Web;

namespace xSdk.Demos.Controllers;

[
    ApiController,
    Route("api/v{version:apiVersion}/[controller]"),
    ApiVersion(1, Deprecated = true),
    AdvertiseApiVersions("1"),
    Produces("application/json"),
    Consumes("application/json"),
    ProducesErrorResponseType(typeof(ProblemDetails))
]
public sealed class SampleController(ILogger<SampleController> logger) : ControllerBase
{
    [
        HttpGet(),
        MapToApiVersion(1),
        AllowAnonymous,
        //Authorize
        EndpointName(nameof(GetSampleAsync)),
        EndpointSummary("Sends a Hello World back"),
        EndpointDescription("Sends a Hello World back in Version 1"),
        ProducesResponseType<string>(StatusCodes.Status200OK, "text/plain", Description = "Sends a Hello World back"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Sample"),
    ]
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


    [
        HttpGet("read"),
        MapToApiVersion(1),
        Authorize(Policy = AuthenticationPluginBuilder.Policy_OnlyRead),
        EndpointName(nameof(GetOnlyReadAsync)),
        EndpointSummary("Loads data for readonly users"),
        EndpointDescription("Requires authentication"),
        ProducesResponseType<string>(StatusCodes.Status200OK, "text/plain", Description = "If allowed a message will be returned"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Sample"),
    ]
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

    [
        HttpPost("write"),
        MapToApiVersion(1),
        Authorize(Policy = AuthenticationPluginBuilder.Policy_ReadAndWrite),
        EndpointName(nameof(GetReadAndWriteAsync)),
        EndpointSummary("Writes data"),
        EndpointDescription("Requires authentication"),
        ProducesResponseType<string>(StatusCodes.Status200OK, "text/plain", Description = "If allowed a message will be returned"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Sample"),
    ]
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
