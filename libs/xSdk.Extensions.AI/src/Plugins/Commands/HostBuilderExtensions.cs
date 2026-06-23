using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using xSdk.Demos.Commands;
using xSdk.Extensions.Commands;
using xSdk.Hosting;

namespace xSdk.Plugins.Commands;

public static class HostBuilderExtensions
{
    extension(IHostBuilder builder)
    {        
        public IHostBuilder EnableChatConsole<TConsoleBuilder, TChatMessageHandler>()
            where TConsoleBuilder : class, IConsolePluginBuilder
            where TChatMessageHandler : class, IChatMessageHandler
            => builder
                .EnableChatConsole<TConsoleBuilder, TChatMessageHandler>(_ => { });

        public IHostBuilder EnableChatConsole<TConsoleBuilder, TChatMessageHandler>(Action<ChatConsolePluginOptions> configure)
            where TConsoleBuilder : class, IConsolePluginBuilder
            where TChatMessageHandler : class, IChatMessageHandler
            => builder
                .RegisterServices(services =>
                    services
                        .AddSingleton<IConsole, ChatConsole>()
                        .AddSingleton<IChatMessageHandler, TChatMessageHandler>()
                )
                .EnableConsole<TConsoleBuilder, ChatConsolePluginOptions, ChatCommand>(configure);
    }
}
