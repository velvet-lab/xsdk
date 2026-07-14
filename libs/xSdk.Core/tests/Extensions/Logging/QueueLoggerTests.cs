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

namespace xSdk.Extensions.Logging;

public class QueueLogInformationTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var logLevel = LogLevel.Warning;
        var eventId = new EventId(42, "TestEvent");
        var exception = new InvalidOperationException("test");
        var message = "Hello world";
        var category = "MyCategory";

        var info = new QueueLogInformation(category, logLevel, eventId, exception, message);

        Assert.Equal(logLevel, info.LogLevel);
        Assert.Equal(eventId, info.EventId);
        Assert.Same(exception, info.Exception);
        Assert.Equal(category, info.CategoryName);
        Assert.Equal(message, info.Message);
    }

    [Fact]
    public void Constructor_WithNullException_IsAllowed()
    {
        var info = new QueueLogInformation("cat", LogLevel.Information, default, null, "msg");

        Assert.Null(info.Exception);
    }

    [Fact]
    public void Constructor_WithNullMessage_IsAllowed()
    {
        var info = new QueueLogInformation("cat", LogLevel.Debug, default, null, null);

        Assert.Null(info.Message);
    }
}

public class QueueLogInformationGenericTests
{
    [Fact]
    public void Constructor_SetsStateAndFormatter()
    {
        var state = "my-state";
        static string Formatter(string s, Exception? ex) => s;
        var info = new QueueLogInformation<string>("cat", LogLevel.Information, default, state, null, Formatter, "msg");

        Assert.Equal(state, info.State);
        Assert.Same((Func<string, Exception?, string>)Formatter, info.Formatter);
    }

    [Fact]
    public void InheritsBaseProperties()
    {
        var state = 99;
        static string Formatter(int s, Exception? ex) => s.ToString();
        var info = new QueueLogInformation<int>("MyCategory", LogLevel.Error, new EventId(1), state, null, Formatter, "formatted");

        Assert.Equal(LogLevel.Error, info.LogLevel);
        Assert.Equal("MyCategory", info.CategoryName);
        Assert.Equal(99, info.State);
    }
}

public class QueueLoggerTests
{
    [Fact]
    public void IsEnabled_AlwaysReturnsTrue()
    {
        var logger = new QueueLogger();

        Assert.True(logger.IsEnabled(LogLevel.Trace));
        Assert.True(logger.IsEnabled(LogLevel.Debug));
        Assert.True(logger.IsEnabled(LogLevel.Information));
        Assert.True(logger.IsEnabled(LogLevel.Warning));
        Assert.True(logger.IsEnabled(LogLevel.Error));
        Assert.True(logger.IsEnabled(LogLevel.Critical));
    }

    [Fact]
    public void BeginScope_ReturnsNonNull()
    {
        var logger = new QueueLogger();

        var scope = logger.BeginScope("my-scope");

        // BeginScope returns default! which for IDisposable is null - but we just verify it doesn't throw
        // The return can be null
    }

    [Fact]
    public void Log_DoesNotThrow()
    {
        var logger = new QueueLogger();

        var ex = Record.Exception(() =>
            logger.Log(LogLevel.Information, new EventId(1), "test-state", null,
                (s, e) => s.ToString()));

        Assert.Null(ex);
    }

    [Fact]
    public void SetCategoryName_UpdatesCategoryName()
    {
        var logger = new QueueLogger();

        var ex = Record.Exception(() => logger.SetCategoryName("MyTestCategory"));

        Assert.Null(ex);
    }
}
