using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.WebApi;

namespace xSdk.Plugins.WebApi;

internal class DefaultWebApiPluginBuilder : PluginBuilder, IWebApiPluginBuilder
{
    public void ConfigureMvc(MvcOptions options)
    {

    }
}
