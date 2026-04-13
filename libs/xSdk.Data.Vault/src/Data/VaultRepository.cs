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

using Microsoft.Extensions.Logging;
using VaultSharp;
using xSdk.Extensions.Options;
using xSdk.Hosting;

namespace xSdk.Data;

internal partial class VaultRepository : ReadOnlyVaultRepository, IVaultRepository
{
    private static readonly ILogger _logger = LogManager.CreateLogger<VaultRepository>();

    public async Task<bool> AddSecretAsync(string? mountPoint, string path, Dictionary<string, object> data, CancellationToken token = default)
    {
        IDatabase? database = DatabaseHandler?.Retrieve();

        if (database != null)
        {
            try
            {
                VaultDatabaseOptions? setup = GetOptions<VaultDatabaseOptions>(OptionsScope.Datalayer);
                if (setup != null)
                {
                    var pathFormater = setup.PathFormatFactory;
                    if (pathFormater != null)
                    {
                        EnvironmentOptions? env = GetOptions<EnvironmentOptions>();
                        if (env != null)
                        {
                            string? cleanedPath = pathFormater?.Invoke(env.Stage, path);
                            if (cleanedPath != null)
                            {
                                path = cleanedPath;
                            }
                        }
                    }
                }

                VaultClient? client = database.Open<VaultClient>();
                if (client != null)
                {
                    mountPoint = ValidateMountPoint(mountPoint);

                    await client.V1.Secrets.KeyValue.V2.WriteSecretAsync(path, data, mountPoint: mountPoint);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "A Error occured while Vault will readed");
                throw;
            }
            finally
            {
                DatabaseHandler?.Return(database);
            }
        }

        return true;
    }
}
