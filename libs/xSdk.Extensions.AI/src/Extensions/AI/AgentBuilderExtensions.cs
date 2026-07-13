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

namespace xSdk.Extensions.AI;

public static class AgentBuilderExtensions
{

    extension(AgentBuilder builder)
    {
        public AgentBuilder UseClient(string name)
        {
            builder.ClientName = name;
            return builder;
        }

        public AgentBuilder WithName(string name)
        {
            builder.Name = name;
            return builder;
        }

        public AgentBuilder WithDescription(string description)
        {
            builder.Description = description;
            return builder;
        }

        public AgentBuilder WithModel(string model)
        {
            builder.Model = model;
            return builder;
        }

        public AgentBuilder WithFile(string filePath)
        {
            builder.FilePath = filePath;
            return builder;
        }

        public AgentBuilder WithPrompt(string instructions)
        {
            builder.Instructions = instructions;
            return builder;
        }

        public AgentBuilder UseTool(string name)
        {
            builder.Tools.Add(name);
            return builder;
        }
    }
}



//// ── Prompt ──
//public AgentBuilder WithPrompt(string prompt)
//{
//    // Store the prompt for later use
//    return this;
//}

//// ── Model Client ──
//public AgentBuilder WithModel<TClient>()
//    where TClient : IChatClient
//{
//    return this;
//}

//// ── Tools ──
//public ToolBuilder WithTool(string name)
//{
//    return new ToolBuilder(this, name);
//}

//// ── Transport: In-Process ──
//public AgentBuilder WithInProcess()
//{
//    return this;
//}

//// ── Transport: Verteilt ──
//public AgentBuilder Distribute(string url)
//{
//    return this;
//}

//// ── Endpoints ──
//public AgentBuilder ExposeOpenAiEndpoint(int port)
//{

//    return this;
//}

//public AgentBuilder WithHealthChecks()
//{
//    return this;
//}
