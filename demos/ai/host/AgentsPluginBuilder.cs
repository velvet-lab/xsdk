using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI;
using xSdk.Extensions.AI;
using xSdk.Hosting;

namespace xSdk.Demos;

internal class AgentsPluginBuilder() : AIPluginBuilder, IAIPluginBuilder
{
    public override void Initialize()
    {   
        // Register a Layer for OpenAI Clients, so that it can be used by agents and tools
        CreateAILayer(OpenAIHelper.CreateClient)

            // Register a factory for creating Chat Clients with different models, so that agents can use it
            .RegisterChatClientFactory(OpenAIHelper.CreateChatClient)

            // Register a factory for creating Agents, so that they can be created from definitions
            .RegisterAgentFactory(OpenAIHelper.CreateAgent)

            // Simple Agent
            .AddAgentFile("AI\\Agents\\Assistant.yaml")

            // A Weather Agent with Tools
            .AddAgentFile("AI\\Agents\\GetWeather.yaml");
    }
}
