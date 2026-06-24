using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Options;

namespace xSdk.Hosting;

public static class HostBuilderContextExtensions
{
    extension(HostBuilderContext context)
    {
        public void EnrichEnvironment(EnvironmentOptions options)
        {            
            if (context.HostingEnvironment.EnvironmentName != options.StageAsString)
            {
                context.HostingEnvironment.EnvironmentName = options.StageAsString;
            }
        }
    }
}
