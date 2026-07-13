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

using System.ComponentModel;
using xSdk.Extensions.Commands.Attributes;

namespace xSdk.Extensions.Commands;

internal class ChatCommand(IChatMessageHandler handler) : CommandHandler
{
    internal static class Definitions
    {
        public const string Name = "chat";
        public const string HelpText = "Start a chat session";
    }

    [CommandArgument("userInput")]
    [Description("The user input to send to the chat client")]
    public string[]? UserInput { get; set; }

    public override async Task<int> ExecuteAsync(CancellationToken cancellationToken)
    {
        if (UserInput is not null && UserInput.Length > 0)
        {
            var message = string.Join(" ", UserInput);
            return await handler.HandleMessageAsync(message, cancellationToken);
        }

        return 0;
    }
}

