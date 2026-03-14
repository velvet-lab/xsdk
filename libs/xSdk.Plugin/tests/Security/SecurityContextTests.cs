using xSdk.Security;

namespace xSdk.Plugin.Tests.Security;

public class SecurityContextTests
{
    [Fact]
    public void IsSuperUser_DoesNotThrow()
    {
        var ex = Record.Exception(() => SecurityContext.IsSuperUser());

        Assert.Null(ex);
    }

    [Fact]
    public void IsSuperUser_ReturnsBoolean()
    {
        var result = SecurityContext.IsSuperUser();

        Assert.IsType<bool>(result);
    }
}
