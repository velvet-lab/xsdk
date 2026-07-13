using Microsoft.Extensions.AI;
using xSdk.Extensions.Builder;
using xSdk.Plugins.AI;

namespace xSdk.Extensions.AI;

public class ToolBuilder<TBuilder>(TBuilder parent) : ToolBuilder(parent)
    where TBuilder : AIBuilder
{ }

public class ToolBuilder(AIBuilder parent) : BuilderBase
{
    public Action<ToolBuilder>? ConfigureBuilderAction { get; internal set; }

    internal string? Name { get; set; }

    internal string? Description { get; set; }

    internal Delegate? InProcessDelegate { get; set; }

    internal AIFunctionFactoryOptions? InProcessOptions { get; set; }


    internal AIFunction? BuildInProcess()
    {
        ConfigureBuilderAction?.Invoke(this);

        if (InProcessDelegate is not null)
        {
            if (InProcessOptions is null)
            {
                InProcessOptions = new AIFunctionFactoryOptions();
            }

            InProcessOptions.Name = Name;
            InProcessOptions.Description = Description;

            return AIFunctionFactory.Create(InProcessDelegate, InProcessOptions);
        }
        return default;
    }
}
