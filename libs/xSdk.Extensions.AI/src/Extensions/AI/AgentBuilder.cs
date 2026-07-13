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

using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Hosting;
using Microsoft.Agents.ObjectModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Builder;
using xSdk.Plugins.AI;

namespace xSdk.Extensions.AI;

public sealed class AgentBuilder(AIBuilder builder, YamlDeclarationLoader yamlLoader) : BuilderBase
{
    private readonly Dictionary<string, ToolBuilder> _toolBuilders = new();

    internal Action<AgentBuilder>? ConfigureBuilderAction { get; set; }

    internal string? Name { get; set; }

    internal string? Description { get; set; }

    internal string? Model { get; set; }

    internal string? Instructions { get; set; }

    internal string? ClientName { get; set; }

    internal string? FilePath { get; set; }


    internal List<string> Tools = new();

    internal void Build(IServiceCollection services)
    {
        ConfigureBuilderAction?.Invoke(this);

        if (IsValid<AgentBuilder, AgentBuilderValidator>())
        {
            var metaData = LoadMetaData();
            if (metaData is not null)
            {
                if (builder.ClientBuilders.TryGetValue(ClientName, out var clientBuilder))
                {
                    IChatClient? chatClient = clientBuilder.Build(Model);
                    if (chatClient is not null)
                    {
                        services.AddKeyedChatClient(ClientName, chatClient);

                        IHostedAgentBuilder agentBuilder = services.AddAIAgent(Name!, (provider, key) =>
                        {
                            var realAgent = CreateRawAgent(provider, metaData, ClientName, key);
                            return realAgent ?? throw new InvalidOperationException("Failed to create definition instance for definition: " + key);
                        });
                        services.AddKeyedSingleton(Name, agentBuilder);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Failed to build chat client for '{ClientName}' with model '{Model}'.");
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Client builder for '{ClientName}' not found.");
                }
            }
        }
    }

    private AIAgent? CreateRawAgent(IServiceProvider provider, GptComponentMetadata metaData, string clientName, string agentName)
    {
        IChatClient chatClient = provider.GetRequiredKeyedService<IChatClient>(clientName);

        IEnumerable<AIFunction> tools = LoadTools();

        var agentFactory = new ChatClientPromptAgentFactory(chatClient, functions: [.. tools]);
        AIAgentBuilder rawAgentBuilder = agentFactory
            .CreateAsync(metaData).GetAwaiter().GetResult()
            .AsBuilder();

        if (builder.Options.IsTelemetryEnabled)
        {
            rawAgentBuilder
                .UseOpenTelemetry(sourceName: builder.DiagnosticsSourceName, configure: cfg => cfg.EnableSensitiveData = builder.Options.IsSensitiveDataEnabled);

            if (builder.EnableLogging)
            {
                rawAgentBuilder
                    .UseLogging(builder.LoggerFactory);
            }
        }

        return rawAgentBuilder.Build();
    }

    private GptComponentMetadata? LoadMetaData()
    {
        GptComponentMetadata? metaData = default;
        if (!string.IsNullOrEmpty(Instructions) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Model))
        {
            metaData = yamlLoader.FromInstructions(Name, Instructions, Model);
        }

        if (!string.IsNullOrEmpty(FilePath))
        {
            metaData = yamlLoader.FromFile(FilePath);
        }

        if (metaData is not null)
        {
            Name = metaData.Name;
            if (metaData.Model.ExtensionData?.TryReadValue("name", out string? modelName) ?? false)
            {
                Model = modelName;
            }
        }

        return metaData;
    }

    //public IEnumerable<AIFunction> LoadTools(GptComponentMetadata metaData)
    //{
    //    // Nur Tools injizieren, die im YAML deklariert sind
    //    IEnumerable<string?>? toolNames = metaData?.Tools.Select(x => x.GetType().GetProperty("Name")?.GetValue(x) as string);
    //    if (toolNames is not null && toolNames.Any())
    //    {
    //        foreach (string? toolName in toolNames.Where(toolName => !string.IsNullOrEmpty(toolName)))
    //        {
    //            builder.BuildTool(toolName);
    //            //if (tool is not null)
    //            //{
    //            //    yield return tool;
    //            //}
    //        }
    //    }
    //    return default;
    //}

    internal IEnumerable<AIFunction> LoadTools()
    {
        foreach (var toolName in Tools)
        {
            if (builder.ToolBuilders.TryGetValue(toolName, out var toolBuilder))
            {
                AIFunction? aiFunction = toolBuilder.BuildInProcess();
                if (aiFunction is not null)
                {
                    yield return aiFunction;
                }
            }
            else
            {
                throw new InvalidOperationException($"Tool builder for '{toolName}' not found.");
            }
        }
    }
}
