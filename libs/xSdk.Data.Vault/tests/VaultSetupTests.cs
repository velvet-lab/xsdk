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

public class VaultSetupTests
{
    [Fact]
    public void VaultDatabaseSetup_DefaultAuthMethod_IsNone()
    {
        var setup = new VaultDatabaseSetup();

        Assert.Equal(AuthMethod.None, setup.AuthMethod);
    }

    [Fact]
    public void VaultDatabaseSetup_SetHost_StoresValue()
    {
        var setup = new VaultDatabaseSetup();

        setup.Host = "http://localhost:8200";

        Assert.Equal("http://localhost:8200", setup.Host);
    }

    [Fact]
    public void VaultDatabaseSetup_SetBasePath_StoresValue()
    {
        var setup = new VaultDatabaseSetup();

        setup.BasePath = "secret/myapp";

        Assert.Equal("secret/myapp", setup.BasePath);
    }

    [Fact]
    public void VaultDatabaseSetup_SetAppRoleAuth_StoresValue()
    {
        var setup = new VaultDatabaseSetup();
        var auth = new AppRoleAuth { RoleId = "role-123", Secret = "secret-456" };

        setup.AppRoleAuth = auth;

        Assert.Same(auth, setup.AppRoleAuth);
    }

    [Fact]
    public void VaultDatabaseSetup_SetTokenAuth_StoresValue()
    {
        var setup = new VaultDatabaseSetup();
        var auth = new TokenAuth { Token = "test-token" };

        setup.TokenAuth = auth;

        Assert.Same(auth, setup.TokenAuth);
    }

    [Fact]
    public void AppRoleAuth_SetProperties_StoresValues()
    {
        var auth = new AppRoleAuth();

        auth.RoleId = "my-role-id";
        auth.Secret = "my-secret";

        Assert.Equal("my-role-id", auth.RoleId);
        Assert.Equal("my-secret", auth.Secret);
    }

    [Fact]
    public void TokenAuth_SetToken_StoresValue()
    {
        var auth = new TokenAuth();

        auth.Token = "s.test-token-123";

        Assert.Equal("s.test-token-123", auth.Token);
    }

    [Fact]
    public void VaultDatabaseSetup_SetUsernamePasswordAuth_StoresValue()
    {
        var setup = new VaultDatabaseSetup();
        var auth = new UsernamePasswordAuth { Username = "admin", Password = "pass" };

        setup.UsernamePasswordAuth = auth;

        Assert.Same(auth, setup.UsernamePasswordAuth);
    }

    [Fact]
    public void VaultDatabaseSetup_SetJwtAuth_StoresValue()
    {
        var setup = new VaultDatabaseSetup();
        var auth = new JwtAuth { Role = "my-role", Token = "jwt-token" };

        setup.JwtAuth = auth;

        Assert.Same(auth, setup.JwtAuth);
    }

    [Fact]
    public void VaultDatabaseSetup_SetLdapAuth_StoresValue()
    {
        var setup = new VaultDatabaseSetup();
        var auth = new LdapAuth { Username = "user", Password = "pass" };

        setup.LdapAuth = auth;

        Assert.Same(auth, setup.LdapAuth);
    }

    [Fact]
    public void VaultDatabaseSetup_SetCertAuth_StoresValue()
    {
        var setup = new VaultDatabaseSetup();
        var auth = new CertAuth { RoleName = "my-cert-role" };

        setup.CertAuth = auth;

        Assert.Same(auth, setup.CertAuth);
    }

    [Fact]
    public void VaultDatabaseSetup_SetOidcAuth_StoresValue()
    {
        var setup = new VaultDatabaseSetup();
        var auth = new OidcAuth { Role = "my-oidc-role" };

        setup.OidcAuth = auth;

        Assert.Same(auth, setup.OidcAuth);
    }

    [Fact]
    public void OidcAuth_DefaultHeaders_IsEmpty()
    {
        var auth = new OidcAuth();

        Assert.NotNull(auth.Headers);
        Assert.Empty(auth.Headers);
    }

    [Fact]
    public void OidcAuth_AddHeader_StoresHeader()
    {
        var auth = new OidcAuth();

        auth.AddHeader("X-Custom", "value");

        Assert.Contains("X-Custom", auth.Headers.Keys);
        Assert.Equal("value", auth.Headers["X-Custom"]);
    }

    [Fact]
    public void OidcAuth_AddHeader_ReturnsSelf()
    {
        var auth = new OidcAuth();

        var result = auth.AddHeader("key", "val");

        Assert.Same(auth, result);
    }

    [Fact]
    public void UsernamePasswordAuth_SetProperties_StoresValues()
    {
        var auth = new UsernamePasswordAuth { Username = "admin", Password = "secret" };

        Assert.Equal("admin", auth.Username);
        Assert.Equal("secret", auth.Password);
    }

    [Fact]
    public void JwtAuth_SetProperties_StoresValues()
    {
        var auth = new JwtAuth { Role = "role-1", Token = "tok.123" };

        Assert.Equal("role-1", auth.Role);
        Assert.Equal("tok.123", auth.Token);
    }

    [Fact]
    public void LdapAuth_SetProperties_StoresValues()
    {
        var auth = new LdapAuth { Username = "uid=test,dc=example,dc=com", Password = "pass" };

        Assert.Equal("uid=test,dc=example,dc=com", auth.Username);
        Assert.Equal("pass", auth.Password);
    }

    [Fact]
    public void CertAuth_SetRoleName_StoresValue()
    {
        var auth = new CertAuth { RoleName = "test-cert-role" };

        Assert.Equal("test-cert-role", auth.RoleName);
    }

    [Fact]
    public void VaultAuthenticationMethod_Values_AreCorrect()
    {
        Assert.Equal(0, (int)AuthMethod.None);
        Assert.Equal(1, (int)AuthMethod.AppRole);
        Assert.Equal(6, (int)AuthMethod.Token);
        Assert.Equal(7, (int)AuthMethod.Cert);
    }
}
