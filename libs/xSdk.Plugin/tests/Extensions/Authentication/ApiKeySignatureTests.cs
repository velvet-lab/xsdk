using xSdk.Extensions.Authentication;

namespace xSdk.Plugin.Tests.Extensions.Authentication;

public class ApiKeySignatureTests
{
    [Fact]
    public void Name_HasExpectedValue()
    {
        Assert.Equal("xSDK ApiKey", ApiKeySignature.Name);
    }

    [Fact]
    public void Identifier_HasExpectedValue()
    {
        Assert.Equal("xsdk-api-key", ApiKeySignature.Identifier);
    }

    [Fact]
    public void Name_IsNotEmpty()
    {
        Assert.NotEmpty(ApiKeySignature.Name);
    }

    [Fact]
    public void Identifier_IsNotEmpty()
    {
        Assert.NotEmpty(ApiKeySignature.Identifier);
    }
}
