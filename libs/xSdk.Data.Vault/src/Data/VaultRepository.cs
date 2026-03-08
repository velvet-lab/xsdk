using NLog;
using VaultSharp;
using xSdk.Extensions.Variable;
using xSdk.Hosting;

namespace xSdk.Data;

internal partial class VaultRepository : ReadOnlyVaultRepository, IVaultRepository
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public async Task<bool> AddSecretAsync(string? mountPoint, string path, Dictionary<string, object> data, CancellationToken token = default)
    {
        try
        {
            var client = Database.Open<VaultClient>();

            var pathFormater = this.Database.Setup.PathFormat;
            if (pathFormater != null)
            {
                var env = SlimHost.Instance.VariableSystem.GetSetup<EnvironmentSetup>();
                path = pathFormater(env.Stage, path);
            }

            mountPoint = ValidateMountPoint(mountPoint);
            await client.V1.Secrets.KeyValue.V2.WriteSecretAsync(path, data, mountPoint: mountPoint);
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "A Error occured while Vault will readed");
            throw;
        }

        return true;
    }
}
