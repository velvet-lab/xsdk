using System;
using System.Collections.Generic;
using System.Text;
using xSdk.Tools;

namespace xSdk.Extensions.Commands;

internal class ChatCommandlineParser : CommandlineParser
{
    private readonly char[] _chatCommandPatterns = new[] { '/', '!', '#', '@' };

    protected ChatCommandlineParser(string? input) : base(input)
    {
        
    }

    internal static ChatCommandlineParser Create(string[] args)
        => new(string.Join(" ", args));

    internal bool ContainsChatCommand(string? input)
    {
        var args = ParseInternal(input);
        foreach (var arg in args)
        {
            if (!string.IsNullOrEmpty(arg) && _chatCommandPatterns.Any(x => arg.StartsWith(x)))
            {
                return true;
            }
        }
        return false;
    }

    internal (string?, string?) ExtractChatCommand(string? input)
    {
        string? command = null;

        var args = ParseInternal(input);
        foreach (var arg in args)
        {
            if (!string.IsNullOrEmpty(arg) && _chatCommandPatterns.Any(x => arg.StartsWith(x)))
            {
                command = arg.TrimStart(_chatCommandPatterns);
                break;
            }
        }

        string remainingArgs = ExtractRemainingArgs(input, command);
        return (command, remainingArgs);
    }

    private string ExtractRemainingArgs(string? input, string? command)
    {
        if (string.IsNullOrEmpty(command))
        {
            return input ?? string.Empty;
        }

        var args = ParseInternal(input);
        var remainingArgs = args.SkipWhile(x => !string.Equals(x.TrimStart(_chatCommandPatterns), command, StringComparison.InvariantCultureIgnoreCase)).Skip(1);
        return string.Join(" ", remainingArgs);
    }
}
