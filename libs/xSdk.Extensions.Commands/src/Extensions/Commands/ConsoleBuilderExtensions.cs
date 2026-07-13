using System;
using System.Collections.Generic;
using System.Text;

namespace xSdk.Extensions.Commands;

public static class ConsoleBuilderExtensions
{
    extension(ConsoleBuilder builder)
    {
        public ConsoleBuilder AddDefaultCommands()
        {
            builder
                .AddCommand<ExitCommand>(ExitCommand.Definitions.Name, ExitCommand.Definitions.HelpText)
                .AddCommand<ClearCommand>(ClearCommand.Definitions.Name, ClearCommand.Definitions.HelpText)
                .AddCommand<HelpCommand>(HelpCommand.Definitions.Name, HelpCommand.Definitions.HelpText);

            return builder;
        }

        public ConsoleBuilder WithDescription(string description)
        {
            builder.Description = description;
            return builder;
        }

        public ConsoleBuilder AddCommand<THandler>(string name, string? description = default)
            where THandler : class, ICommandHandler
            => builder.AddCommand<THandler>(name, description);

        public CommandHandlerBuilder AddBranch(string name, string? description = default)
            => builder.AddBranch(name, description);
    }
}
