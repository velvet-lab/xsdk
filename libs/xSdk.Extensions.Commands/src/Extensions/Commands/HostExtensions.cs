using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using Spectre.Console.Cli;

namespace xSdk.Extensions.Commands
{
    public static class HostExtensions
    {
        internal static Action<IReplBuilder> ReplBuilderDelegate { get; set; }

        public static int RunConsole(this IHost host, string[] args) => host.RunConsoleAsync(args).GetAwaiter().GetResult();

        public static async Task<int> RunConsoleAsync(this IHost host, string[] args)
        {
            if (host is null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            var app = host.Services.GetService<ICommandApp>();
            if (app == null)
            {
                throw new InvalidOperationException("Command application has not been configured.");
            }

            host.RemoveConsoleLoggers();

            var lastResult = 0;
            var replArgs = args;
            var defaultArgs = CommandlineParser.Parse(args).BackupDefaultArgs();
            var replBuilder = new ReplBuilder();
            var isReplConsole = CommandlineParser.Parse().ContainsPattern(ConsoleCommand.Definitions.Name);

            if (isReplConsole && ReplBuilderDelegate != null)
            {
                ReplBuilderDelegate(replBuilder);
            }

            do
            {
                var currentResult = await app.RunAsync(replArgs);
                if (isReplConsole)
                {
                    System.Console.Write(replBuilder.PromptFactory());
                    var input = System.Console.ReadLine();

                    if (CommandlineParser.Parse(input).AddDefaultArgs(defaultArgs).ContainsPattern(ExitCommand.Definitions.Name))
                    {
                        isReplConsole = false;
                    }
                    else
                    {
                        lastResult = currentResult;
                    }
                    replArgs = CommandlineParser.Parse(input).Arguments;
                }
                else
                {
                    lastResult = currentResult;
                }
            } while (isReplConsole);

            return lastResult;
        }

        private static IHost RemoveConsoleLoggers(this IHost host)
        {
            var config = NLog.LogManager.Configuration;

            var logNames = new List<string>();
            foreach (var target in config.AllTargets)
            {
                if (target is NLog.Targets.ConsoleTarget consoleTarget)
                {
                    logNames.Add(consoleTarget.Name);
                }
            }

            var wasRemoved = false;
            foreach (var logName in logNames)
            {
                if (!string.IsNullOrEmpty(logName))
                {
                    wasRemoved = true;
                    LogManager.Configuration.RemoveTarget(logName);
                }
            }

            if (wasRemoved)
            {
                LogManager.ReconfigExistingLoggers();
            }

            return host;
        }
    }
}
