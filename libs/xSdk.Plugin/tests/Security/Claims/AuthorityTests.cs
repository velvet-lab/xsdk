using xSdk.Security.Claims;

namespace xSdk.Plugin.Tests.Security.Claims;

public class AuthorityTests
{
    [Fact]
    public void Name_HasExpectedValue()
    {
        Assert.Equal("xSDK Authority", Authority.Name);
    }

    [Fact]
    public void Name_IsNotEmpty()
    {
        Assert.NotEmpty(Authority.Name);
    }
}

public class SdkClaimTypesTests
{
    [Fact]
    public void ApiKeyName_ReturnsNonEmptyString()
    {
        var result = SdkClaimTypes.ApiKey.Name;

        Assert.NotEmpty(result);
    }

    [Fact]
    public void ApiKeyIdentifier_ReturnsNonEmptyString()
    {
        var result = SdkClaimTypes.ApiKey.Identifier;

        Assert.NotEmpty(result);
    }

    [Fact]
    public void ApiKeyName_ContainsApikey()
    {
        var result = SdkClaimTypes.ApiKey.Name;

        Assert.Contains("apikey", result);
    }

    [Fact]
    public void ApiKeyIdentifier_ContainsApikey()
    {
        var result = SdkClaimTypes.ApiKey.Identifier;

        Assert.Contains("apikey", result);
    }

    [Fact]
    public void ApiKeyName_ContainsName()
    {
        var result = SdkClaimTypes.ApiKey.Name;

        Assert.Contains("name", result);
    }

    [Fact]
    public void ApiKeyIdentifier_ContainsIdentifier()
    {
        var result = SdkClaimTypes.ApiKey.Identifier;

        Assert.Contains("identifier", result);
    }

    [Fact]
    public void ApiKeyName_StartsWithHttps()
    {
        var result = SdkClaimTypes.ApiKey.Name;

        Assert.StartsWith("https://", result);
    }
}
