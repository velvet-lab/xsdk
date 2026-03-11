using xSdk.Data;
using xSdk.Extensions.Links;

namespace xSdk.Extensions.AspNetCore.Links.Tests;

public class MultiPolicyTests
{
    private class TestModel : Model
    {
        public TestModel()
        {
            PrimaryKey = new GuidStringPK();
        }

        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void AddPolicy_ChainedCalls_AddsMultiplePolicies()
    {
        var options = new LinksOptions();

        options
            .AddPolicy<TestModel>(policy => policy.RequireRoutedLink("self", "GetById"))
            .AddPolicy<TestModel>(policy => policy.RequireRoutedLink("list", "GetAll"));

        Assert.Equal(2, options.Policies.Count);
    }

    [Fact]
    public void AddPolicy_ConfigureActionIsCalled_LinkCountMatches()
    {
        var options = new LinksOptions();

        options.AddPolicy<TestModel>(policy =>
        {
            policy.RequireRoutedLink("get", "GetById");
            policy.RequireRoutedLink("list", "GetAll");
        });

        Assert.Single(options.Policies);
        Assert.Equal(2, options.Policies[0].Links.Count);
    }
}
