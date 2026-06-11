using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using xSdk.Demos.AI.Executors;
using xSdk.Demos.AI.Tools;
using xSdk.Extensions.AI;

namespace xSdk.Demos;

internal class AgentsPluginBuilder() : AIPluginBuilder, IAIPluginBuilder
{
    public override void Initialize()
    {
        // Register a Layer for OpenAI Clients, so that it can be used by agents and tools
        CreateAILayer(OpenAIHelper.CreateClient)

            // Register a factory for creating Chat Clients with different models, so that agents can use it
            .AddChatClientFactory(OpenAIHelper.CreateChatClient)

            // Register a factory for creating Agents, so that they can be created from definitions
            .AddAgentFactory(OpenAIHelper.CreateAgent)

            .AddWorkflowFactory<FileInspectorExecutor>("EmbeddingWorkflow", "A workflow to embedd files", WorkflowFactory)

            // Simple Agent
            .AddAgentFile("AI\\Agents\\FileInspector.yaml");
    }

    private static Workflow WorkflowFactory(WorkflowBuilder builder, Executor firstExecutor, IServiceProvider provider)
    {
        Workflow workflow = builder
            .WithOutputFrom(firstExecutor)
            .Build();

        return workflow;
    }
}
