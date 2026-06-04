using System.ClientModel;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.AI;
using OpenAI;
using xSdk.Extensions.AI;

namespace xSdk.Demos.AI;

public static class OpenAIHelper
{
    public static IChatClient? CreateChatClient(AIPluginOptions? options)
    {
        if (options is not null)
        {
            OpenAIClient openaiClient = CreateAiClient(options);
            return openaiClient.GetChatClient(options.Model).AsIChatClient();
        }

        throw new InvalidOperationException("Invalid plugin options");
    }

    public static IEmbeddingGenerator? CreateEmbeddingGenerator(AIPluginOptions? options)
    {
        if (options is not null)
        {
            OpenAIClient openaiClient = CreateAiClient(options);
            return openaiClient.GetEmbeddingClient(options.EmbeddingModel).AsIEmbeddingGenerator();
        }

        throw new InvalidOperationException("Invalid plugin options");
    }

    private static OpenAIClient CreateAiClient(AIPluginOptions options)
    {
        Guard.IsNotNullOrEmpty(options.ApiKey);
        Guard.IsNotNullOrEmpty(options.Endpoint);

        return new OpenAIClient(new ApiKeyCredential(options.ApiKey), new OpenAIClientOptions
        {
            Endpoint = new Uri(options.Endpoint),
            EnableDistributedTracing = true,
            ClientLoggingOptions = new System.ClientModel.Primitives.ClientLoggingOptions
            {
                EnableLogging = true,
                EnableMessageContentLogging = true,
                EnableMessageLogging = true
            }
        });
    }
}
