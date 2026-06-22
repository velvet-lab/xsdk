using System.ComponentModel;
using Spectre.Console.Cli;

namespace xSdk.Demos.Commands;

public class ChatCommandSettings : CommandSettings
{
    [CommandArgument(0, "[ARGS...]")]
    public string[]? Args { get; set; }
}
