/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.ClientModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using xSdk.Extensions.AI;

namespace xSdk.Demos;

public static class OpenAIHelper
{
    private const string Endpoint = "http://192.168.189.32:8000/v1";
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

    //public static IEmbeddingGenerator? CreateEmbeddingGenerator(AIOptions? options)
    //{
    //    if (options is not null)
    //    {
    //        OpenAIClient openaiClient = CreateClient();
    //        return openaiClient.GetEmbeddingClient(options.EmbeddingModel).AsIEmbeddingGenerator();
    //    }

    //    throw new InvalidOperationException("Invalid plugin options");
    //}

    //public static IChatClient CreateChatClient(OpenAIClient client, string model)
    //{
    //    return client.GetChatClient(model)
    //        .AsIChatClient()
    //        .AsBuilder()
    //        .EnableTelemetry(true)
    //        .Build();
    //}

    //public static AIAgent? CreateAgent(IServiceProvider provider, string name, AIDefinition definition)
    //{
    //    IChatClient chatClient = provider.GetRequiredKeyedService<IChatClient>(name);

    //    var agentFactory = new ChatClientPromptAgentFactory(chatClient, functions: [.. definition.LoadTools(provider)]);
    //    return agentFactory
    //        .CreateAsync(definition.Metadata).GetAwaiter().GetResult()
    //        .AsBuilder()
    //        // .EnableTelemetry(true)
    //        .Build();
    //}
}
