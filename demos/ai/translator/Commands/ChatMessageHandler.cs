using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using xSdk.Demos.AI;
using xSdk.Extensions.Commands;

namespace xSdk.Demos.Commands;

internal class ChatMessageHandler([FromKeyedServices(NameRegistry.TranslationWorkflow)] Workflow workflow) : IChatMessageHandler
{
    public async Task<int> HandleMessageAsync(string? message, CancellationToken token = default)
    {
        if (!string.IsNullOrEmpty(message))
        {
            return SendMessage(message, token);
        }
        return 0;
    }

    private int SendMessage(string message, CancellationToken stoppingToken)
    {
        var returnCode = AnsiConsole
            .Status()
            .Start<int>("Agent is thinking", context =>
            {
                context.Spinner(Spinner.Known.Balloon);

                return Task.Run<int>(async () =>
                {
                    context.Status = "Agent is responding";
                    await using StreamingRun run = await InProcessExecution.RunStreamingAsync(workflow, input: message, cancellationToken: stoppingToken);

                    await run.TrySendMessageAsync(new TurnToken(emitEvents: true));
                    await foreach (WorkflowEvent evt in run.WatchStreamAsync(stoppingToken))
                    {
                        context.Status = "Receiving response";
                        if (evt is ExecutorCompletedEvent executorCompleted)
                        {
                            WriteResponse(executorCompleted);
                        }
                        else if (evt is WorkflowErrorEvent workflowError)
                        {
                            return WriteError(workflowError);
                        }
                        else if (evt is ExecutorFailedEvent executorFailed)
                        {
                            return WriteError(executorFailed);
                        }
                    }
                    return 0;
                }, stoppingToken).GetAwaiter().GetResult();
            });

        AnsiConsole.WriteLine();

        return returnCode;
    }

    private static void WriteResponse(ExecutorCompletedEvent executorCompleted)
    {
        string? result = executorCompleted.Data as string;
        if (result is not null)
        {
            AnsiConsole.MarkupLine($"[green]{executorCompleted.ExecutorId}:[/] {result}");
        }
    }

    private static int WriteError(WorkflowErrorEvent workflowError)
    {
        var message = workflowError.Exception?.ToString() ?? "Unknown workflow error occurred.";
        return WriteError(message);
    }

    private static int WriteError(ExecutorFailedEvent executorFailed)
    {
        var message = $"Executor '{executorFailed.ExecutorId}' failed with {(executorFailed.Data == null ? "unknown error" : $"exception {executorFailed.Data}")}.";
        return WriteError(message);
    }

    private static int WriteError(string? message)
    {
        AnsiConsole.MarkupLine($"[red]Error:[/] {message}");
        return -1;
    }
}
