using System;
using System.Collections.Generic;
using System.Text;

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

    internal string? ExtractChatCommand(string? input)
    {
        var args = ParseInternal(input);
        foreach (var arg in args)
        {
            if (!string.IsNullOrEmpty(arg) && _chatCommandPatterns.Any(x => arg.StartsWith(x)))
            {
                return arg.TrimStart(_chatCommandPatterns);
            }
        }
        return default;
    }
}
