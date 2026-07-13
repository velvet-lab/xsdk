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
using Microsoft.Extensions.AI;
using OpenAI;
using xSdk.Extensions.Builder;
using xSdk.Plugins.AI;
using xSdk.Tools;

namespace xSdk.Extensions.AI;

public sealed class ClientBuilder<TBuilder>(TBuilder builder) : ClientBuilder(builder)
    where TBuilder : AIBuilder
{
}

public class ClientBuilder(AIBuilder builder) : BuilderBase
{
    private readonly Dictionary<string, IChatClient> _chatClients = new();

    internal Action<ClientBuilder> ConfigureBuilderAction { get; set; }

    internal string? Name { get; set; }

    internal string? ApiKey { get; set; }

    internal string? Endpoint { get; set; }

    internal Action<OpenAIClientOptions>? OpenAiClientOptionsFactory { get; set; }

    internal IChatClient? Build(string? model)
    {
        ConfigureBuilderAction?.Invoke(this);

        if (IsValid<ClientBuilder, ClientBuilderValidator>())
        {
            var key = $"{Name}:{model}".ToLowerInvariant();
            if (_chatClients.TryGetValue(key, out IChatClient? existingClient))
            {
                return existingClient;
            }

            if (OpenAiClientOptionsFactory is not null)
            {
                var chatClientBuilder = BuildOpenAIChatClient(model);
                var chatClient = ConfigureChatClient(chatClientBuilder);
                _chatClients.AddOrNew(key, chatClient);

                return chatClient;
            }
        }
        return default;
    }

    private ChatClientBuilder BuildOpenAIChatClient(string? model)
    {
        var options = new OpenAIClientOptions
        {
            Endpoint = new Uri(Endpoint)
        };

        OpenAiClientOptionsFactory?.Invoke(options);

        var client = new OpenAIClient(new ApiKeyCredential(ApiKey!), options);

        return client
            .GetChatClient(model)
            .AsIChatClient()
            .AsBuilder();
    }

    private IChatClient ConfigureChatClient(ChatClientBuilder chatClientBuilder)
    {
        if (builder.Options.IsTelemetryEnabled)
        {
            chatClientBuilder
                .UseOpenTelemetry(sourceName: builder.DiagnosticsSourceName, configure: cfg => cfg.EnableSensitiveData = builder.Options.IsSensitiveDataEnabled);

            if (builder.EnableLogging)
            {
                chatClientBuilder
                    .UseLogging(builder.LoggerFactory);
            }
        }

        return chatClientBuilder
            .Build();
    }
}

