using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using xSdk.Demos.Builders;
using xSdk.Demos.Data;
using xSdk.Extensions.Links;
using xSdk.Extensions.Web;

namespace xSdk.Demos.Controllers.v3;

[ApiVersion(3)]
[AdvertiseApiVersions("3")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
public sealed class SampleController(IValidator<SampleModel> validator, ILinksService linksService, ILogger<SampleController> logger) : ControllerBase
{
    [
        HttpGet(),
        MapToApiVersion(3),
        SwaggerOperation(
            Summary = "Sends all samples model without hateoas links back",
            OperationId = nameof(GetSamplesAsync),
            Tags = new[] { "Sample" }
        ),
        SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<SampleModel>), Description = "All sample models without hateoas links"),
        SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))
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
        SwaggerOperation(
            Summary = "Sends all samples model with hateoas links back",
            OperationId = nameof(GetSamplesHateOasAsync),
            Tags = new[] { "Sample" }
        ),
        SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<SampleModel>), Description = "All sample models with hateoas links"),
        SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))
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
        SwaggerOperation(
            Summary = "Sends a sample model with hateoas links back",
            OperationId = nameof(GetSampleHateOasAsync),
            Tags = new[] { "Sample" }
        ),
        SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<SampleModel>), Description = "The sample model with hateoas links"),
        SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))
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
        SwaggerOperation(
            Summary = "Saves a sample model",
            OperationId = nameof(SaveSampleHateOasAsync),
            Tags = new[] { "Sample" }
        ),
        SwaggerResponse(StatusCodes.Status200OK, Description = "True if saved, otherwise false", Type = typeof(bool)),
        SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))
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
        SwaggerOperation(
            Summary = "Saves a sample model",
            OperationId = nameof(UpdateSampleHateOasAsync),
            Tags = new[] { "Sample" }
        ),
        SwaggerResponse(StatusCodes.Status200OK, Description = "True if saved, otherwise false", Type = typeof(bool)),
        SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))
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
        SwaggerOperation(
            Summary = "Removes a sample model",
            OperationId = nameof(DeleteSampleHateOasAsync),
            Tags = new[] { "Sample" }
        ),
        SwaggerResponse(StatusCodes.Status200OK, Description = "True if removed, otherwise false", Type = typeof(bool)),
        SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))
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
