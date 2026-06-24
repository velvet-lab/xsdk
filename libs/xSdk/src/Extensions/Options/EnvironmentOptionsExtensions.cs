using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace xSdk.Extensions.Options;

public static class EnvironmentOptionsExtensions
{
    extension(EnvironmentOptions options)
    {
        public EnvironmentOptions PostConfigure(ApplicationOptions appOptions)
        {
            var serviceDescription = ServiceDescription.Create(appOptions);
            options.InitializeService(serviceDescription);

            return options;
        }
    }
}
