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
