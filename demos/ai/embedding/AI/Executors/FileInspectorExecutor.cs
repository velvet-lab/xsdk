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
using xSdk.Demos.Data.Models;

namespace xSdk.Demos.AI.Executors;

internal sealed partial class FileInspectorExecutor([FromKeyedServices("FileInspector")] AIAgent agent) : Executor<string, InspectionResult>(nameof(FileInspectorExecutor))
{
    [MessageHandler]
    public override async ValueTask<InspectionResult> HandleAsync(string message, IWorkflowContext context, CancellationToken cancellationToken = default)
    {
        AgentSession session = await agent.CreateSessionAsync(cancellationToken);

        AgentResponse<InspectionResult> result = await agent.RunAsync<InspectionResult>(message, session: session, cancellationToken: cancellationToken);

        return result.Result;
    }
}
