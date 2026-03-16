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

internal class MyHost : IHostedService
{
    private readonly LocalService _localSvc;
    private readonly ILogger<MyHost> _logger;

    public MyHost(LocalService localSvc, ILogger<MyHost> logger)
    {
        this._localSvc = localSvc ?? throw new ArgumentNullException(nameof(localSvc));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Now lets start a local service");
        _localSvc.DoWorkA();
        _localSvc.DoWorkB();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
