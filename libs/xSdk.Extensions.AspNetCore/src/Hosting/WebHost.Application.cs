/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace xSdk.Hosting;

public static partial class WebHost
{
    private static void ConfigureApplicationWithContext(WebHostBuilderContext context, IApplicationBuilder app, SlimHost slimHost, ILogger logger)
    {
        logger.LogInformation("Configuring application services");

        var plugins = slimHost.GetPluginHosts<WebPluginHost>();

        // Only the first Plugin needs to configure defaults
        var firstPlugin = plugins.FirstOrDefault();
        if (firstPlugin != null)
        {
            firstPlugin.ConfigureDefaults(context, app);
        }

        foreach (var plugin in plugins)
        {
            plugin.Configure(context, app);
        }

        app.UseEndpoints(builder =>
        {
            foreach (var plugin in plugins)
            {
                plugin.ConfigureEndpoint(builder);
            }
        });
    }
}
