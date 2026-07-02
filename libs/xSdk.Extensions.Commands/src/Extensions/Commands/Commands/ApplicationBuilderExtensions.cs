using xSdk.Extensions.Commands.Commands;

namespace xSdk.Extensions.Commands;

public static class ApplicationBuilderExtensions
{
    extension(IApplicationBuilder builder)
    {
        public IApplicationBuilder AddDefaultCommands()
        {
            builder
                .AddCommand<ExitCommand>(ExitCommand.Definitions.Name, ExitCommand.Definitions.HelpText)
                .AddCommand<ClearCommand>(ClearCommand.Definitions.Name, ClearCommand.Definitions.HelpText)
                .AddCommand<HelpCommand>(HelpCommand.Definitions.Name, HelpCommand.Definitions.HelpText);

            return builder;
        }
    }
}
