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
        EndpointName(nameof(GetSampleAsync)),
        EndpointSummary("Sends a Hello World in Version 2 back"),
        EndpointDescription("Demostrates a hello world in version 2"),
        ProducesResponseType<string>(StatusCodes.Status200OK, "text/plain", Description = "Sends a Hello World back in Version 2"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Sample"),
    ]
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
