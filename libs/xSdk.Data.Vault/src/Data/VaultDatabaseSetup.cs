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

using xSdk.Extensions.Variable;

namespace xSdk.Data;

public sealed class VaultDatabaseSetup : DatabaseSetup
{
    public VaultDatabaseSetup()
    {
        AuthMethod = VaultAuthenticationMethod.None;
    }

    internal VaultAuthenticationMethod AuthMethod { get; set; }

    public string Host { get; set; }

    public string? BasePath { get; set; }

    public Func<Stage, string, string> PathFormat { get; set; }

    public AppRoleAuth? AppRoleAuth { get; set; }

    public JwtAuth? JwtAuth { get; set; }

    public LdapAuth? LdapAuth { get; set; }

    public OidcAuth? OidcAuth { get; set; }

    public TokenAuth? TokenAuth { get; set; }

    public UsernamePasswordAuth? UsernamePasswordAuth { get; set; }

    public CertAuth? CertAuth { get; set; }

    protected override void ValidateSetup()
    {
        if (AuthMethod == VaultAuthenticationMethod.None && AppRoleAuth != null)
            AuthMethod = VaultAuthenticationMethod.AppRole;

        if (AuthMethod == VaultAuthenticationMethod.None && JwtAuth != null)
            AuthMethod = VaultAuthenticationMethod.Jwt;

        if (AuthMethod == VaultAuthenticationMethod.None && LdapAuth != null)
            AuthMethod = VaultAuthenticationMethod.Ldap;

        if (AuthMethod == VaultAuthenticationMethod.None && OidcAuth != null)
            AuthMethod = VaultAuthenticationMethod.Oidc;

        if (AuthMethod == VaultAuthenticationMethod.None && UsernamePasswordAuth != null)
            AuthMethod = VaultAuthenticationMethod.UsernamePassword;

        if (AuthMethod == VaultAuthenticationMethod.None && TokenAuth != null)
            AuthMethod = VaultAuthenticationMethod.Token;

        if (AuthMethod == VaultAuthenticationMethod.None && CertAuth != null)
            AuthMethod = VaultAuthenticationMethod.Cert;

        this.ValidateMember(x => x.AuthMethod == VaultAuthenticationMethod.None);
        this.ValidateMember(x => string.IsNullOrEmpty(x.Host));
        // this.ValidateMember(x => AuthMethod != VaultAuthenticationMethod.RoleId && string.IsNullOrEmpty(x.MountPoint));
    }
}
