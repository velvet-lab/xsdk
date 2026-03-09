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
using DotNet.Testcontainers.Images;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using xSdk;
using xSdk.Data;
using xSdk.Demos.Hosting;
using xSdk.Hosting;

const string APP_NAME = "datalayer-vault";
const string APP_COMPANY = "xdemos";
const string APP_PREFIX = "dv";

//// Prepare Testcontainer
//var container = new ContainerBuilder()
//    .WithImage("packages.repo.dvint.de/docker-central-proxy/hashicorp/vault:1.18")
//    .WithPortBinding(8200, true)
//    .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8200))
//    .WithImagePullPolicy(PullPolicy.Always)
//    .Build();

//await container.StartAsync();
//var port = container.GetMappedPublicPort(8200);
//var (stdout, stderr) = container.GetLogsAsync(timestampsEnabled: false).GetAwaiter().GetResult();

//var splitted = stdout.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
//var rootToken = splitted.Where(x => x.IndexOf("Root Token:") > -1).FirstOrDefault()?.Replace("Root Token:", "").Trim();
//var unsealKey = splitted.Where(x => x.IndexOf("Unseal Key:") > -1).FirstOrDefault()?.Replace("Unseal Key:", "").Trim();

var host = xSdk.Hosting.Host
    .CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX)
    .ConfigureServices((context, services) =>
    {
        services
            // Sample for Vault Datalayer
            .AddDatalayer(builder =>
            {
                var vaultSetup = SlimHost.Instance.VariableSystem.GetSetup<VaultSetup>();
                var approleAuth = SlimHost.Instance.VariableSystem.GetSetup<AppRoleAuthSetup>();

                builder
                    .UseVault("MyVaultDatabase", _ =>
                    {
                        // _.Endpoint = $"http://localhost:{port}";
                        // _.TokenAuth = new() { Token = rootToken };
                        _.Host = "https://vault.localhost";
                        // _.CertAuth = new() { Certificate = approleAuth?.CreateCertificate() };
                        _.AppRoleAuth = new()
                        {
                            RoleId = approleAuth?.RoleId,
                            Secret = approleAuth?.Secret,
                        };
                        _.PathFormat = (stage, path) =>
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
                    });
            })
            .AddHostedService<MyDataHost>();
    })
    .EnableVaultAuth()
    .Build();

var logger = LogManager.GetCurrentClassLogger();
logger.Info("Starting {AppName}", APP_NAME);

await host.RunAsync();
