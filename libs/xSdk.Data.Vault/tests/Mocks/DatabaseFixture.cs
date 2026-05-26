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
using xSdk.Hosting;

namespace xSdk.Data.Mocks;

public class DatabaseFixture : DatabaseHostFixture
{
    private readonly IContainer? _container = null;

    private const string VAULT_IMAGE_NAME = "openbao/openbao:2.5.2";

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
        ConfigureBuilder(hostBuilder =>
        {
            hostBuilder
                .AddDatalayer(builder =>
                 {
                     IContainer container = new ContainerBuilder(VAULT_IMAGE_NAME)
                         .WithPortBinding(8200, true)
                         .WithWaitStrategy(Wait.ForUnixContainer()
                             .UntilHttpRequestIsSucceeded(r => r.ForPort(8200))
                             .UntilMessageIsLogged("Root Token:"))
                         .WithImagePullPolicy(PullPolicy.Missing)
                         .Build();

                     container.StartAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                     ushort port = container.GetMappedPublicPort(8200);
                     (string? stdout, string? stderr) = container.GetLogsAsync(timestampsEnabled: false).GetAwaiter().GetResult();

                     string[] splitted = stdout.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                     string? rootToken = splitted.FirstOrDefault(x => x.IndexOf("Root Token:") > -1)?.Replace("Root Token:", "").Trim();
                     string? unsealKey = splitted.FirstOrDefault(x => x.IndexOf("Unseal Key:") > -1)?.Replace("Unseal Key:", "").Trim();

                     builder
                        // Enable Vault
                        .UseVault(true, _ =>
                        {
                            _.Endpoint = $"http://localhost:{port}";
                            _.AuthMethod = AuthMethods.Token;
                        })
                        .ConfigureAuth<TokenAuthOptions>(options =>
                        {
                            options.Token = rootToken;
                        });
                 });
        });
    }
}
