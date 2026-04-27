using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace xSdk.Hosting;

public class WebHostTestFixture : TestHostFixture
{
    private readonly List<Action<WebHostBuilderContext, IServiceCollection>> _webhostServicesDelegates = new();

    public WebHostTestFixture() { }

    public WebHostTestFixture ConfigureWebHostServices(Action<WebHostBuilderContext, IServiceCollection> configure)
    {
        _webhostServicesDelegates.Add(configure);
        return this;
    }

    protected override IHostBuilder CreateHostBuilder()
        => TestWebHost.CreateBuilder();

    protected override void Initialize()
    {
        ConfigureBuilder(builder =>
        {
            builder
                .ConfigureWebHost(webhostBuilder =>
                {
                    webhostBuilder
                        .ConfigureServices((context, services) =>
                        {
                            foreach (var configure in _webhostServicesDelegates)
                            {
                                configure?.Invoke(context, services);
                            }
                        });
                });
        });
    }
}
