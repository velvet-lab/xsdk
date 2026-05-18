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

namespace xSdk.Controllers;

[
    ApiVersion(1),
    ApiController,
    Route("api/v{version:apiVersion}/[controller]"),
    Produces("text/plain"),
    ProducesErrorResponseType(typeof(ProblemDetails))
]
public class HealthController(ILogger<HealthController> logger) : ControllerBase
{
    [
        HttpGet(),
        MapToApiVersion(1),
        AllowAnonymous,
        EndpointName(nameof(GetStatus)),
        EndpointSummary("Gets current status of API"),
        EndpointDescription("It could be used for Health Checks"),
        ProducesResponseType<string>(StatusCodes.Status200OK, "text/plain", Description = "OK, if everything is fine"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Health"),
    ]
    public async Task<ActionResult> GetStatus()
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

    [
        HttpGet("ping"),
        MapToApiVersion(1),
        AllowAnonymous,
        EndpointName(nameof(GetPong)),
        EndpointSummary("Sends a ping"),
        EndpointDescription("Sends a ping and await a pong. Could be used for Health Checks"),
        ProducesResponseType<string>(StatusCodes.Status200OK, "text/plain", Description = "Returns Pong, if everything is fine"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Health"),
    ]
    public async Task<ActionResult> GetPong()
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
