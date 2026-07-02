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
using Microsoft.Extensions.Options;
using xSdk.Extensions.Options;

namespace xSdk.Demos.Hosting;

public class MyCustomHost : IHostedService
{
    private readonly ILogger<MyCustomHost> _logger;
    private readonly OptionsWithoutPrefix _optionsWithoutPrefix;
    private readonly OptionsWithPrefix _optionsWithPrefix;
    private readonly EnvironmentOptions _environment;

    public MyCustomHost(IOptions<EnvironmentOptions> environmentOptions, IOptions<OptionsWithPrefix> optionsWithPrefix, IOptions<OptionsWithoutPrefix> optionsWithoutPrefix, ILogger<MyCustomHost> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _optionsWithoutPrefix = optionsWithoutPrefix.Value ?? throw new ArgumentNullException(nameof(optionsWithoutPrefix));
        _optionsWithPrefix = optionsWithPrefix.Value ?? throw new ArgumentNullException(nameof(optionsWithPrefix));

        _environment = environmentOptions.Value ?? throw new ArgumentNullException(nameof(environmentOptions));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();

        _logger.LogInformation("Host was started");

        _logger.LogInformation("{0} = {1}", nameof(this._optionsWithPrefix.WithAppPrefix_WithSetupPrefix), this._optionsWithPrefix.WithAppPrefix_WithSetupPrefix);
        _logger.LogInformation("{0} = {1}", nameof(this._optionsWithPrefix.NoAppPrefix_NoSetupPrefix), this._optionsWithPrefix.NoAppPrefix_NoSetupPrefix);

        _logger.LogInformation("{0} = {1}", nameof(this._optionsWithoutPrefix.NoAppPrefix_NoSetupPrefix), this._optionsWithoutPrefix.NoAppPrefix_NoSetupPrefix);
        _logger.LogInformation("{0} = {1}", nameof(this._optionsWithoutPrefix.WithAppPrefix_NoSetupPrefix), this._optionsWithoutPrefix.WithAppPrefix_NoSetupPrefix);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
