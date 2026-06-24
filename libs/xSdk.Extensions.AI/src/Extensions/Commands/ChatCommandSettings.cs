using System.ComponentModel;
using Spectre.Console.Cli;

namespace xSdk.Demos.Commands;

internal class ChatCommandSettings : CommandSettings
{
    [CommandArgument(0, "[ARGS...]")]
    public string[]? Args { get; set; }
}
