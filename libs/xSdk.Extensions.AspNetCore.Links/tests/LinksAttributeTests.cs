using xSdk.Extensions.Links;

namespace xSdk.Extensions.AspNetCore.Links.Tests;

public class LinksAttributeTests
{
    [Fact]
    public void LinksAttribute_WithPolicyName_SetsPolicyName()
    {
        var attribute = new LinksAttribute("MyPolicy");

        Assert.Equal("MyPolicy", attribute.PolicyName);
    }

    [Fact]
    public void LinksAttribute_WithNullPolicyName_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new LinksAttribute(null!));
    }

    [Fact]
    public void LinksAttribute_IsAttributeOnMethod()
    {
        var usage = typeof(LinksAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), inherit: false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        Assert.NotNull(usage);
        Assert.True(usage.ValidOn.HasFlag(AttributeTargets.Method));
    }
}
