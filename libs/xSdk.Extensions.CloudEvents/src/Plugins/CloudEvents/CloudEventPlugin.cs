using xSdk.Extensions.CloudEvents;
using xSdk.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace xSdk.Plugins.CloudEvents
{
    class CloudEventPlugin : WebHostPluginBase
    {
        public void ConfigureMvc(MvcOptions options)
        {
            var formatter = CloudEventFactory.CreateFormatter();
            options.InputFormatters.Insert(0, new CloudEventJsonInputFormatter(formatter));
        }
    }
}
