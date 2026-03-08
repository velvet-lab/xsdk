using xSdk.Demos.Controllers.v3;
using xSdk.Demos.Data;
using xSdk.Extensions.Links;
using xSdk.Extensions.Plugin;

namespace xSdk.Demos.Builders;

internal class LinksPluginBuilder : PluginBuilderBase, ILinksPluginBuilder
{
    public void ConfigureLinks(LinksOptions options)
    {
        options
            .AddPolicy<SampleModel>(policy =>
            {
                policy
                    .RequireRoutedLink("all", nameof(SampleController.GetSamplesHateOasAsync))
                    .RequireRoutedLink("new", nameof(SampleController.SaveSampleHateOasAsync))
                    .RequireRoutedLink("self", nameof(SampleController.GetSampleHateOasAsync), x => new { Id = x.Id })
                    .RequireRoutedLink("edit", nameof(SampleController.UpdateSampleHateOasAsync), x => new { Id = x.Id })
                    .RequireRoutedLink("delete", nameof(SampleController.DeleteSampleHateOasAsync), x => new { Id = x.Id });
            });
    }
}
