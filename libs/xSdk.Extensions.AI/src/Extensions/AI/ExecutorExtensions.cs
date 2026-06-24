using Microsoft.Agents.AI.Workflows;

namespace xSdk.Extensions.AI;

internal static class ExecutorExtensions
{
    extension<TExecutor> (TExecutor executor)
        where TExecutor : Executor
    {
        internal string RetrieveExecutorName()
            => RetrieveExecutorName(executor.GetType());
    }

    internal static string RetrieveExecutorName<TExecutor>()
        where TExecutor : Executor
        => RetrieveExecutorName(typeof(TExecutor));

    private static string RetrieveExecutorName(Type executorType)
    {
        string typeName = executorType.Name;
        return typeName;
    }
}
