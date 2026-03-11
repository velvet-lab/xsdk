using xSdk.Extensions.Links;

namespace xSdk.Extensions.AspNetCore.Links.Tests;

public class LinksOptionsTests
{
    [Fact]
    public void LinksOptions_DefaultConstructor_HasEmptyPoliciesList()
    {
        var options = new LinksOptions();

        Assert.NotNull(options.Policies);
        Assert.Empty(options.Policies);
    }
}
