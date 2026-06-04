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

// Before you can use the Datalayer you have to create a AppRole Auth Method and a Role
//
// Enable Auth Method
// vault auth enable approle path=approle4tests
//
// Configure a new Role
// vault write auth/approle4tests/role/my-test-role secret_id_ttl=10m token_num_uses=10 token_ttl=20m token_max_ttl=30m secred_id_num_uses=40 token_policies=acl4tests
//
// Retrieve the Role ID
// vault read auth/approle4tests/role/my-test-role/role-id
//
// Retrieve the Secret ID
// vault write -f auth/approle4tests/role/my-test-role/secret-id

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using xSdk;
using xSdk.Data;
using xSdk.Demos.Hosting;
using xSdk.Hosting;

const string APP_NAME = "datalayer-vault";
const string APP_COMPANY = "xdemos";
const string APP_PREFIX = "dv";

// Prepare Testcontainer
IContainer container = new ContainerBuilder("openbao/openbao:2.5.2")
    .WithPortBinding(8200, true)
    .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("Development mode should NOT be used in production installations!"))
    .WithImagePullPolicy(PullPolicy.Always)
    .Build();

await container.StartAsync();
ushort port = container.GetMappedPublicPort(8200);
(string? stdout, string? _) = container.GetLogsAsync(timestampsEnabled: false).GetAwaiter().GetResult();

string[] splitted = stdout.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
string? rootToken = splitted.FirstOrDefault(x => x.IndexOf("Root Token:") > -1)?.Replace("Root Token:", "").Trim();
string? unsealKey = splitted.FirstOrDefault(x => x.IndexOf("Unseal Key:") > -1)?.Replace("Unseal Key:", "").Trim();

IHost host = xSdk.Hosting.Host
    .CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX)
    .AddDatalayer(builder => builder
            .UseVault(true, _ =>
            {
                _.AuthMethod = AuthMethods.Token;
                _.Endpoint = $"http://localhost:{port}";
                _.PathFormatFactory = (stage, path) =>
                {
                    if (stage == Stage.Development)
                    {
                        return string.Format(path, "dev");
                    }

                    if (stage == Stage.Integration)
                    {
                        return string.Format(path, "qs");
                    }

                    if (stage == Stage.PreProduction)
                    {
                        return string.Format(path, "pp");
                    }

                    if (stage == Stage.Production)
                    {
                        return string.Format(path, "prod");
                    }

                    return path;
                };
            })
            .ConfigureAuth<TokenAuthOptions>(options => options.Token = rootToken))
    .AddHost<MyDataHost>()
    .Build();

ILogger logger = LogManager.GetCurrentClassLogger();
logger.LogInformation("Starting {AppName}", APP_NAME);

await host.RunAsync();
