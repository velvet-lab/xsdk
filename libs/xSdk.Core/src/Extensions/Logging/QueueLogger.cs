using Microsoft.Extensions.Logging;

namespace xSdk.Extensions.Logging;

internal sealed class QueueLogger() : ILogger
{
    private static readonly Queue<QueueLogInformation> _queue = new();
    private static readonly Lock _lock = new();
    private string _categoryName = typeof(QueueLogger).FullName ?? nameof(QueueLogger);

    public IDisposable BeginScope<TState>(TState state) => default!;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        // Queue the message
        lock (_lock)
        {
            string message = formatter(state, exception);
            var logInformation = new QueueLogInformation<TState>(_categoryName, logLevel, eventId, state, exception, formatter, message);
            _queue.Enqueue(logInformation);
        }
    }

    internal void SetCategoryName(string categoryName)
    {
        // This method can be used to set the category name for the logger if needed
        _categoryName = categoryName;
    }

    /// <summary>
    /// Flushes all queued messages to the real _logger.
    /// </summary>
    internal static void FlushQueuedMessages(ILoggerFactory factory)
    {
        lock (_lock)
        {
            while (_queue.Count > 0)
            {
                QueueLogInformation logInformation = _queue.Dequeue();
                try
                {
                    string category = nameof(QueueLogger);
                    if(!string.IsNullOrEmpty(logInformation.CategoryName))
                    {
                        category = logInformation.CategoryName;                        
                    }

                    ILogger logger = factory.CreateLogger(category);
                    if (logInformation is QueueLogInformation<object> logInfo)
                    {
                        logger.Log(logInformation.LogLevel, logInformation.EventId, logInfo.State, logInformation.Exception, logInfo.Formatter);
                    }
                    else
                    {
                        logger.Log(logInformation.LogLevel, logInformation.EventId, logInformation.Exception, logInformation.Message);
                    }                    
                }
                catch
                {
                    // Ignore errors during flush to prevent breaking the application
                }
            }
        }
    }
}
