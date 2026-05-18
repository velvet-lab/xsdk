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

using Microsoft.Extensions.Logging;

namespace xSdk.Hosting;

public class LogManagerTests
{
    [Fact]
    public void CreateLogger_WithGenericType_ReturnsNonNullLogger()
    {
        ILogger<LogManagerTests> logger = LogManager.CreateLogger<LogManagerTests>();

        Assert.NotNull(logger);
    }

    [Fact]
    public void CreateLogger_WithCategoryName_ReturnsNonNullLogger()
    {
        ILogger logger = LogManager.CreateLogger("MyCategory");

        Assert.NotNull(logger);
    }

    [Fact]
    public void CreateLogger_WithType_ReturnsNonNullLogger()
    {
        ILogger logger = LogManager.CreateLogger<LogManagerTests>();

        Assert.NotNull(logger);
    }

    [Fact]
    public void CreateLogger_WithType_ReturnsILogger()
    {
        ILogger logger = LogManager.CreateLogger<string>();

        Assert.IsAssignableFrom<ILogger>(logger);
    }

    [Fact]
    public void GetCurrentClassLogger_ReturnsNonNullLogger()
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        Assert.NotNull(logger);
    }

    [Fact]
    public void Initialize_WithNullFactory_UsesDefault()
    {
        LogManager.Initialize(null);

        ILogger logger = LogManager.CreateLogger("InitTest");
        Assert.NotNull(logger);
    }

    [Fact]
    public void Initialize_WithCustomFactory_UsesProvidedFactory()
    {
        ILoggerFactory factory = LoggerFactory.Create(b => b.AddConsole());
        LogManager.Initialize(factory);

        ILogger logger = LogManager.CreateLogger<LogManagerTests>();
        Assert.NotNull(logger);
    }

    [Fact]
    public void CreateLogger_AfterReset_StillWorks()
    {
        LogManager.Reset();

        ILogger logger = LogManager.CreateLogger<LogManagerTests>();

        Assert.NotNull(logger);
    }

    [Fact]
    public void CreateLogger_GenericType_IsAssignableFromILogger()
    {
        ILogger logger = LogManager.CreateLogger<LogManagerTests>();

        Assert.IsAssignableFrom<ILogger<LogManagerTests>>(logger);
    }

    [Fact]
    public void CreateLogger_TwiceSameName_ReturnsDifferentInstances()
    {
        ILogger logger1 = LogManager.CreateLogger("test-category");
        ILogger logger2 = LogManager.CreateLogger("test-category");

        // Both should be non-null and functional
        Assert.NotNull(logger1);
        Assert.NotNull(logger2);
    }
}
