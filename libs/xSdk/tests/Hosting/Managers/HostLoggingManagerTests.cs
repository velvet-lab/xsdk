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

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Options;

namespace xSdk.Hosting.Managers;

public class HostLoggingManagerTests
{
    private class TestLoggingBuilder : ILoggingBuilder
    {
        public IServiceCollection Services { get; } = new ServiceCollection();
    }

    [Theory]
    [InlineData("off", LogLevel.None)]
    [InlineData("OFF", LogLevel.None)]
    [InlineData("fatal", LogLevel.Critical)]
    [InlineData("FATAL", LogLevel.Critical)]
    [InlineData("error", LogLevel.Error)]
    [InlineData("warn", LogLevel.Warning)]
    [InlineData("info", LogLevel.Information)]
    [InlineData("debug", LogLevel.Debug)]
    [InlineData("trace", LogLevel.Trace)]
    [InlineData("unknown-value", LogLevel.Information)]
    [InlineData(null, LogLevel.Information)]
    public void ConvertLogLevel_ReturnsExpectedLevel(string? input, LogLevel expected)
    {
        var method = typeof(HostLoggingManager).GetMethod(
            "ConvertLogLevel",
            BindingFlags.NonPublic | BindingFlags.Static);

        var result = (LogLevel)method!.Invoke(null, [input])!;

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConfigureLogging_WithDefaultOptions_DoesNotThrow()
    {
        var builder = new TestLoggingBuilder();
        var options = new EnvironmentOptions();

        var ex = Record.Exception(() => HostLoggingManager.ConfigureLogging(builder, options));

        Assert.Null(ex);
    }

    [Fact]
    public void ConfigureLogging_RegistersServicesInBuilder()
    {
        var builder = new TestLoggingBuilder();
        var options = new EnvironmentOptions();

        HostLoggingManager.ConfigureLogging(builder, options);

        Assert.NotEmpty(builder.Services);
    }

    [Fact]
    public void ResetLogger_WithNullBuilder_DoesNotThrow()
    {
        var ex = Record.Exception(() => HostLoggingManager.ResetLogger(null));

        Assert.Null(ex);
    }

    [Fact]
    public void ResetLogger_WithBuilder_DoesNotThrow()
    {
        var builder = new TestLoggingBuilder();

        var ex = Record.Exception(() => HostLoggingManager.ResetLogger(builder));

        Assert.Null(ex);
    }
}
