using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using xSdk.Demos.Data.Models;

namespace xSdk.Demos.Hosting;

internal class FileInspectorService([FromKeyedServices("EmbeddingWorkflow")] Workflow workflow) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //var agent = workflow.AsAIAgent();
        //var session = await agent.CreateSessionAsync(stoppingToken);

        await using StreamingRun run = await InProcessExecution.RunStreamingAsync(workflow, input: "Hallo", cancellationToken: stoppingToken);

        // var result = await agent.RunAsync<InspectionResult>($"File: {file.Name}", session: session, cancellationToken: stoppingToken);
        // Console.WriteLine($"Summary for {file.Name}:\n{result}\n");

        await run.TrySendMessageAsync(new TurnToken(emitEvents: true));
        await foreach (WorkflowEvent evt in run.WatchStreamAsync(stoppingToken))
        {
            if (evt is ExecutorCompletedEvent executorCompleted)
            {
                var result = executorCompleted.Data as InspectionResult;
                if (result is not null)
                {
                    Console.WriteLine($"{executorCompleted.ExecutorId}: IsReady ({result.IsReady}) ({result.Reason})");
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

    private static FileInfo[] LoadFiles()
    {
        return [.. Directory
            .GetFiles(Path.Combine(Environment.CurrentDirectory, "Embeddings"))
            .Select(x => new FileInfo(x))];
    }
}
