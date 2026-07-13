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

//using CommunityToolkit.Diagnostics;
//using Microsoft.Agents.AI;
//using Microsoft.Agents.AI.Hosting;
//using Microsoft.Agents.AI.Workflows;
//using Microsoft.Agents.ObjectModel;
//using Microsoft.Extensions.AI;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using xSdk.Extensions.Options;
//using xSdk.Hosting;
//using xSdk.Plugins.AI;
//using xSdk.Tools;

//namespace xSdk.Extensions.AI;

//internal partial class AILayerBuilder
//{
//    //private readonly Dictionary<string, IChatClient> _chatClients = [];

//    //public void Build(IServiceCollection services, PluginOptions? pluginOptions, EnvironmentOptions? environmentOptions)
//    //{
//    //    BuildExecutors(services);
//    //    BuildTools(services);

//    //    InvokeAgentBuilder(services, pluginOptions, environmentOptions);
//    //    BuildWorkflows(services);
//    //}

//    //private void InvokeAgentBuilder(IServiceCollection services, PluginOptions? pluginOptions, EnvironmentOptions? environmentOptions)
//    //{
//    //    Logger.LogInformation("Initializing AI Layer for client type: {ClientType}", typeof(TClient).Name);

//    //    TClient client = clientFactory.Invoke();
//    //    foreach (AIDefinition definition in _agentDefinitions)
//    //    {
//    //        if (definition.TryReadMetadata(pluginOptions, environmentOptions, out GptComponentMetadata? metadata))
//    //        {
//    //            definition.Metadata = metadata;
//    //        }

//    //        Logger.LogInformation("Loaded definition: {AgentName}", definition.Name);

//    //        string? modelName = definition.Model ?? pluginOptions?.Model;
//    //        IChatClient chatClient = RetrieveChatClients(client, modelName);
//    //        services.AddKeyedChatClient(definition.Name, chatClient);

//    //        if (_agentFactory is not null)
//    //        {
//    //            IHostedAgentBuilder agentBuilder = services.AddAIAgent(definition.Name, (provider, key) =>
//    //            {
//    //                AIAgent? realAgent = _agentFactory.Invoke(provider, key, definition);
//    //                return realAgent ?? throw new InvalidOperationException("Failed to create definition instance for definition: " + definition.Name);
//    //            });
//    //            services.AddKeyedSingleton(definition.Name, agentBuilder);
//    //        }
//    //    }
//    //}

//    //private void BuildWorkflows(IServiceCollection services)
//    //{
//    //    Logger.LogInformation("Registering {WorkflowCount} workflows in the AI Layer", _workflowDefinitions.Count);
//    //    foreach (AIWorkflowDefinition workflowDefinition in _workflowDefinitions)
//    //    {
//    //        services.AddWorkflow(workflowDefinition.Name, (provider, key) =>
//    //        {
//    //            (WorkflowBuilder? builder, Executor? firstExecutor) = workflowDefinition.CreateBuilder(provider);

//    //            Workflow workflow = workflowDefinition.Factory(builder, firstExecutor, provider);

//    //            return workflow;
//    //        });
//    //    }
//    //}

//    //private void BuildTools(IServiceCollection services)
//    //{

//    //    Logger.LogInformation("Registering {ToolCount} tools in the AI Layer", _tools.Count);
//    //    foreach (KeyValuePair<string, AIFunction> tool in _tools)
//    //    {
//    //        services.AddKeyedSingleton(tool.Key, tool.Value);
//    //        Logger.LogDebug("Registered tool: {ToolName}", tool.Key);
//    //    }
//    //}

//    //private void BuildExecutors(IServiceCollection services)
//    //{
//    //    Logger.LogInformation("Registering {ExecutorCount} executors in the AI Layer", _executorDefinitions.Count);
//    //    foreach (KeyValuePair<string, Type> kvp in _executorDefinitions)
//    //    {
//    //        Logger.LogInformation("Registered executor: {ExecutorType}", kvp.Value);
//    //        services.AddKeyedSingleton(kvp.Value, kvp.Key, (provider, key) =>
//    //        {
//    //            var executor = ActivatorUtilities.CreateInstance(provider, kvp.Value) as Executor;
//    //            return executor ?? throw new InvalidOperationException("Failed to create executor instance for type: " + kvp.Value.Name);
//    //        });
//    //    }
//    //}

//    //private IChatClient RetrieveChatClients(TClient client, string? model)
//    //{
//    //    Guard.IsNotNull(model);

//    //    if (!_chatClients.ContainsKey(model))
//    //    {
//    //        // Build chat client here
//    //        IChatClient chatClient = _chatClientFactory?.Invoke(client, model) ?? throw new InvalidOperationException("Failed to create chat client.");
//    //        _chatClients.AddOrNew(model, chatClient);
//    //    }

//    //    return _chatClients[model];
//    //}
//}
