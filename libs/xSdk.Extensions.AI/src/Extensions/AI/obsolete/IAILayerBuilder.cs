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

//using Microsoft.Agents.AI;
//using Microsoft.Agents.AI.Workflows;
//using Microsoft.Extensions.AI;
//using Microsoft.Extensions.DependencyInjection;
//using OpenAI;
//using xSdk.Extensions.Options;
//using xSdk.Plugins.AI;

//namespace xSdk.Extensions.AI;

//public interface IAILayerBuilder
//{
//    //IList<AIDefinition> Definitions { get; }

//    //void Build(IServiceCollection services, PluginOptions? pluginOptions, EnvironmentOptions? environmentOptions);

//    //IAILayerBuilder AddAgentFile(string filePath);

//    //IAILayerBuilder AddAgentFactory(Func<IServiceProvider, string, AIDefinition, AIAgent?> factory);

//    //// IAILayerBuilder AddChatClientFactory(Func<TClient, string, IChatClient> factory);

//    //IAILayerBuilder AddTool(string name, AIFunction aIFunction);

//    //IAILayerBuilder AddWorkflowFactory<TStartExecutor>(string name, Func<WorkflowBuilder, Executor, IServiceProvider, Workflow> factory)
//    //    where TStartExecutor : Executor
//    //    => AddWorkflowFactory<TStartExecutor>(name, default, factory);

//    //IAILayerBuilder AddWorkflowFactory<TStartExecutor>(string name, string? description, Func<WorkflowBuilder, Executor, IServiceProvider, Workflow> factory)
//    //    where TStartExecutor : Executor;

//    //IAILayerBuilder AddExecutor<TExecutor>()
//    //    where TExecutor : Executor;

//    //IAILayerBuilder AddExecutor<TExecutor>(string id)
//    //    where TExecutor : Executor;


//    IAILayerBuilder AddClientFactory(string? name, Func<IChatClient> factory);
//}
