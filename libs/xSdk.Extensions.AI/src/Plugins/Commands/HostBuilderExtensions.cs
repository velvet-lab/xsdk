using Microsoft.Extensions.Hosting;
using xSdk.Hosting;
using xSdk.Extensions.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Plugins.Commands;

public static class HostBuilderExtensions
{
    extension(IHostBuilder builder)
    {        
        public IHostBuilder EnableChatConsole<TBuilder, TChatMessageHandler>()
            where TBuilder : class, IReplConsolePluginBuilder
            where TChatMessageHandler : class, IChatMessageHandler
            => builder
                .RegisterPluginServices(services => services.AddSingleton<IApplicationBuilder, ApplicationBuilder<ChatApplication>>())
                .RegisterPluginBuilder<IReplConsolePluginBuilder, TBuilder>()
                .RegisterServices(services =>
                    services
                        .AddSingleton<IChatMessageHandler, TChatMessageHandler>()
                )
                .EnableConsole<PluginOptions>(options =>
                {
                    options.DisableDefaultHelp = true;
                });
    }
}
