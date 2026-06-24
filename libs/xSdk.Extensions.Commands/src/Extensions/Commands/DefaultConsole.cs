using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using xSdk.Tools;

namespace xSdk.Extensions.Commands;

internal class DefaultConsole(IServiceProvider provider) : IConsole
{
    public async Task<int> RunAsync(string[] args)
    {
        var app = provider.GetRequiredService<ICommandApp>();
        var parser = CommandlineParser.Parse(args);

        var a = new CommandApp();        
        Environment.ExitCode = await app.RunAsync(parser.Arguments);
        return Environment.ExitCode;
    }
}
