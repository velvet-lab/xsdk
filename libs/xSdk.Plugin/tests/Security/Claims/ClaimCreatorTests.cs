using System.Security.Claims;
using xSdk.Security.Claims;

namespace xSdk.Plugin.Tests.Security.Claims;

public class ClaimCreatorTests
{
    [Fact]
    public void CreateClaimType_WithClaimOnly_ReturnsFallbackUrl()
    {
        var result = ClaimCreator.CreateClaimType("myrole");

        Assert.Contains("/claims/myrole", result);
        Assert.StartsWith("https://", result);
    }

    [Fact]
    public void CreateClaimType_WithContextAndClaim_IncludesContext()
    {
        var result = ClaimCreator.CreateClaimType("apikey", "name");

        Assert.Contains("apikey", result);
        Assert.Contains("name", result);
    }

    [Fact]
    public void CreateClaimType_WithUrlContextClaim_BuildsFullUrl()
    {
        var result = ClaimCreator.CreateClaimType("https://example.com", "user", "role");

        Assert.StartsWith("https://example.com", result);
        Assert.Contains("user", result);
        Assert.Contains("role", result);
    }

    [Fact]
    public void CreateClaimType_WithUrlWithoutHttps_PrefixesWithHttps()
    {
        var result = ClaimCreator.CreateClaimType("example.com", "user", "role");

        Assert.StartsWith("https://", result);
    }

    [Fact]
    public void CreateClaim_WithClaimTypeAndValue_CreatesClaim()
    {
        var claim = ClaimCreator.CreateClaim("test-type", "test-value");

        Assert.NotNull(claim);
        Assert.Equal("test-type", claim.Type);
        Assert.Equal("test-value", claim.Value);
    }

    [Fact]
    public void CreateClaim_WithValueType_SetValueType()
    {
        var claim = ClaimCreator.CreateClaim("test-type", "test-value", ClaimValueTypes.String);

        Assert.Equal(ClaimValueTypes.String, claim.ValueType);
    }

    [Fact]
    public void CreateClaim_WithIssuer_SetsIssuer()
    {
        var claim = ClaimCreator.CreateClaim("test-type", "test-value", ClaimValueTypes.String, "my-issuer");

        Assert.Equal("my-issuer", claim.Issuer);
    }

    [Fact]
    public void CreateClaim_WithAllParameters_SetsAll()
    {
        var claim = ClaimCreator.CreateClaim("test-type", "test-value", ClaimValueTypes.String, "issuer", "origissuer");

        Assert.Equal("test-type", claim.Type);
        Assert.Equal("test-value", claim.Value);
        Assert.Equal(ClaimValueTypes.String, claim.ValueType);
        Assert.Equal("issuer", claim.Issuer);
        Assert.Equal("origissuer", claim.OriginalIssuer);
    }

    [Fact]
    public void CreateClaim_WithNullValue_ThrowsSdkException()
    {
        Assert.Throws<SdkException>(() => ClaimCreator.CreateClaim("test-type", null));
    }

    [Fact]
    public void CreateClaim_WithEmptyValue_ThrowsSdkException()
    {
        Assert.Throws<SdkException>(() => ClaimCreator.CreateClaim("test-type", string.Empty));
    }
}
