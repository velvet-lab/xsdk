using System.CommandLine;
using xSdk.Extensions.Commands.Commands;
using xSdk.Plugins.Commands;
using xSdk.Tools;

namespace xSdk.Extensions.Commands;

public sealed class ChatApplication(RootCommand command, IReplConsolePluginBuilder builder) : IApplication
{
    public async Task<int> RunAsync(string[] args)
    {
        bool shouldRun = true;
        bool isCleared = false;

        var parser = ChatCommandlineParser.Create(args);
        string[] chatArgs = [];

        builder.CreateBanner();

        do
        {
            if (chatArgs.Length > 0)
            {
                ParseResult parseResult = command.Parse(chatArgs);

                Environment.ExitCode = await parseResult.InvokeAsync();
                if (isCleared)
                {
                    builder.CreateBanner();
                    isCleared = false;
                }
            }

            string input = builder.CreateUserPrompt();
            if (parser.ContainsChatCommand(input))
            {
                (string? command, string? remainingArgs) = parser.ExtractChatCommand(input);
                if (!string.IsNullOrEmpty(command))
                {
                    if (string.Equals(command, ExitCommand.Definitions.Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        shouldRun = false;
                    }
                    else if (string.Equals(command, ClearCommand.Definitions.Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        isCleared = true;
                    }
                    input = $"{command} {remainingArgs}".Trim();
                }
            }
            else
            {
                input = $"{ChatCommand.Definitions.Name} '{input}'".Trim();
            }

            chatArgs = CommandlineParser.Parse(input).Arguments;

        } while (shouldRun);

        builder.CreateLastWill();

        return Environment.ExitCode;
    }    
}
