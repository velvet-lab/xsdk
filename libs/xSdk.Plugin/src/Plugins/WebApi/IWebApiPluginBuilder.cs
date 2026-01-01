using xSdk.Extensions.Plugin;
using Microsoft.AspNetCore.Mvc;

namespace xSdk.Plugins.WebApi
{
    public interface IWebApiPluginBuilder : IPluginBuilder
    {
        void ConfigureMvc(MvcOptions options);
    }
}
