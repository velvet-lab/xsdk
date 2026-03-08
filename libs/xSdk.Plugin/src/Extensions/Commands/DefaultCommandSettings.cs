using System.ComponentModel;
using Spectre.Console.Cli;

namespace xSdk.Extensions.Commands;

public class DefaultCommandSettings : CommandSettings
{
    [CommandOption(Definitions.LogLevel.Template)]
    [Description(Definitions.LogLevel.HelpText)]
    public string LogLevel { get; set; }

    [CommandOption(Definitions.Stage.Template)]
    [Description(Definitions.Stage.HelpText)]
    [DefaultValue(Definitions.Stage.DefaultValue)]
    public Stage Stage { get; set; }

    [CommandOption(Definitions.Demo.Template)]
    [Description(Definitions.Demo.HelpText)]
    public bool IsDemo { get; set; }

    [CommandOption(Definitions.ContentRoot.Template)]
    [Description(Definitions.ContentRoot.HelpText)]
    public string ContentRoot { get; set; }

    public static class Definitions
    {
        public static class LogLevel
        {
            public const string Name = "log-level";
            public const string Template = "--log-level <LEVEL>";
            public const string HelpText =
                "Set the log level for the application. Default primaryKey is 'Info'. Possible Values: Off, Trace, Debug, Info, Warn, Error or Fatal";
            public const string DefaultValue = "Info";
        }

        public static class Stage
        {
            public const string Name = "stage";
            public const string Template = "--stage <STAGE>";
            public const string HelpText = "Stage where application is running. Default primaryKey is 'Development'.";
            public const xSdk.Stage DefaultValue = xSdk.Stage.Development;
        }

        public static class Demo
        {
            public const string Name = "demo";
            public const string Template = "--demo";
            public const string HelpText = "Enables the demo mode for the application. This will generate fake data for demostration";
        }

        public static class ContentRoot
        {
            public const string Name = "content-root";
            public const string Template = "--content-root <ROOT>";
            public const string HelpText = "Content root folder where application should working. If not given, content root will automatically determined";
        }
    }
}
