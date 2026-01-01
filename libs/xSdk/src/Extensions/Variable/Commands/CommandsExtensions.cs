using Spectre.Console.Cli;

namespace xSdk.Extensions.Variable.Commands
{
    public static class CommandsExtensions
    {
        public static IConfigurator AddVariableCommands(this IConfigurator configurator)
        {
            configurator.AddBranch(
                "variable",
                x =>
                {
                    x.SetDescription("Helps to show current configured application variables");
                    x.AddCommand<ListCommand>(ListCommand.Definitions.Name).WithExample("variable", "list", "--show-help");
                }
            );

            return configurator;
        }
    }
}
