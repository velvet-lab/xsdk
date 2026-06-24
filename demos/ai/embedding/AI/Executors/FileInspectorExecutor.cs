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
