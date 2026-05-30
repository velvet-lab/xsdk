using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace xSdk.Extensions.Options;

public static class EnvironmentOptionsExtensions
{
    public static EnvironmentOptions PostConfigure(this EnvironmentOptions options, ApplicationOptions appOptions)
    {
        var serviceDescription = ServiceDescription.Create(appOptions);
        options.InitializeService(serviceDescription);

        return options;
    }
}
