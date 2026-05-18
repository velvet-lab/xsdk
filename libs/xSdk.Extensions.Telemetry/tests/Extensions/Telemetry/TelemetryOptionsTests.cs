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
    public void TelemetryOptions_DefaultIsDisabled_IsFalse()
    {
        var options = new TelemetryOptions();

        Assert.False(options.IsDisabled);
    }

    [Fact]
    public void TelemetryOptions_DefaultIsLoggingDisabled_IsFalse()
    {
        var options = new TelemetryOptions();

        Assert.False(options.IsLoggingDisabled);
    }

    [Fact]
    public void TelemetryOptions_DefaultIsTracingDisabled_IsFalse()
    {
        var options = new TelemetryOptions();

        Assert.False(options.IsTracingDisabled);
    }

    [Fact]
    public void TelemetryOptions_DefaultIsMetricsDisabled_IsFalse()
    {
        var options = new TelemetryOptions();

        Assert.False(options.IsMetricsDisabled);
    }

    [Fact]
    public void TelemetryOptions_DefaultIsOtlpExporterDisabled_IsTrue()
    {
        var options = new TelemetryOptions();

        Assert.False(options.IsOtlpExporterDisabled);
    }

    [Fact]
    public void TelemetryOptions_DefaultIsConsoleEnabled_IsFalse()
    {
        var options = new TelemetryOptions();

        Assert.False(options.IsConsoleEnabled);
    }

    [Fact]
    public void TelemetryOptions_SetIsDisabled_StoresValue()
    {
        var options = new TelemetryOptions();

        options.IsDisabled = true;

        Assert.True(options.IsDisabled);
    }

    [Fact]
    public void TelemetryOptions_Definitions_DisableAllName_IsCorrect()
    {
        Assert.Equal("disable-all", TelemetryOptions.Definitions.DisableAll.Name);
    }

    [Fact]
    public void TelemetryOptions_Definitions_LoggingDisabledName_IsCorrect()
    {
        Assert.Equal("disable-logging", TelemetryOptions.Definitions.LoggingDisabled.Name);
    }

    [Fact]
    public void TelemetryOptions_Definitions_TracingDisabledName_IsCorrect()
    {
        Assert.Equal("disable-tracing", TelemetryOptions.Definitions.TracingDisabled.Name);
    }

    [Fact]
    public void TelemetryOptions_Definitions_MetricsDisabledName_IsCorrect()
    {
        Assert.Equal("disable-metrics", TelemetryOptions.Definitions.MetricsDisabled.Name);
    }

    [Fact]
    public void TelemetryOptions_Definitions_OtlpExporterDisabledDefaultValue_IsTrue()
    {
        Assert.True(TelemetryOptions.Definitions.OtlpExporterDisabled.DefaultValue);
    }

    [Fact]
    public void TelemetryOptions_Definitions_EndpointDefaultValue_IsNotEmpty()
    {
        Assert.False(string.IsNullOrEmpty(TelemetryOptions.Definitions.Endpoint.DefaultValue));
    }
}
