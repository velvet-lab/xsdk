using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using xSdk.Demos.AI;
using xSdk.Extensions.Commands;

namespace xSdk.Demos;

[Description(Definitions.HelpText)]
[ExcludeFromCodeCoverage]
internal class ChatCommand([FromKeyedServices(NameRegistry.TranslationWorkflow)] Workflow workflow, IOptions<ChatConsolePluginOptions> options) : Command<ChatCommandSettings>
{
    internal static class Definitions
    {
        public const string Name = "chat";
        public const string HelpText = "Start a chat session";
    }

    protected override int Execute(CommandContext context, ChatCommandSettings settings, CancellationToken cancellationToken)
    {
        if(!string.IsNullOrEmpty(settings.Message))
        {
            return SendMessage(settings.Message, cancellationToken);
        }
        return 0;
    }

    private int SendMessage(string input, CancellationToken stoppingToken)
    {
        var returnCode = AnsiConsole
            .Status()
            .Start<int>("Agent is thinking", context =>
            {
                context.Spinner(Spinner.Known.Balloon);

                return Task.Run<int>(async () =>
                {
                    context.Status = "Agent is responding";
                    await using StreamingRun run = await InProcessExecution.RunStreamingAsync(workflow, input: input, cancellationToken: stoppingToken);

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

