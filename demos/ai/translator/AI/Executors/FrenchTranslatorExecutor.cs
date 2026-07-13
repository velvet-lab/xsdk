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
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Demos.AI.Executors;

internal sealed partial class FrenchTranslatorExecutor([FromKeyedServices(NameRegistry.FrenchTranslator)] AIAgent agent) : Executor<string, string>(NameRegistry.FrenchTranslator)
{
    [MessageHandler]
    public override async ValueTask<string> HandleAsync(string message, IWorkflowContext context, CancellationToken cancellationToken = default)
    {
        AgentResponse<string> response = await agent.RunAsync<string>(message, cancellationToken: cancellationToken);

        return response.Result;
    }
}
