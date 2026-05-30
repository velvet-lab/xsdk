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

namespace xSdk.Extensions.Telemetry;

public class TelemetryOptionsTests
{
    [Fact]
    public void TelemetryOptions_DefaultIsLoggingEnabled_IsFalse()
    {
        var options = new TelemetryPluginOptions();

        Assert.False(options.LoggingEnabled);
    }

    [Fact]
    public void TelemetryOptions_DefaultIsTracingEnabled_IsFalse()
    {
        var options = new TelemetryPluginOptions();

        Assert.False(options.TracingEnabled);
    }

    [Fact]
    public void TelemetryOptions_DefaultIsMetricsEnabled_IsFalse()
    {
        var options = new TelemetryPluginOptions();

        Assert.False(options.MetricsEnabled);
    }
    
    [Fact]
    public void TelemetryOptions_SetIsLoggingEnabled_StoresValue()
    {
        var options = new TelemetryPluginOptions();

        options.LoggingEnabled = true;

        Assert.True(options.LoggingEnabled);
    }

    [Fact]
    public void TelemetryOptions_Definitions_LoggingEnabledName_IsCorrect()
    {
        Assert.Equal("enable-logging", TelemetryPluginOptions.Definitions.LoggingEnabled.Name);
    }

    [Fact]
    public void TelemetryOptions_Definitions_TracingEnabledName_IsCorrect()
    {
        Assert.Equal("enable-tracing", TelemetryPluginOptions.Definitions.TracingEnabled.Name);
    }

    [Fact]
    public void TelemetryOptions_Definitions_MetricsEnabledName_IsCorrect()
    {
        Assert.Equal("enable-metrics", TelemetryPluginOptions.Definitions.MetricsEnabled.Name);
    }
}
