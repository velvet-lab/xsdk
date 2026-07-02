using System.ComponentModel;
using xSdk.Extensions.Commands.Attributes;

namespace xSdk.Extensions.Commands;

internal class ChatCommand(IChatMessageHandler handler) : CommandHandler
{
    internal static class Definitions
    {
        public const string Name = "chat";
        public const string HelpText = "Start a chat session";
    }

    [CommandArgument("userInput")]
    [Description("The user input to send to the chat client")]
    public string[]? UserInput { get; set; }

    public override async Task<int> ExecuteAsync(CancellationToken cancellationToken)
    {
        if(UserInput is not null && UserInput.Length > 0)
        {
            var message = string.Join(" ", UserInput);
            return await handler.HandleMessageAsync(message, cancellationToken);
        }
    
        return 0;
    }
}

