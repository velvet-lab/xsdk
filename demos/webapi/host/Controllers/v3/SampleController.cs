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
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using xSdk.Demos.Builders;
using xSdk.Demos.Data;
using xSdk.Extensions.Links;
using xSdk.Extensions.Web;

namespace xSdk.Demos.Controllers.v3;

[
    ApiController,
    Route("api/v{version:apiVersion}/[controller]"),
    ApiVersion(3),
    AdvertiseApiVersions("3"),
    Produces("application/json"),
    Consumes("application/json"),
    ProducesErrorResponseType(typeof(ProblemDetails))
]
public sealed class SampleController(IValidator<SampleModel> validator, ILinksService linksService, ILogger<SampleController> logger) : ControllerBase
{
    [
        HttpGet(),
        MapToApiVersion(3),
        EndpointName(nameof(GetSamplesAsync)),
        EndpointSummary("Sends all samples model without hateoas links back"),
        EndpointDescription("Demostrates all samples model without hateoas links"),
        ProducesResponseType<IEnumerable<SampleModel>>(StatusCodes.Status200OK, "application/json", Description = "All sample models without hateoas links"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Sample"),
    ]
    public async Task<ActionResult<IEnumerable<SampleModel>>> GetSamplesAsync(CancellationToken token = default)
    {
        try
        {
            logger.LogDebug("Call get all Sample without Hateoas Links");

            await Task.Yield();

            return Ok(SampleDatabase.Load());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Samples could not returned.");
            return this.BadRequestAsProblem(ex);
        }
    }

    [
        HttpGet("hateoas", Name = nameof(GetSamplesHateOasAsync)),
        Authorize(Policy = AuthenticationPluginBuilder.Policy_OnlyRead),
        MapToApiVersion(3),
        EndpointName(nameof(GetSamplesHateOasAsync)),
        EndpointSummary("Sends all samples model with hateoas links back"),
        EndpointDescription("Demostrates all samples model with hateoas links. Requires authentication"),
        ProducesResponseType<IEnumerable<SampleModel>>(StatusCodes.Status200OK, "application/json", Description = "All sample models with hateoas links"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Sample"),
    ]
    public async Task<ActionResult<IEnumerable<SampleModel>>> GetSamplesHateOasAsync(CancellationToken token = default)
    {
        try
        {
            logger.LogDebug("Call get all Samples with Hateoas Links");

            var database = SampleDatabase.Load();
            foreach (var model in database)
            {
                await linksService.AddLinksAsync(model);
            }

            return Ok(database);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Samples could not returned.");
            return this.BadRequestAsProblem(ex);
        }
    }

    [
        HttpGet("hateoas/{id}", Name = nameof(GetSampleHateOasAsync)),
        Authorize(Policy = AuthenticationPluginBuilder.Policy_OnlyRead),
        MapToApiVersion(3),
        EndpointName(nameof(GetSampleHateOasAsync)),
                EndpointSummary("Sends a sample model with hateoas links back"),
        EndpointDescription("Demostrates a sample model with hateoas links. Requires authentication"),
        ProducesResponseType<SampleModel>(StatusCodes.Status200OK, "application/json", Description = "The sample model with hateoas links"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Sample"),
    ]
    public async Task<ActionResult<IEnumerable<SampleModel>>> GetSampleHateOasAsync(string id, CancellationToken token = default)
    {
        try
        {
            logger.LogDebug("Call get Sample with Hateoas Links");

            var database = SampleDatabase.Load();
            // var item = database.Single(x => x.Id == id);
            var item = database.First();
            await linksService.AddLinksAsync(item);

            return Ok(item);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Sample could not returned.");
            return this.BadRequestAsProblem(ex);
        }
    }

    [
        HttpPost("hateoas", Name = nameof(SaveSampleHateOasAsync)),
        Authorize(Policy = AuthenticationPluginBuilder.Policy_ReadAndWrite),
        MapToApiVersion(3),
        EndpointName(nameof(SaveSampleHateOasAsync)),
        EndpointSummary("Saves a sample model"),
        EndpointDescription("Demostrates saving a sample model. Requires authentication"),
        ProducesResponseType<bool>(StatusCodes.Status200OK, "application/json", Description = "True if saved, otherwise false"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Sample"),
    ]
    public async Task<ActionResult<bool>> SaveSampleHateOasAsync(
        [FromBody]
        SampleModel model,
        CancellationToken token = default)
    {
        try
        {
            logger.LogDebug("Call save Sample");

            await Task.Yield();

            var database = SampleDatabase.Load();
            database.Add(model);

            return Ok(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Sample could not saved.");
            return this.BadRequestAsProblem(ex);
        }
    }

    [
        HttpPut("hateoas/{id}", Name = nameof(UpdateSampleHateOasAsync)),
        Authorize(Policy = AuthenticationPluginBuilder.Policy_ReadAndWrite),
        MapToApiVersion(3),
        EndpointName(nameof(UpdateSampleHateOasAsync)),
        EndpointSummary("Updates a sample model"),
        EndpointDescription("Demostrates updating a sample model. Requires authentication"),
        ProducesResponseType<bool>(StatusCodes.Status200OK, "application/json", Description = "True if updated, otherwise false"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Sample"),
    ]
    public async Task<ActionResult<bool>> UpdateSampleHateOasAsync(
        string id,
        [FromBody]
        SampleModel model,
        CancellationToken token = default)
    {
        try
        {
            logger.LogDebug("Call save Sample");

            await Task.Yield();

            var database = SampleDatabase.Load();
            var item = database.Single(x => x.Id == id);
            database.Remove(item);
            database.Add(model);

            return Ok(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Sample could not saved.");
            return this.BadRequestAsProblem(ex);
        }
    }

    [
        HttpDelete("hateoas/{id}", Name = nameof(DeleteSampleHateOasAsync)),
        Authorize(Policy = AuthenticationPluginBuilder.Policy_ReadAndWrite),
        MapToApiVersion(3),
        EndpointName(nameof(DeleteSampleHateOasAsync)),
        EndpointSummary("Removes a sample model"),
        EndpointDescription("Demostrates removing a sample model. Requires authentication"),
        ProducesResponseType<bool>(StatusCodes.Status200OK, "application/json", Description = "True if removed, otherwise false"),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json"),
        Tags("Sample"),
    ]
    public async Task<ActionResult<bool>> DeleteSampleHateOasAsync(string id, CancellationToken token = default)
    {
        try
        {
            logger.LogDebug("Call delete Sample");

            await Task.Yield();

            var database = SampleDatabase.Load();
            var item = database.Single(x => x.Id == id);
            database.Remove(item);

            return Ok(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Sample could not removed.");
            return this.BadRequestAsProblem(ex);
        }
    }
}
