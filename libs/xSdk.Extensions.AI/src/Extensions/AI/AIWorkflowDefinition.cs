using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Extensions.AI;

internal abstract class AIWorkflowDefinition(string name, string? description, Func<WorkflowBuilder, Executor, IServiceProvider, Workflow> factory)
{
    public string Name { get; } = name;

    public string? Description { get; } = description;

    public Func<WorkflowBuilder, Executor, IServiceProvider, Workflow> Factory { get; } = factory;

    internal abstract (WorkflowBuilder, Executor) CreateBuilder(IServiceProvider provider);
}

internal class AIWorkflowDefinition<TStartExecutor>(string name, string? description, Func<WorkflowBuilder, Executor, IServiceProvider, Workflow> factory) : AIWorkflowDefinition(name, description, factory) where TStartExecutor : Executor
{
    internal override (WorkflowBuilder, Executor) CreateBuilder(IServiceProvider provider)
    {
        string executorName = ExecutorExtensions.RetrieveExecutorName<TStartExecutor>();

        TStartExecutor firstExecutor = provider.GetKeyedService<TStartExecutor>(executorName) ?? throw new InvalidOperationException($"Unable to resolve the starting executor of type {typeof(TStartExecutor).FullName} for workflow '{Name}'. Ensure it is registered in the service collection.");
        var builder = new WorkflowBuilder(firstExecutor);        

        if (!string.IsNullOrEmpty(Name))
        {
            builder.WithName(Name);
        }

        if (!string.IsNullOrEmpty(Description))
        {
            builder.WithDescription(Description);
        }

        return (builder, firstExecutor);
    }
}
