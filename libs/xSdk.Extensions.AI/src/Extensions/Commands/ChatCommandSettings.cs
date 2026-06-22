using System.ComponentModel;
using Spectre.Console.Cli;

namespace xSdk.Extensions.Commands;

public class ChatCommandSettings : CommandSettings
{
    [CommandOption("-m|--message <MESSAGE>")]
    [Description("Message to send in the chat session")]    
    public string? Message { get; set; }
}
