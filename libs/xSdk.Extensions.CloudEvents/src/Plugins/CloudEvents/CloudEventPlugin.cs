using Microsoft.AspNetCore.Mvc;
using xSdk.Extensions.CloudEvents;
using xSdk.Hosting;

namespace xSdk.Plugins.CloudEvents;

internal class CloudEventPlugin : WebHostPluginBase
{
    public void ConfigureMvc(MvcOptions options)
    {
        var formatter = CloudEventFactory.CreateFormatter();
        options.InputFormatters.Insert(0, new CloudEventJsonInputFormatter(formatter));
    }
}
