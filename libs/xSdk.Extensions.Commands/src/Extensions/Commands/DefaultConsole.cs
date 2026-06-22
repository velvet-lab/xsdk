using Spectre.Console.Cli;

namespace xSdk.Extensions.Commands;

internal class DefaultConsole(ICommandApp app) : IConsole
{
    public async Task<int> RunAsync(string[] args)
    {
        var parser = SpecificCommandlineParser.Create(args);
        Environment.ExitCode = await app.RunAsync(parser.Arguments);
        return Environment.ExitCode;
    }
}
