//using Microsoft.Agents.AI;
//using Microsoft.Agents.AI.Workflows;
//using Microsoft.Extensions.AI;
//using Microsoft.Extensions.DependencyInjection;
//using OpenAI;
//using xSdk.Extensions.Options;
//using xSdk.Plugins.AI;

//namespace xSdk.Extensions.AI;

//public interface IAILayerBuilder
//{
//    //IList<AIDefinition> Definitions { get; }

//    //void Build(IServiceCollection services, PluginOptions? pluginOptions, EnvironmentOptions? environmentOptions);

//    //IAILayerBuilder AddAgentFile(string filePath);

//    //IAILayerBuilder AddAgentFactory(Func<IServiceProvider, string, AIDefinition, AIAgent?> factory);

//    //// IAILayerBuilder AddChatClientFactory(Func<TClient, string, IChatClient> factory);

//    //IAILayerBuilder AddTool(string name, AIFunction aIFunction);

//    //IAILayerBuilder AddWorkflowFactory<TStartExecutor>(string name, Func<WorkflowBuilder, Executor, IServiceProvider, Workflow> factory)
//    //    where TStartExecutor : Executor
//    //    => AddWorkflowFactory<TStartExecutor>(name, default, factory);

//    //IAILayerBuilder AddWorkflowFactory<TStartExecutor>(string name, string? description, Func<WorkflowBuilder, Executor, IServiceProvider, Workflow> factory)
//    //    where TStartExecutor : Executor;

//    //IAILayerBuilder AddExecutor<TExecutor>()
//    //    where TExecutor : Executor;

//    //IAILayerBuilder AddExecutor<TExecutor>(string id)
//    //    where TExecutor : Executor;


//    IAILayerBuilder AddClientFactory(string? name, Func<IChatClient> factory);
//}
