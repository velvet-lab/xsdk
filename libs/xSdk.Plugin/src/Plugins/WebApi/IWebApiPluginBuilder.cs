using Microsoft.AspNetCore.Mvc;
using xSdk.Extensions.Plugin;

namespace xSdk.Plugins.WebApi;

public interface IWebApiPluginBuilder : IPluginBuilder
{
    void ConfigureMvc(MvcOptions options);
}
