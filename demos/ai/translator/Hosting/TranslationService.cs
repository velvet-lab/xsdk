using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using xSdk.Demos.AI;

namespace xSdk.Demos.Hosting;

internal class TranslationService([FromKeyedServices(NameRegistry.TranslationWorkflow)] Workflow workflow) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using StreamingRun run = await InProcessExecution.RunStreamingAsync(workflow, input: "Hallo", cancellationToken: stoppingToken);

        await run.TrySendMessageAsync(new TurnToken(emitEvents: true));
        await foreach (WorkflowEvent evt in run.WatchStreamAsync(stoppingToken))
        {
            if (evt is ExecutorCompletedEvent executorCompleted)
            {
                string? result = executorCompleted.Data as string;
                if (result is not null)
                {
                    Console.WriteLine($"{executorCompleted.ExecutorId}: {result}");
                }
                else
                {
                    Console.WriteLine($"{executorCompleted.ExecutorId}: {executorCompleted.Data}");
                }
            }
            else if (evt is WorkflowErrorEvent workflowError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(workflowError.Exception?.ToString() ?? "Unknown workflow error occurred.");
                Console.ResetColor();
            }
            else if (evt is ExecutorFailedEvent executorFailed)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"Executor '{executorFailed.ExecutorId}' failed with {(executorFailed.Data == null ? "unknown error" : $"exception {executorFailed.Data}")}.");
                Console.ResetColor();
            }
        }
    }
}
