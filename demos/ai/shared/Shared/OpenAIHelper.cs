using System.ClientModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using xSdk.Demos.AI.Tools;
using xSdk.Extensions.AI;
using xSdk.Hosting;

namespace xSdk.Demos;

public static class OpenAIHelper
{
    private const string Endpoint = "http://192.168.189.32:11434/v1";
    private const string ApiKey = "sk-none";

    public static OpenAIClient CreateClient()
    {
        return new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
        {
            Endpoint = new Uri(Endpoint),
            EnableDistributedTracing = true,
            ClientLoggingOptions = new System.ClientModel.Primitives.ClientLoggingOptions
            {
                EnableLogging = true,
                EnableMessageContentLogging = true,
                EnableMessageLogging = true
            }
        });
    }

    public static IEmbeddingGenerator? CreateEmbeddingGenerator(AIPluginOptions? options)
    {
        if (options is not null)
        {
            OpenAIClient openaiClient = CreateClient();
            return openaiClient.GetEmbeddingClient(options.EmbeddingModel).AsIEmbeddingGenerator();
        }

        throw new InvalidOperationException("Invalid plugin options");
    }

    public static IChatClient CreateChatClient(OpenAIClient client, string model)
    {
        return client.GetChatClient(model)
            .AsIChatClient()
            .AsBuilder()
            .EnableTelemetry(true)
            .Build();
    }

    public static AIAgent? CreateAgent(IServiceProvider provider, string name, AIDefinition definition)
    {
        IChatClient chatClient = provider.GetRequiredKeyedService<IChatClient>(name);

        AIFunction[] tools = [AIFunctionFactory.Create(WeatherTool.GetWeather)];

        var agentFactory = new ChatClientPromptAgentFactory(chatClient, functions: tools, loggerFactory: LogManager.Factory);
        return agentFactory
            .CreateAsync(definition.Metadata).ConfigureAwait(false).GetAwaiter().GetResult()
            .AsBuilder()
            .EnableTelemetry(true)
            .Build();
    }    
}
