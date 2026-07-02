using Microsoft.Extensions.Logging;

namespace xSdk.Extensions.Logging;

internal sealed class QueueLoggerFactory : ILoggerFactory
{
    private readonly QueueLogger _logger;

    public QueueLoggerFactory()
    {
        _logger = new QueueLogger();
    }

    public void AddProvider(ILoggerProvider provider)
    { }

    public ILogger CreateLogger(string categoryName)
    {
        _logger.SetCategoryName(categoryName);
        return _logger;
    }

    public void Dispose()
    {

    }
}
