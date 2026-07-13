/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
