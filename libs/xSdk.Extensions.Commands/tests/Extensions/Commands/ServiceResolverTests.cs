using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Commands;

namespace xSdk.Extensions.Commands.Tests.Extensions.Commands;

public class ServiceResolverTests
{
    [Fact]
    public void Resolve_WithRegisteredService_ReturnsInstance()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ITestService, TestService>();
        var provider = services.BuildServiceProvider();
        var resolver = new ServiceResolver(provider);

        var result = resolver.Resolve(typeof(ITestService));

        Assert.NotNull(result);
        Assert.IsType<TestService>(result);
    }

    [Fact]
    public void Resolve_WithNullType_ReturnsNull()
    {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        var resolver = new ServiceResolver(provider);

        var result = resolver.Resolve(null);

        Assert.Null(result);
    }

    [Fact]
    public void Resolve_WithUnregisteredService_ReturnsNull()
    {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        var resolver = new ServiceResolver(provider);

        var result = resolver.Resolve(typeof(ITestService));

        Assert.Null(result);
    }

    private interface ITestService { }

    private class TestService : ITestService { }
}
