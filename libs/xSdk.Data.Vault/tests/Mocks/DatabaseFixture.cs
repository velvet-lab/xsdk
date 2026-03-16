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

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Hosting;

namespace xSdk.Data.Mocks;

public class DatabaseFixture : DatabaseHostFixture
{
    private readonly IContainer? _container = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            try
            {
                _container?.StopAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch
            {
                // Nothing to tell
            }
        }
        base.Dispose(disposing);
    }

    protected override void Initialize()
    {
        ConfigureServices(services =>
        {
            services
                .AddDatalayer(builder =>
                {
                    var imageName = Environment.GetEnvironmentVariable("VAULT_IMAGE_NAME");
                    if (string.IsNullOrEmpty(imageName))
                    {
                        throw new SdkException("The environment variable VAULT_IMAGE_NAME is not defined.");
                    }

                    var container = new ContainerBuilder()
                        .WithImage(imageName)
                        .WithPortBinding(8200, true)
                        .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8200)))
                        .WithImagePullPolicy(PullPolicy.Always)
                        .Build();

                    container.StartAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    var port = container.GetMappedPublicPort(8200);
                    var (stdout, stderr) = container.GetLogsAsync(timestampsEnabled: false).GetAwaiter().GetResult();

                    var splitted = stdout.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    var rootToken = splitted.Where(x => x.IndexOf("Root Token:") > -1).FirstOrDefault()?.Replace("Root Token:", "").Trim();
                    var unsealKey = splitted.Where(x => x.IndexOf("Unseal Key:") > -1).FirstOrDefault()?.Replace("Unseal Key:", "").Trim();

                    builder
                        // Enable Vault
                        .UseVault(
                            Globals.DatalayerName,
                            true,
                            _ =>
                            {
                                _.Host = $"http://localhost:{port}";
                                _.TokenAuth = new() { Token = rootToken };
                            }
                        );
                });
        });
    }
}
