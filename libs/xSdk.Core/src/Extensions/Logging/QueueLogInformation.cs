using Microsoft.Extensions.Logging;

namespace xSdk.Extensions.Logging;

internal class QueueLogInformation(string? categoryName, LogLevel logLevel, EventId eventId, Exception? exception, string? message)
{
    public LogLevel LogLevel => logLevel;

    public EventId EventId => eventId;

    public Exception? Exception => exception;

    public string? CategoryName => categoryName;

    public string? Message => message;
}

internal sealed class QueueLogInformation<TState>(
    string? categoryName,
    LogLevel logLevel,
    EventId eventId,
    TState state,
    Exception? exception,
    Func<TState, Exception?, string> formatter,
    string? message) : QueueLogInformation(categoryName, logLevel, eventId, exception, message)
{
    public TState State => state;
    
    public Func<TState, Exception?, string> Formatter => formatter;
}
