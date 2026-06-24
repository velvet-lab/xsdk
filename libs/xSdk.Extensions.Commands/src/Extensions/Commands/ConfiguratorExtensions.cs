using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console.Cli;

namespace xSdk.Extensions.Commands;

public static class ConfiguratorExtensions
{
    extension(IConfigurator configurator)
    {
        public IConfigurator AddDefaultCommands()
        {
            configurator.AddCommand<ExitCommand>(ExitCommand.Definitions.Name);
            configurator.AddCommand<ClearCommand>(ClearCommand.Definitions.Name);
            configurator.AddCommand<HelpCommand>(HelpCommand.Definitions.Name);
            return configurator;
        }
    }
}
