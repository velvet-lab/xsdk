using System.CommandLine;

namespace xSdk.Extensions.Commands;

public sealed class CommandContext
{
    public ParseResult ParseResult { get; internal set; }
}
