using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace xSdk.Extensions.Commands;

internal class CommandAppBuilder<TDefaultCommand> : ICommandAppBuilder
    where TDefaultCommand : class, ICommand
{
    public ICommandApp Build(IServiceCollection services)
    {
        return new CommandApp<TDefaultCommand>(new ServiceRegistrar(services));
    }
}
