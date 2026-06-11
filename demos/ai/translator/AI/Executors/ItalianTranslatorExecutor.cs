using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Demos.AI.Executors;

internal sealed partial class ItalianTranslatorExecutor([FromKeyedServices(NameRegistry.ItalianTranslator)] AIAgent agent) : Executor<string, string>(nameof(NameRegistry.ItalianTranslator))
{
    [MessageHandler]
    public override async ValueTask<string> HandleAsync(string message, IWorkflowContext context, CancellationToken cancellationToken = default)
    {
        AgentResponse<string> response = await agent.RunAsync<string>(message, cancellationToken: cancellationToken);

        return response.Result;
    }
}
