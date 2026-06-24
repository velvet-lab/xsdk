using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;

namespace xSdk.Extensions.Commands;

public interface IReplConsolePluginBuilder : IConsolePluginBuilder
{
    void CreateBanner();

    void CreateHelp(ICommandAppSettings settings, ICommandModel model);

    void CreateLastWill();

    string CreateUserPrompt();
}
