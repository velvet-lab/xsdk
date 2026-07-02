using System.Reflection;

namespace xSdk.Extensions.Commands;

public abstract class CommandHandler : ICommandHandler
{
    internal bool IsAsyncOverridden => HasMethodBeenOverridden(GetType().GetMethod(nameof(ICommandHandler.ExecuteAsync)));

    protected internal CommandContext? Context { get; internal set; }

    public virtual int Execute() => 0;

    public virtual Task<int> ExecuteAsync(CancellationToken cancellationToken)
        => Task.FromResult(0);

    /// <summary>
    /// Internal method to check if a method has been overridden by a derived class
    /// </summary>
    /// <param name="method">The method to check</param>
    /// <returns>True if the method has been overridden</returns>
    private static bool HasMethodBeenOverridden(MethodInfo? method)
    {
        if (method == null)
        {
            return false;
        }

        // Check if this is the actual implementation in CommandHandler
        // If a child class overrides this method, it won't be the same implementation
        if (method.DeclaringType == typeof(CommandHandler))
        {
            // This is the base implementation - we don't consider this as overridden
            return false;
        }

        // If we got here, it means this method has been overridden in a derived class
        return true;
    }


}
