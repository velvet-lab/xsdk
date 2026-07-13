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
using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Extensions.Commands;

internal class CommandActivator
{
    internal static CommandActivator Instance { get; private set; }

    private readonly IServiceProvider _serviceProvider;

    public CommandActivator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Instance = this;
    }

    internal async Task<int> ActivateCommandHandlerAsync(string key, ParseResult parseResult, CancellationToken token)
    {
        var handler = _serviceProvider.GetRequiredKeyedService<ICommandHandler>(key);
        var commandResult = -1;

        if (handler is CommandHandler concrecteHandler)
        {
            OptionManager.Inject(handler, parseResult);
            ArgumentManager.Inject(handler, parseResult);

            concrecteHandler.Context = new CommandContext { ParseResult = parseResult };
            if (concrecteHandler.IsAsyncOverridden)
            {
                commandResult = await handler.ExecuteAsync(token);
            }
            else
            {
                commandResult = handler.Execute();
            }
        }

        return commandResult;
    }
}
