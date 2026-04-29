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

using System.Security.Claims;
using xSdk.Security.Claims;

namespace xSdk.Extensions.Authentication;

public class ClaimsPrincipalExtensionsTests
{
    [Fact]
    public void IsApiKeyPrincipal_WithValidApiKeyClaims_ReturnsTrue()
    {
        var claims = new List<Claim>
        {
            new Claim(SdkClaimTypes.ApiKey.Name, ApiKeySignature.Name, ClaimValueTypes.String, Authority.Name),
            new Claim(SdkClaimTypes.ApiKey.Identifier, ApiKeySignature.Identifier, ClaimValueTypes.String, Authority.Name),
        };
        var identity = new ClaimsIdentity(claims, "ApiKey");
        var principal = new ClaimsPrincipal(identity);

        var result = principal.IsApiKeyPrincipal();

        Assert.True(result);
    }

    [Fact]
    public void IsApiKeyPrincipal_WithNoClaims_ReturnsFalse()
    {
        var identity = new ClaimsIdentity("ApiKey");
        var principal = new ClaimsPrincipal(identity);

        var result = principal.IsApiKeyPrincipal();

        Assert.False(result);
    }

    [Fact]
    public void IsApiKeyPrincipal_WithOnlyNameClaim_ReturnsFalse()
    {
        var claims = new List<Claim>
        {
            new Claim(SdkClaimTypes.ApiKey.Name, ApiKeySignature.Name),
        };
        var identity = new ClaimsIdentity(claims, "ApiKey");
        var principal = new ClaimsPrincipal(identity);

        var result = principal.IsApiKeyPrincipal();

        Assert.False(result);
    }

    [Fact]
    public void IsApiKeyPrincipal_WithWrongValues_ReturnsFalse()
    {
        var claims = new List<Claim>
        {
            new Claim(SdkClaimTypes.ApiKey.Name, "wrong-name"),
            new Claim(SdkClaimTypes.ApiKey.Identifier, "wrong-identifier"),
        };
        var identity = new ClaimsIdentity(claims, "ApiKey");
        var principal = new ClaimsPrincipal(identity);

        var result = principal.IsApiKeyPrincipal();

        Assert.False(result);
    }

    [Fact]
    public void IsApiKeyPrincipal_WithNullPrincipal_Throws()
    {
        ClaimsPrincipal principal = null!;

        Assert.ThrowsAny<Exception>(() => principal.IsApiKeyPrincipal());
    }
}
