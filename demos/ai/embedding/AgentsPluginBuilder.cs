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

using Microsoft.Agents.AI.Workflows;
using xSdk.Demos.AI.Executors;
using xSdk.Extensions.AI;
using xSdk.Plugins.AI;

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

            .AddWorkflowFactory<FileInspectorExecutor>("EmbeddingWorkflow", "A workflow to embedd files", WorkflowFactory)

            // Simple Agent
            .AddAgentFile("AI\\Agents\\FileInspector.yaml");
    }

    private static Workflow WorkflowFactory(WorkflowBuilder builder, Executor firstExecutor, IServiceProvider provider)
    {
        Workflow workflow = builder
            .WithOutputFrom(firstExecutor)
            .Build();

        return workflow;
    }
}
