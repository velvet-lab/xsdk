/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using xSdk.Extensions.Variable;

namespace xSdk.Extensions.Telemetry;

internal partial class TelemetryService(EnvironmentSetup envSetup, ILogger<TelemetryService> logger) : ITelemetryService
{
    private ActivitySource? _mainActivitySource;
    private Meter? _mainMeter;

    public ActivitySource MainActivitySource => CreateMainActivitySource();

    public Meter MainMeter => CreateMainMeter();


    public Activity? StartActivity(ActivityKind kind = ActivityKind.Internal, [CallerMemberName] string name = "")
    {
        var source = CreateMainActivitySource();

        var activity = source.StartActivity(name, kind);
        if (activity == null)
        {
            throw new SdkException("Activity could not started");
        }

        return activity;
    }

    public Counter<T> CreateCounter<T>(string name, string unit = null, string description = null)
        where T : struct => CreateInstrument<Counter<T>, T>(meter => meter.CreateCounter<T>(name, unit, description));

    public Histogram<T> CreateHistogram<T>(string name, string unit = null, string description = null)
        where T : struct => CreateInstrument<Histogram<T>, T>(meter => meter.CreateHistogram<T>(name, unit, description));

    public UpDownCounter<T> CreateUpDownCounter<T>(string name, string unit = null, string description = null)
        where T : struct => CreateInstrument<UpDownCounter<T>, T>(meter => meter.CreateUpDownCounter<T>(name, unit, description));

    public ObservableUpDownCounter<T> CreateObservableUpDownCounter<T>(string name, Func<T> observeValue, string unit = null, string description = null)
        where T : struct =>
        CreateObservableInstrument<ObservableUpDownCounter<T>, T>(meter => meter.CreateObservableUpDownCounter(name, observeValue, unit, description));

    public ObservableCounter<T> CreateObservableCounter<T>(string name, Func<T> observeValue, string unit = null, string description = null)
        where T : struct =>
        CreateObservableInstrument<ObservableCounter<T>, T>(meter => meter.CreateObservableCounter(name, observeValue, unit, description));

    public ObservableGauge<T> CreateObservableGauge<T>(string name, Func<T> observeValue, string unit = null, string description = null)
        where T : struct => CreateObservableInstrument<ObservableGauge<T>, T>(meter => meter.CreateObservableGauge(name, observeValue, unit, description));

    internal static ResourceBuilder CreateResourceBuilder(IVariableService variableService)
    {
        var setup = variableService.GetSetup<EnvironmentSetup>();

        return ResourceBuilder
            .CreateDefault()
            .AddEnvironmentVariableDetector()
            .AddTelemetrySdk()
            //.AddAttributes(resources)
            .AddDetector(provider =>
            {
                // Up2date Resources, is better than static resources
                var resources = variableService.BuildResources();
                return new VariableResourceDetector(resources);
            })
            .AddService(serviceName: setup.ServiceName, serviceNamespace: setup.ServiceNamespace, serviceVersion: setup.ServiceVersion);
    }

    private ActivitySource CreateMainActivitySource()
    {
        if (_mainActivitySource == null)
        {
            logger.LogInformation("Create main activity source for tracing");
            _mainActivitySource = new ActivitySource(envSetup.ServiceFullName, envSetup.ServiceVersion);
        }

        return _mainActivitySource;
    }

    private Meter CreateMainMeter()
    {
        if (_mainMeter == null)
        {
            logger.LogInformation("Create main meter for metrics");
            _mainMeter = new Meter(envSetup.ServiceFullName, envSetup.ServiceVersion);
        }

        return _mainMeter;
    }

    private TInstrument CreateInstrument<TInstrument, T>(Func<Meter, TInstrument> configure)
        where TInstrument : Instrument<T>
        where T : struct
    {
        var meter = CreateMainMeter();

        var instrument = configure(meter);
        if (instrument == null)
        {
            throw new SdkException("Meter could not created");
        }
        return instrument;
    }

    private TInstrument CreateObservableInstrument<TInstrument, T>(Func<Meter, TInstrument> configure)
        where TInstrument : ObservableInstrument<T>
        where T : struct
    {
        var meter = CreateMainMeter();

        var instrument = configure(meter);
        if (instrument == null)
        {
            throw new SdkException("Meter could not created");
        }
        return instrument;
    }
}
