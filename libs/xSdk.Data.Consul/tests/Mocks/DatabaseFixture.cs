using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
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
                    var imageName = Environment.GetEnvironmentVariable("CONSUL_IMAGE_NAME");
                    if (string.IsNullOrEmpty(imageName))
                    {
                        throw new SdkException("The environment variable CONSUL_IMAGE_NAME is not defined.");
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

                    //builder
                    //    // Enable Vault
                    //    .UseVault(
                    //        Globals.DatalayerName,
                    //        true,
                    //        _ =>
                    //        {
                    //            _.Host = $"http://localhost:{port}";
                    //            _.TokenAuth = new() { Token = rootToken };
                    //        }
                    //    );
                });
        });
    }
}

