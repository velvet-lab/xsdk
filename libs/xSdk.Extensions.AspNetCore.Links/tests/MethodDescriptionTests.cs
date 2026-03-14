using System.Reflection;
using xSdk.Extensions.Links;

namespace xSdk.Extensions.AspNetCore.Links.Tests;

public class MethodDescriptionTests
{
    [Fact]
    public void MethodDescription_ToString_ReturnsMethodName()
    {
        var desc = new MethodDescription { MethodName = "GetById" };

        Assert.Equal("GetById", desc.ToString());
    }

    [Fact]
    public void MethodDescription_ToString_WhenMethodNameIsNull_ReturnsTypeName()
    {
        var desc = new MethodDescription();

        var result = desc.ToString();

        Assert.Equal(typeof(MethodDescription).Name, result);
    }

    [Fact]
    public void MethodDescription_ShouldAuthorize_WhenAuthRolesNotEmpty_ReturnsTrue()
    {
        var desc = new MethodDescription { AuthRoles = ["Admin"] };

        Assert.True(desc.ShouldAuthorize);
    }

    [Fact]
    public void MethodDescription_ShouldAuthorize_WhenAuthPolicyNotEmpty_ReturnsTrue()
    {
        var desc = new MethodDescription { AuthPolicy = "RequireAuthenticated" };

        Assert.True(desc.ShouldAuthorize);
    }

    [Fact]
    public void MethodDescription_ShouldAuthorize_WhenNeitherSet_ReturnsFalse()
    {
        var desc = new MethodDescription
        {
            AuthRoles = [],
            AuthPolicy = null
        };

        Assert.False(desc.ShouldAuthorize);
    }

    [Fact]
    public void MethodDescription_DefaultAuthRoles_IsEmptyArray()
    {
        var desc = new MethodDescription();

        Assert.NotNull(desc.AuthRoles);
        Assert.Empty(desc.AuthRoles);
    }
}
