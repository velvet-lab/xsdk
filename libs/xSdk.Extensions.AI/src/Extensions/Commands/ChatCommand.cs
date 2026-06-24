using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;
using xSdk.Extensions.Commands;

namespace xSdk.Demos.Commands;

[Description(Definitions.HelpText)]
[ExcludeFromCodeCoverage]
internal class ChatCommand(IChatMessageHandler handler) : AsyncCommand<ChatCommandSettings>
{
    internal static class Definitions
    {
        public const string Name = "chat";
        public const string HelpText = "Start a chat session";
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, ChatCommandSettings settings, CancellationToken cancellationToken)
    {
        if(settings.Args is not null && settings.Args.Length > 0)
        {
            var message = string.Join(" ", settings.Args);
            return await handler.HandleMessageAsync(message, cancellationToken);
        }
    
        return 0;
    }
}

