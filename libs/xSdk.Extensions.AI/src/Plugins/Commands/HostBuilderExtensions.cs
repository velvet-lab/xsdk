using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using xSdk.Extensions.Commands;
using xSdk.Hosting;

namespace xSdk.Plugins.Commands;

public static class HostBuilderExtensions
{
    extension(IHostBuilder builder)
    {        
        public IHostBuilder EnableChatConsole<TConsoleBuilder, TDefaultCommand>()
            where TConsoleBuilder : class, IConsolePluginBuilder
            where TDefaultCommand : class, ICommand
            => builder
                .EnableChatConsole<TConsoleBuilder, TDefaultCommand>(_ => { });

        public IHostBuilder EnableChatConsole<TConsoleBuilder, TDefaultCommand>(Action<ChatConsolePluginOptions> configure)
            where TConsoleBuilder : class, IConsolePluginBuilder
            where TDefaultCommand : class, ICommand
            => builder
                .RegisterServices(services => services.AddSingleton<IConsole, ChatConsole>())
                .EnableConsole<TConsoleBuilder, ChatConsolePluginOptions, TDefaultCommand>(configure);
    }
}
