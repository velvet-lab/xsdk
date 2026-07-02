using System.CommandLine;
using System.CommandLine.Help;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using xSdk.Plugins.Commands;

namespace xSdk.Extensions.Commands;

public class ApplicationBuilder : IApplicationBuilder
{
    private readonly IList<ICommandHandlerBuilder> _commandBuilders = new List<ICommandHandlerBuilder>();

    protected internal RootCommand RootCommand
    {
        get => field ?? throw new InvalidOperationException("RootCommand has not been initialized and builded.");
        set => field = value;
    }

    public string? Description { get; private set; } = string.Empty;

    public IApplicationBuilder SetDescription(string description)
    {
        Description = description;
        return this;
    }

    public IApplicationBuilder AddCommand<THandler>(string name, string? description = default)
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

    public ICommandHandlerBuilder AddBranch(string name, string? description = default)
    {
        var builder = new CommandHandlerBuilder(this)
        {
            Name = name,
            Description = description
        };
        _commandBuilders.Add(builder);
        return builder;
    }

    public virtual void Build(IServiceCollection services)
    {
        RootCommand = new RootCommand(Description ?? string.Empty);
        services.TryAddSingleton(RootCommand);

        foreach (var commandBuilder in _commandBuilders)
        {
            if (commandBuilder is CommandHandlerBuilder handlerBuilder)
            {
                handlerBuilder.Build(services);
            }
        }
    }
}

public sealed class ApplicationBuilder<TApplication> : ApplicationBuilder
    where TApplication : class, IApplication
{
    public override void Build(IServiceCollection services)
    {
        base.Build(services);

        services.TryAddSingleton<IApplication>(provider =>
        {
            ActivatorUtilities.CreateInstance<CommandActivator>(provider);

            var rootCommand = provider.GetRequiredService<RootCommand>();

            var options = provider.GetRequiredService<IOptions<PluginOptions>>();
            if (options.Value.DisableDefaultHelp)
            {
                foreach (var option in rootCommand.Options)
                {
                    if (option is HelpOption helpOption)
                    {
                        rootCommand.Options.Remove(helpOption);
                        break;
                    }
                }
            }

            return ActivatorUtilities.CreateInstance<TApplication>(provider);
        });

    }
}
