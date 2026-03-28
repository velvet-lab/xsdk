using Microsoft.Extensions.Logging;

namespace xSdk.Hosting;

/// <summary>
/// Provides static access to <see cref="ILogger"/> instances for SDK code that cannot
/// use constructor injection (static utility classes, pre-DI bootstrap contexts).
/// Uses a Console logger by default so bootstrap messages are always visible.
/// Replaced by the full DI-provided <see cref="ILoggerFactory"/> once the host is running.
/// </summary>
/// <remarks>
/// Replaced automatically via <see cref="LogManagerInitializer"/> which is
/// registered as an <see cref="IHostedService"/> during <c>HostLoggingManager.ConfigureLogging</c>.
/// </remarks>
public static class LogManager
{
    private static ILoggerFactory _factory = LoggerFactory.Create(b => b.AddConsole());

    internal static void Initialize(ILoggerFactory factory) =>
        _factory = factory ?? LoggerFactory.Create(b => b.AddConsole());

    public static ILogger CreateLogger(string categoryName) => _factory.CreateLogger(categoryName);

    public static ILogger<T> CreateLogger<T>() => _factory.CreateLogger<T>();

    public static ILogger CreateLogger(Type type) => _factory.CreateLogger(type);

    public static ILogger GetCurrentClassLogger()
    {
        var className = StackTraceUtils.GetClassFullName(new System.Diagnostics.StackFrame(1, false));
        return CreateLogger(className);
    }
}
