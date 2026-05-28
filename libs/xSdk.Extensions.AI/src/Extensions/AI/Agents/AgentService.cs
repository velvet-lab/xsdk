using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xSdk.Hosting;

namespace xSdk.Extensions.AI.Agents;

internal class AgentService(IChatClient chatClient, IOptions<AgentsPluginOptions> pluginOptions, ILogger<AgentService> logger) : IAgentService
{
    private const string DefaultAgentYaml = @"
kind: Prompt
name: Assistant
description: Helpful assistant
instructions: You are a helpful assistant. You answer questions in the language specified by the user. You return your answers in a JSON format.
model:
    options:
        temperature: 0.9
        topP: 0.95
";
//outputSchema:
//    properties:
//        language:
//            type: string
//            required: true
//            description: The language of the answer.
//        answer:
//            type: string
//            required: true
//            description: The answer text.
//";

    public async Task<AIAgent> CreateAgentAsync(CancellationToken token = default)
    {
        logger.LogInformation("Creating agent from YAML definition.");

        var agentFactory = new ChatClientPromptAgentFactory(chatClient, loggerFactory: LogManager.Factory);
        var agent = await agentFactory.CreateFromYamlAsync(DefaultAgentYaml, token);

        return agent;
    }
    
}
