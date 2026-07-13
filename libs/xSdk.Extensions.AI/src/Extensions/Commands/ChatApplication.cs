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

using System.CommandLine;
using xSdk.Tools;

namespace xSdk.Extensions.Commands;

internal sealed class ChatApplication(RootCommand command, ChatConsoleBuilder builder) : IApplication
{
    public async Task<int> RunAsync(string[] args)
    {
        bool shouldRun = true;
        bool isCleared = false;

        var parser = ChatCommandlineParser.Create(args);
        string[] chatArgs = [];

        builder.CreateBannerAction?.Invoke();

        do
        {
            if (chatArgs.Length > 0)
            {
                ParseResult parseResult = command.Parse(chatArgs);

                Environment.ExitCode = await parseResult.InvokeAsync();
                if (isCleared)
                {
                    builder.CreateBannerAction?.Invoke();
                    isCleared = false;
                }
            }

            string input = builder.CreateUserPromptAction?.Invoke() ?? string.Empty;
            if (parser.ContainsChatCommand(input))
            {
                (string? command, string? remainingArgs) = parser.ExtractChatCommand(input);
                if (!string.IsNullOrEmpty(command))
                {
                    if (string.Equals(command, ExitCommand.Definitions.Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        shouldRun = false;
                    }
                    else if (string.Equals(command, ClearCommand.Definitions.Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        isCleared = true;
                    }
                    input = $"{command} {remainingArgs}".Trim();
                }
            }
            else
            {
                input = $"{ChatCommand.Definitions.Name} '{input}'".Trim();
            }

            chatArgs = CommandlineParser.Parse(input).Arguments;

        } while (shouldRun);

        builder.CreateLastWillAction?.Invoke();

        return Environment.ExitCode;
    }
}
