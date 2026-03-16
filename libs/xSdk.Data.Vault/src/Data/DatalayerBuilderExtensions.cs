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

namespace xSdk.Data;

public static class DatalayerBuilderExtensions
{
    public static IDatalayerBuilder UseVault(this IDatalayerBuilder builder, string name, Action<VaultDatabaseSetup> configure)
        => builder.UseVault(name, false, configure);

    public static IDatalayerBuilder UseVault(this IDatalayerBuilder builder, string name, bool enableWrite, Action<VaultDatabaseSetup> configure)
    {
        builder.UseDatabase<VaultDatabase, VaultDatabaseSetup, VaultConnectionBuilder>(name, configure);
        builder.MapRepository<IReadOnlyVaultRepository, ReadOnlyVaultRepository>(name);

        if (enableWrite)
        {
            builder.MapRepository<IVaultRepository, VaultRepository>(name);
        }

        return builder;
    }
}
