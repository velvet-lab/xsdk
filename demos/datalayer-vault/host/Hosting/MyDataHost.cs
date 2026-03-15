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
using xSdk.Data;

namespace xSdk.Demos.Hosting;

public class MyDataHost(IDatalayerFactory factory, ILogger<MyDataHost> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Request informations from vault");

        var repo = factory.CreateRepository<IReadOnlyVaultRepository>();

        var secrets = await repo.GetSecretsAsync("kv", "groups/{0}/portal/azure");
        foreach (var kvp in secrets)
            System.Console.WriteLine("Secret for Key '{0}' is '{1}'", kvp.Key, kvp.Value);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
