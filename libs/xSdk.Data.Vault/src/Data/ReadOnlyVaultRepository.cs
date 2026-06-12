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

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using VaultSharp;
using VaultSharp.V1.Commons;
using xSdk.Extensions.Logging;
using xSdk.Extensions.Options;
using xSdk.Tools;

namespace xSdk.Data;

[ExcludeFromCodeCoverage(Justification = "Requires a live HashiCorp Vault instance – integration-only.")]
internal partial class ReadOnlyVaultRepository : Repository, IReadOnlyVaultRepository
{
    private static ILogger Logger => field ??= LogManager.CreateLogger<ReadOnlyVaultRepository>();

    public async Task<IDictionary<string, string>> GetSecretsAsync(string? mountPoint, string path, CancellationToken token = default)
    {
        var result = new Dictionary<string, string>();

        IDatabase? database = DatabaseHandler.Retrieve();

        if (database != null)
        {
            try
            {
                VaultDatabaseOptions? setup = GetOptions<VaultDatabaseOptions>(OptionsScope.Datalayer) ?? throw new SdkException("VaultDatabaseOptions is not configured properly.");
                VaultClient? client = database.Open<VaultClient>() ?? throw new SdkException("Vault client could not be opened from the database.");

                if (setup.PathFormatFactory != null)
                {
                    EnvironmentOptions? env = GetOptions<EnvironmentOptions>() ?? throw new SdkException("EnvironmentOptions is not configured properly.");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    path = setup.PathFormatFactory?.Invoke(env.Stage, path);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
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
                    IDictionary<string, object> data = secret.Data.Data;
                    if (data == null || data.Count == 0)
                    {
                        throw new SdkException($"No Secrets found in Vault '{path}'");
                    }

                    foreach (KeyValuePair<string, object> item in data)
                    {
                        object value = item.Value;
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
                Logger.LogCritical(ex, "A Error occured while Vault will readed");
                throw;
            }
            finally
            {
                DatabaseHandler.Return(database);
            }
        }

        return result;
    }

    protected static string? ValidateMountPoint(string? mountPoint)
    {
        string defaultMountPoint = "secret";
        if (string.IsNullOrEmpty(mountPoint))
        {
            return defaultMountPoint;
        }

        return mountPoint;
    }
}
