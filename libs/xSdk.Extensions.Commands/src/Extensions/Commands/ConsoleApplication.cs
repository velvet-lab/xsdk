using System.CommandLine;
using xSdk.Tools;

namespace xSdk.Extensions.Commands;

internal class ConsoleApplication(RootCommand command) : IApplication
{
    public virtual async Task<int> RunAsync(string[] args)
    {
        var parser = CommandlineParser.Parse(args);

        ParseResult parseResults = command.Parse(parser.Arguments);
        if (parseResults == null)
        {
            return -1;
        }

        Environment.ExitCode = await parseResults.InvokeAsync();
        return Environment.ExitCode;
    }
}
