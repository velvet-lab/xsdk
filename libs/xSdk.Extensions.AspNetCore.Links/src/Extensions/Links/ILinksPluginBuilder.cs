using xSdk.Extensions.Plugin;

namespace xSdk.Extensions.Links;

public interface ILinksPluginBuilder : IPluginBuilder
{
    void ConfigureLinks(LinksOptions options);
}
