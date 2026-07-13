using System.CommandLine;
using System.CommandLine.Help;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using xSdk.Extensions.Builder;

namespace xSdk.Extensions.Commands;

public class ConsoleBuilder : BuilderBase
{
    private readonly IList<CommandHandlerBuilder> _commandBuilders = [];

    public string? Description { get; internal set; } = string.Empty;

    protected internal RootCommand RootCommand
    {
        get => field ?? throw new InvalidOperationException("RootCommand has not been initialized and builded.");
        set;
    }

    internal ConsoleBuilder AddCommand<THandler>(string name, string? description = default)
            where THandler : class, ICommandHandler
    {
        var builder = new CommandHandlerBuilder(this)
        {
            Name = name,
            Description = description,
            HandlerType = typeof(THandler)
        };
        _commandBuilders.Add(builder);

        return this;
    }

    internal CommandHandlerBuilder AddBranch(string name, string? description = default)
    {
        var builder = new CommandHandlerBuilder(this)
        {
            Name = name,
            Description = description
        };
        _commandBuilders.Add(builder);
        return builder;
    }

    internal void Build(IServiceCollection? services)
    {
        Guard.IsNotNull(services);

        ConfigureBuilder();

        RootCommand = new RootCommand(Description ?? string.Empty);
        services.TryAddSingleton(RootCommand);

        foreach (CommandHandlerBuilder commandBuilder in _commandBuilders)
        {
            if (commandBuilder is CommandHandlerBuilder handlerBuilder)
            {
                handlerBuilder.Build(services);
            }
        }

        services.TryAddSingleton<IApplication>(provider =>
        {
            ActivatorUtilities.CreateInstance<CommandActivator>(provider);

            RootCommand rootCommand = provider.GetRequiredService<RootCommand>();

            IOptions<ConsoleOptions> options = provider.GetRequiredService<IOptions<ConsoleOptions>>();
            if (options.Value.DisableDefaultHelp)
            {
                foreach (Option option in rootCommand.Options)
                {
                    if (option is HelpOption helpOption)
                    {
                        rootCommand.Options.Remove(helpOption);
                        break;
                    }
                }
            }

            return BuildApplication(provider);
        });
    }

    protected virtual IApplication BuildApplication(IServiceProvider provider)
        => ActivatorUtilities.CreateInstance<ConsoleApplication>(provider);
}
