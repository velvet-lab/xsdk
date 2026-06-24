using Spectre.Console.Cli;
using xSdk.Demos.Commands;
using xSdk.Tools;

namespace xSdk.Extensions.Commands;

internal class ChatConsole(ICommandApp app, IReplConsolePluginBuilder builder) : IConsole
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
            Environment.ExitCode = await app.RunAsync(chatArgs);
            if (isCleared)
            {
                builder.CreateBanner();
                isCleared = false;
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
