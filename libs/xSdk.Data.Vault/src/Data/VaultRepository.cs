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
