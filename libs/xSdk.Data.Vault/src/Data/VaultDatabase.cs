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
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods.Token;

namespace xSdk.Data;

internal class VaultDatabase(ILogger<VaultDatabase> logger) : Database(logger)
{
    public override TDatabaseObject? Open<TDatabaseObject>() where TDatabaseObject : class
    {
        VaultDatabaseOptions? setup = GetOptions<VaultDatabaseOptions>(OptionsScope.Datalayer);

        if (setup.AuthMethod != AuthMethods.None)
        {
            IAuthMethodInfo? authMethod = default;
            if (setup.AuthMethod == AuthMethods.AppRole)
            {
                AppRoleAuthOptions? options = GetOptions<AppRoleAuthOptions>(OptionsScope.Datalayer);
                if (options != null)
                {
                    authMethod = new AppRoleAuthMethodInfo(options.RoleId, options.Secret);
                }
            }
            else if (setup.AuthMethod == AuthMethods.Token)
            {
                TokenAuthOptions? options = GetOptions<TokenAuthOptions>(OptionsScope.Datalayer);
                if (options != null)
                {
                    authMethod = new TokenAuthMethodInfo(options.Token);
                }
            }
            else if (setup.AuthMethod == AuthMethods.UsernamePassword)
            {
                throw new SdkException("Username/Password authentication method is not implemented yet.");
            }
            else if (setup.AuthMethod == AuthMethods.Cert)
            {
                throw new SdkException("Certificate authentication method is not implemented yet.");
            }

            var host = setup.Endpoint;

            var settings = new VaultClientSettings($"{host}", authMethod);            

            return new VaultClient(settings) as TDatabaseObject;
        }

        return default;
    }
}
