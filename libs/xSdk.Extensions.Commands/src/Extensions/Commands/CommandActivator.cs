using System.CommandLine;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using xSdk.Extensions.Commands.Attributes;
using xSdk.Tools;

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
