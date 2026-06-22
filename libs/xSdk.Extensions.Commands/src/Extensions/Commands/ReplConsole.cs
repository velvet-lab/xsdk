using Spectre.Console.Cli;
using xSdk.Tools;

namespace xSdk.Extensions.Commands;

public class ReplConsole(ICommandApp app, IConsolePluginBuilder builder) : IConsole
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
            Environment.ExitCode = await app.RunAsync(replArgs);
            if (isCleared)
            {
                builder.CreateBanner();
                isCleared = false;
            }

            string input = builder.CreateUserPrompt();
            if (parser.Reparse(input).ContainsPattern(ExitCommand.Definitions.Name))
            {
                shouldRun = false;
            }
            else if(parser.Reparse(input).ContainsPattern(ClearCommand.Definitions.Name))
            {
                isCleared = true;
            }

            replArgs = CommandlineParser.Parse(input).Arguments;
            
        } while (shouldRun);

        builder.CreateLastWill();

        return Environment.ExitCode;
    }
}
