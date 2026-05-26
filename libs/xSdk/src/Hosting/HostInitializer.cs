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

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Plugin;

namespace xSdk.Hosting;

internal sealed class HostInitializer(IPluginService pluginService, IPluginHostCollection pluginHostCollection, ILoggerFactory factory) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Initialize the LogManager with the provided ILoggerFactory
        LogManager.Initialize(factory);        

        // Register plugins with the plugin service
        foreach (Type pluginType in pluginHostCollection)
        {
            await pluginService.AddPluginAsync(pluginType, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
