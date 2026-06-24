using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace xSdk.Extensions.Commands;

public interface ICommandAppBuilder
{
    ICommandApp Build(ITypeRegistrar registrar);
}
