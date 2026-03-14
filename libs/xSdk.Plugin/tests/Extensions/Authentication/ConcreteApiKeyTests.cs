using System.Security.Claims;
using xSdk.Extensions.Authentication;

namespace xSdk.Plugin.Tests.Extensions.Authentication;

public class ConcreteApiKeyTests
{
    [Fact]
    public void DefaultConstructor_SetsDefaultValues()
    {
        var apiKey = new ConcreteApiKey();

        Assert.Equal(string.Empty, apiKey.Key);
        Assert.Equal(string.Empty, apiKey.OwnerName);
        Assert.NotNull(apiKey.Claims);
    }

    [Fact]
    public void Key_CanBeSet()
    {
        var apiKey = new ConcreteApiKey { Key = "my-key" };

        Assert.Equal("my-key", apiKey.Key);
    }

    [Fact]
    public void OwnerName_CanBeSet()
    {
        var apiKey = new ConcreteApiKey { OwnerName = "alice" };

        Assert.Equal("alice", apiKey.OwnerName);
    }

    [Fact]
    public void Claims_CanBeAssigned()
    {
        var claims = new List<Claim> { new Claim("type", "value") };
        var apiKey = new ConcreteApiKey { Claims = claims };

        Assert.Single(apiKey.Claims);
    }
}
