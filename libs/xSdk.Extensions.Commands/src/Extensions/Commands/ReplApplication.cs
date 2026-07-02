using System.CommandLine;
using xSdk.Extensions.Commands.Commands;
using xSdk.Plugins.Commands;
using xSdk.Tools;

namespace xSdk.Extensions.Commands;

internal class ReplApplication(RootCommand command, IReplConsolePluginBuilder builder) : IApplication
{
    public async Task<int> RunAsync(string[] args)
    {
        bool shouldRun = true;
        bool isCleared = false;

        var parser = SpecificCommandlineParser.Create(args);
        string[] replArgs = parser.Arguments;        

        builder.CreateBanner();

        do
        {
            if (replArgs.Length > 0)
            {
                ParseResult parseResult = command.Parse(replArgs);

                Environment.ExitCode = await parseResult.InvokeAsync();
                if (isCleared)
                {
                    builder.CreateBanner();
                    isCleared = false;
                }
            }

            string input = builder.CreateUserPrompt();
            if (parser.Reparse(input).ContainsPattern(ExitCommand.Definitions.Name))
            {
                shouldRun = false;
            }
            else if (parser.Reparse(input).ContainsPattern(ClearCommand.Definitions.Name))
            {
                isCleared = true;
            }

            replArgs = CommandlineParser.Parse(input).Arguments;

        } while (shouldRun);

        builder.CreateLastWill();

        return Environment.ExitCode;
    }
}
