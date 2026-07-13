//using CommunityToolkit.Diagnostics;
//using Microsoft.Agents.AI;
//using Microsoft.Agents.AI.Workflows;
//using Microsoft.Extensions.AI;

//namespace xSdk.Extensions.AI;

//internal partial class AILayerBuilder
//{
//    //private Func<TClient, string, IChatClient>? _chatClientFactory;
//    //private Func<IServiceProvider, string, AIDefinition, AIAgent?>? _agentFactory;
//    //private readonly Dictionary<string, AIFunction> _tools = [];
//    //private readonly Dictionary<string, Type> _executorDefinitions = [];

//    //private readonly List<AIDefinition> _agentDefinitions = [];
//    //private readonly IList<AIWorkflowDefinition> _workflowDefinitions = [];

//    //public IList<AIDefinition> Definitions => _agentDefinitions;

//    //public IAILayerBuilder AddChatClientFactory(Func<TClient, string, IChatClient> factory)
//    //{
//    //    _chatClientFactory = factory;
//    //    return this;
//    //}

//    //public IAILayerBuilder AddAgentFactory(Func<IServiceProvider, string, AIDefinition, AIAgent?> factory)
//    //{
//    //    _agentFactory = factory;
//    //    return this;
//    //}

//    //public IAILayerBuilder AddTool(string name, AIFunction aIFunction)
//    //{
//    //    _tools.Add(name, aIFunction);
//    //    return this;
//    //}

//    //public IAILayerBuilder AddWorkflowFactory<TStartExecutor>(string name, string? description, Func<WorkflowBuilder, Executor, IServiceProvider, Workflow> factory)
//    //        where TStartExecutor : Executor
//    //{
//    //    AddExecutor<TStartExecutor>();

//    //    _workflowDefinitions.Add(new AIWorkflowDefinition<TStartExecutor>(name, description, factory));
//    //    return this;
//    //}

//    //public IAILayerBuilder AddAgentFile(string filePath)
//    //{
//    //    Guard.IsNotNullOrEmpty(filePath);

//    //    _agentDefinitions.Add(new AIDefinition()
//    //    {
//    //        FilePath = filePath
//    //    });

//    //    return this;
//    //}

//    //public IAILayerBuilder AddExecutor<TExecutor>()
//    //    where TExecutor : Executor
//    //{
//    //    AddExecutor<TExecutor>(ExecutorExtensions.RetrieveExecutorName<TExecutor>());
//    //    return this;
//    //}

//    //public IAILayerBuilder AddExecutor<TExecutor>(string id)
//    //    where TExecutor : Executor
//    //{
//    //    _executorDefinitions.Add(id, typeof(TExecutor));
//    //    return this;
//    //}
//}
