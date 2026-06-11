using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Demos.AI;
using xSdk.Demos.AI.Executors;
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

            .AddWorkflowFactory<EnglishTranslatorExecutor>(NameRegistry.TranslationWorkflow, NameRegistry.TranslationWorkflowDescription, WorkflowFactory)
            .AddExecutor<FrenchTranslatorExecutor>(NameRegistry.FrenchTranslator)
            .AddExecutor<ItalianTranslatorExecutor>(NameRegistry.ItalianTranslator)

            // Simple Agent
            .AddAgentFile("AI\\Agents\\EnglishTranslator.yaml")
            .AddAgentFile("AI\\Agents\\FrenchTranslator.yaml")
            .AddAgentFile("AI\\Agents\\ItalianTranslator.yaml");
    }

    private static Workflow WorkflowFactory(WorkflowBuilder builder, Executor startExecutor, IServiceProvider provider)
    {
        FrenchTranslatorExecutor? frenchExecutor = provider.GetKeyedService<FrenchTranslatorExecutor>(NameRegistry.FrenchTranslator);
        ItalianTranslatorExecutor? italianExecutor = provider.GetKeyedService<ItalianTranslatorExecutor>(NameRegistry.ItalianTranslator);

        Workflow workflow = builder
            .AddEdge(startExecutor, frenchExecutor)
            .AddEdge(startExecutor, italianExecutor)   
            .Build();

        return workflow;
    }
}
