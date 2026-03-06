using xSdk.Extensions.Variable;
using xSdk.Hosting;
using xSdk.Shared;
using NLog;
using System.Text.Json;
using VaultSharp;
using VaultSharp.V1.Commons;

namespace xSdk.Data
{
    internal partial class ReadOnlyVaultRepository : Repository, IReadOnlyVaultRepository
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected override VaultDatabase Database => base.Database as VaultDatabase;

        public async Task<IDictionary<string, string>> GetSecretsAsync(string? mountPoint, string path, CancellationToken token = default)
        {
            var result = new Dictionary<string, string>();

            try
            {
                var client = Database.Open<VaultClient>();

                var pathFormater = this.Database.Setup.PathFormat;
                if (pathFormater != null)
                {
                    var env = SlimHost.Instance.VariableSystem.GetSetup<EnvironmentSetup>();
                    path = pathFormater(env.Stage, path);
                }

                Secret<SecretData>? secret = null;

                mountPoint = ValidateMountPoint(mountPoint);
                if (!string.IsNullOrEmpty(mountPoint))
                {
                    secret = await client.V1.Secrets.KeyValue.V2.ReadSecretAsync(path, mountPoint: mountPoint);
                }
                else
                {
                    secret = await client.V1.Secrets.KeyValue.V2.ReadSecretAsync(path);
                }

                if (secret != null)
                {
                    var data = secret.Data.Data;
                    if (data == null || data.Count == 0)
                    {
                        throw new SdkException($"No Secrets found in Vault '{path}'");
                    }

                    foreach (var item in data)
                    {
                        var value = item.Value;
                        if (value != null)
                        {
                            string? itemValue = null;

                            if (value is JsonElement element)
                            {
                                itemValue = element.GetString();
                            }
                            else
                            {
                                itemValue = TypeConverter.ConvertTo<string>(value);
                            }

                            if (!string.IsNullOrEmpty(itemValue))
                            {
                                result.Add(item.Key, itemValue);
                            }
                        }
                    }
                }
                else
                {
                    throw new SdkException($"No Secrets found in Vault '{path}'");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "A Error occured while Vault will readed");
                throw;
            }

            return result;
        }

        protected string? ValidateMountPoint(string? mountPoint)
        {
            string defaultMountPoint = "secret";
            if (string.IsNullOrEmpty(mountPoint))
            {
                return defaultMountPoint;
            }

            return mountPoint;
        }
    }
}
