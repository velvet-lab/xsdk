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
using Microsoft.Extensions.Logging;

namespace xSdk.Demos;

internal class LocalService(ILogger<LocalService> logger)
{
    private static readonly ActivitySource LocalSource = Diagnostics.Source;    

    // Metrik-Namen folgen lowercase.with.dots-Konvention (ADR-014)
    private readonly Counter<int> _counter = Diagnostics.Meter.CreateCounter<int>("demo.local.work.calls", description: "Count the calls for methods", unit: "times");

    public void DoWorkA()
    {
        logger.LogInformation("Now start a activity");
        using var activity = Diagnostics.Source.StartActivity("do.work.a", ActivityKind.Client);

        _counter.Add(1);
        logger.LogInformation("This is a Sample Log Entry");
        logger.LogWarning("This is a Warning");
        _counter.Add(1);

        Task.Delay(500);

        activity?.SetStatus(ActivityStatusCode.Ok);
    }

    public void DoWorkB()
    {
        logger.LogInformation("Now start a activity");
        using var activity = Diagnostics.Source.StartActivity("do.work.b", ActivityKind.Client);

        try
        {
            Task.Delay(500);

            _counter.Add(1);

            logger.LogInformation("Try to divide with zero");
            var a = 1;
            var b = 0;
            _counter.Add(1);
            var c = a / b;

            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
        }
    }
}
