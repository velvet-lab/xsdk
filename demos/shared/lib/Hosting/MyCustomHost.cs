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
using xSdk.Extensions.Variable;

namespace xSdk.Demos.Hosting;

public class MyCustomHost : IHostedService
{
    private readonly IVariableService _variableSvc;
    private readonly ILogger<MyCustomHost> _logger;
    private readonly SetupWithoutPrefix _setupWithoutPrefix;
    private readonly SetupWithPrefix _setupWithPrefix;

    public MyCustomHost(IVariableService variableSvc, ILogger<MyCustomHost> logger)
    {
        this._variableSvc = variableSvc ?? throw new ArgumentNullException(nameof(variableSvc));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

        this._setupWithoutPrefix = variableSvc.GetSetup<SetupWithoutPrefix>();
        this._setupWithPrefix = variableSvc.GetSetup<SetupWithPrefix>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();

        _logger.LogInformation("Host was started");

        _logger.LogInformation("{0} = {1}", nameof(this._setupWithPrefix.WithAppPrefix_WithSetupPrefix), this._setupWithPrefix.WithAppPrefix_WithSetupPrefix);
        _logger.LogInformation("{0} = {1}", nameof(this._setupWithPrefix.NoAppPrefix_NoSetupPrefix), this._setupWithPrefix.NoAppPrefix_NoSetupPrefix);

        _logger.LogInformation("{0} = {1}", nameof(this._setupWithoutPrefix.NoAppPrefix_NoSetupPrefix), this._setupWithoutPrefix.NoAppPrefix_NoSetupPrefix);
        _logger.LogInformation("{0} = {1}", nameof(this._setupWithoutPrefix.WithAppPrefix_NoSetupPrefix), this._setupWithoutPrefix.WithAppPrefix_NoSetupPrefix);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
