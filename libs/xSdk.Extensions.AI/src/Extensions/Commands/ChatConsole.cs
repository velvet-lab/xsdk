using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using xSdk.Extensions.Options;

namespace xSdk.Extensions.Commands;

internal class ChatConsole(ICommandApp app, IOptions<ChatConsolePluginOptions> commandOptions, IOptions<ApplicationOptions> appOptions) : IConsole
{
    public async Task<int> RunAsync(string[] args)
    {
        //bool shouldRun = true;
        //bool isCleared = false;

        //var parser = ChatCommandlineParser.Create(args);
        //string[] chatArgs = parser.Arguments;

        //WriteBanner(commandOptions.Value);

        //do
        //{
        //    Environment.ExitCode = await app.RunAsync(chatArgs);
        //    if (isCleared)
        //    {
        //        WriteBanner(commandOptions.Value);
        //        isCleared = false;
        //    }

        //    string input = await AnsiConsole.AskAsync<string>(commandOptions.Value.UserPrompt ?? "Enter your message or command");        
        //    if (parser.ContainsChatCommand(input))
        //    {
        //        chatArgs = [];

        //        string? command = parser.ExtractChatCommand(input);
        //        if(!string.IsNullOrEmpty(command))
        //        {
        //            if (string.Equals(command, ExitCommand.Definitions.Name, StringComparison.InvariantCultureIgnoreCase))
        //            {
        //                shouldRun = false;
        //            }
        //            else if (string.Equals(command, ClearCommand.Definitions.Name, StringComparison.InvariantCultureIgnoreCase))
        //            {                        
        //                chatArgs = [command];
        //                isCleared = true;
        //            }
        //            else if (string.Equals(command, "help", StringComparison.InvariantCultureIgnoreCase))
        //            {
        //                chatArgs = [ "--help" ];
        //            }
        //            else
        //            {
        //                chatArgs = [command];
        //            }
        //        }
        //    }
        //    else
        //    {
        //        chatArgs = ["--message", input];
        //    }

        //} while (shouldRun);

        //WriteLastWill(commandOptions.Value);

        return Environment.ExitCode;
    }

    //private static void WriteBanner(ChatConsolePluginOptions? options)
    //{
    //    if (options is not null && options.Banner is not null)
    //    {
    //        options.Banner.Invoke();
    //        return;
    //    }
    //}

    //private static void WriteLastWill(ChatConsolePluginOptions? options)
    //{
    //    if(options is not null && options.LastWill is not null)
    //    {
    //        options.LastWill.Invoke();
    //        return;
    //    }
    //}
}
