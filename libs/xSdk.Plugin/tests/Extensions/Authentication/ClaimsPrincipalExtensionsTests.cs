using System.Security.Claims;
using xSdk.Extensions.Authentication;
using xSdk.Security.Claims;

namespace xSdk.Plugin.Tests.Extensions.Authentication;

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
