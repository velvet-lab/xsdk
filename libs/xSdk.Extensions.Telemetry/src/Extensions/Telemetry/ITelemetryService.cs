using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace xSdk;

public interface ITelemetryService
{
    ActivitySource MainActivitySource { get; }

    Meter MainMeter { get; }

    Activity? StartActivity(ActivityKind kind = ActivityKind.Internal, [CallerMemberName] string name = "");

    Counter<T> CreateCounter<T>(string name, string? unit = null, string? description = null)
        where T : struct;

    Histogram<T> CreateHistogram<T>(string name, string? unit = null, string? description = null)
        where T : struct;

    UpDownCounter<T> CreateUpDownCounter<T>(string name, string? unit = null, string? description = null)
        where T : struct;

    ObservableUpDownCounter<T> CreateObservableUpDownCounter<T>(
        string name,
        Func<T> observeValue,
        string? unit = null,
        string? description = null
    )
        where T : struct;

    ObservableCounter<T> CreateObservableCounter<T>(
        string name,
        Func<T> observeValue,
        string? unit = null,
        string? description = null
    )
        where T : struct;

    ObservableGauge<T> CreateObservableGauge<T>(
        string name,
        Func<T> observeValue,
        string? unit = null,
        string? description = null
    )
        where T : struct;
}
