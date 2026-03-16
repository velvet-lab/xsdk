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

using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods.Cert;
using VaultSharp.V1.AuthMethods.JWT;
using VaultSharp.V1.AuthMethods.LDAP;
using VaultSharp.V1.AuthMethods.OCI;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods.UserPass;

namespace xSdk.Data;

internal class VaultDatabase : Database
{
    internal VaultDatabaseSetup Setup { get; private set; }

    protected override TConnection Open<TConnection>(Func<object> connectionStringBuilder)
    {
        var setup = connectionStringBuilder() as VaultDatabaseSetup;

        IAuthMethodInfo? authMethod = default;
        if (setup.AuthMethod == VaultAuthenticationMethod.AppRole)
        {
            authMethod = new AppRoleAuthMethodInfo(setup.AppRoleAuth.RoleId, setup.AppRoleAuth.Secret);
        }
        else if (setup.AuthMethod == VaultAuthenticationMethod.Jwt)
        {
            authMethod = new JWTAuthMethodInfo(setup.JwtAuth.Role, setup.JwtAuth.Token);
        }
        else if (setup.AuthMethod == VaultAuthenticationMethod.Ldap)
        {
            authMethod = new LDAPAuthMethodInfo(setup.LdapAuth.Username, setup.LdapAuth.Password);
        }
        else if (setup.AuthMethod == VaultAuthenticationMethod.Oidc)
        {
            authMethod = new OCIAuthMethodInfo(setup.OidcAuth.Role, setup.OidcAuth.Headers);
        }
        else if (setup.AuthMethod == VaultAuthenticationMethod.UsernamePassword)
        {
            authMethod = new UserPassAuthMethodInfo(setup.UsernamePasswordAuth.Username, setup.UsernamePasswordAuth.Password);
        }
        else if (setup.AuthMethod == VaultAuthenticationMethod.Token)
        {
            authMethod = new TokenAuthMethodInfo(setup.TokenAuth.Token);
        }
        else if (setup.AuthMethod == VaultAuthenticationMethod.Cert)
        {
            authMethod = new CertAuthMethodInfo(setup.CertAuth.Certificate, setup.CertAuth.RoleName);

        }

        if (authMethod == null)
        {
            throw new SdkException("Vault authentication method is not set or not supported.");
        }

        var host = setup.Host;

        var settings = new VaultClientSettings($"{host}", authMethod);

        this.Setup = setup;

        return new VaultClient(settings) as TConnection;
    }
}
