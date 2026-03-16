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

namespace xSdk.Demos;

internal class PluginHost(ILogger<PluginHost> _logger) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Demostriere das Logging
        _logger.LogInformation("Demostrate logging");
        _logger.LogCritical("This is a critical Message");
        _logger.LogError("This is a error Message");
        _logger.LogWarning("This is a warning Message");
        _logger.LogInformation("This is a info Message");
        _logger.LogDebug("This is a debug Message");
        _logger.LogTrace("This is a trace Message");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
