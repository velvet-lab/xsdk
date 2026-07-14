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
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace xSdk;

public class DiagnosticsTests
{
    [Fact]
    public void SourceName_IsXSdk()
    {
        Assert.Equal("xSdk", Diagnostics.SourceName);
    }

    [Fact]
    public void Source_IsNotNull()
    {
        Assert.NotNull(Diagnostics.Source);
    }

    [Fact]
    public void Source_HasCorrectName()
    {
        Assert.Equal(Diagnostics.SourceName, Diagnostics.Source.Name);
    }

    [Fact]
    public void Meter_IsNotNull()
    {
        Assert.NotNull(Diagnostics.Meter);
    }

    [Fact]
    public void Meter_HasCorrectName()
    {
        Assert.Equal(Diagnostics.SourceName, Diagnostics.Meter.Name);
    }
}

public class DiagnosticsExtensionsTests
{
    [Fact]
    public void AddSdkInstrumentation_TracerProviderBuilder_ReturnsBuilder()
    {
        var builder = Sdk.CreateTracerProviderBuilder();

        var result = builder.AddSdkInstrumentation();

        Assert.NotNull(result);
    }

    [Fact]
    public void AddSdkInstrumentation_MeterProviderBuilder_ReturnsBuilder()
    {
        var builder = Sdk.CreateMeterProviderBuilder();

        var result = builder.AddSdkInstrumentation();

        Assert.NotNull(result);
    }
}
