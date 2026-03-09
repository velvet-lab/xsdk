using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Logging;

namespace xSdk.Demos;

internal class LocalService
{
    private readonly ITelemetryService _telemetrySvc;
    private readonly ILogger<LocalService> _logger;
    private readonly Counter<int> _counter;

    public LocalService(ITelemetryService telemetrySvc, ILogger<LocalService> logger)
    {
        this._telemetrySvc = telemetrySvc ?? throw new ArgumentNullException(nameof(telemetrySvc));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Create a metrics counter
        this._counter = _telemetrySvc.CreateCounter<int>("MyCounter", description: "Count the calls for methods", unit: "times");
    }

    public void DoWorkA()
    {
        _logger.LogInformation("Now start a activity");
        using var activity = _telemetrySvc.StartActivity(ActivityKind.Client, "Do work A");

        _counter.Add(1);
        _logger.LogInformation("This is a Sample Log Entry");
        _logger.LogWarning("This is a Warning");
        _counter.Add(1);

        Task.Delay(500);

        activity?.SetStatus(ActivityStatusCode.Ok);
    }

    public void DoWorkB()
    {
        _logger.LogInformation("Now start a activity");
        using var activity = _telemetrySvc.StartActivity(kind: ActivityKind.Producer);

        try
        {
            Task.Delay(500);

            _counter.Add(1);

            _logger.LogInformation("Try to divide with zero");
            var a = 1;
            var b = 0;
            _counter.Add(1);
            var c = a / b;

            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);

        }
    }
}
