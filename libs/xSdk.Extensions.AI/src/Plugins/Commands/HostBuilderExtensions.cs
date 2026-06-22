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
        public IHostBuilder EnableChatConsole<TCommand>()
            where TCommand : class, ICommand
           => builder.EnableChatConsole<TCommand>(_ => { });

        public IHostBuilder EnableChatConsole<TCommand>(Action<ChatConsolePluginOptions> configure)
            where TCommand : class, ICommand
            => builder
            .RegisterServices(services => services.AddSingleton<IConsole, ChatConsole>())
            .RegisterPluginHostOptions(configure)
            .EnableConsole<ChatCommandsPluginBuilder, TCommand>();
    }
}
