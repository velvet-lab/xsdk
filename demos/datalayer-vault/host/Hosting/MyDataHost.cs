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
