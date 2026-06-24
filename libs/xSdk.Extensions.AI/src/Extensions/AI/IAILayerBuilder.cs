using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Options;

namespace xSdk.Extensions.AI;

public interface IAILayerBuilder
{
    IList<AIDefinition> Definitions { get; }

    void Build(IServiceCollection services, AIPluginOptions? pluginOptions, EnvironmentOptions? environmentOptions);
}

public interface IAILayerBuilder<TClient> : IAILayerBuilder
    where TClient : class
{
    IAILayerBuilder<TClient> AddAgentFile(string filePath);

    IAILayerBuilder<TClient> AddAgentFactory(Func<IServiceProvider, string, AIDefinition, AIAgent?> factory);

    IAILayerBuilder<TClient> AddChatClientFactory(Func<TClient, string, IChatClient> factory);

    IAILayerBuilder<TClient> AddTool(string name, AIFunction aIFunction);

    IAILayerBuilder<TClient> AddWorkflowFactory<TStartExecutor>(string name, Func<WorkflowBuilder, Executor, IServiceProvider, Workflow> factory)
        where TStartExecutor : Executor
        => AddWorkflowFactory<TStartExecutor>(name, default, factory);

    IAILayerBuilder<TClient> AddWorkflowFactory<TStartExecutor>(string name, string? description, Func<WorkflowBuilder, Executor, IServiceProvider, Workflow> factory)
        where TStartExecutor : Executor;

    IAILayerBuilder<TClient> AddExecutor<TExecutor>()
        where TExecutor : Executor;

    IAILayerBuilder<TClient> AddExecutor<TExecutor>(string id)
        where TExecutor : Executor;
}
