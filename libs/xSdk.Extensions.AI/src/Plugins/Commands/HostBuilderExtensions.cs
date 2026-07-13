using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Commands;
using xSdk.Hosting;

namespace xSdk.Plugins.Commands;

public static class HostBuilderExtensions
{
    extension(IHostBuilder builder)
    {
        public IHostBuilder EnableChatConsole<TChatMessageHandler>()
            where TChatMessageHandler : class, IChatMessageHandler
            => builder
                .RegisterServices(services =>
                    services
                        .AddSingleton<IChatMessageHandler, TChatMessageHandler>()
                )
                .EnableConsole<ChatConsoleBuilder>(options => options.DisableDefaultHelp = true);
    }
}
