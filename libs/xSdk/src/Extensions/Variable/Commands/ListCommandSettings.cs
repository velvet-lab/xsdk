using System.ComponentModel;
using Spectre.Console.Cli;

namespace xSdk.Extensions.Variable.Commands;

internal class ListCommandSettings : CommandSettings
{
    [CommandOption("-f|--format <FORMAT>")]
    [Description("Formats the Output (default Name, Template, Protected, Prefix, Defined, Value)")]
    [DefaultValue("Name, Template, Protected, Prefix, Defined, Value")]
    public string FormatString { get; set; }

    [CommandOption("--show-help")]
    [Description("Should Help foreach Variable displayed?")]
    public bool ShowHelp { get; set; }
}
