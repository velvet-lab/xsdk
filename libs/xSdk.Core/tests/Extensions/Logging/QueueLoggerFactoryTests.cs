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

public class QueueLoggerFactoryTests
{
    [Fact]
    public void CreateLogger_ReturnsNonNull()
    {
        var factory = new QueueLoggerFactory();

        ILogger logger = factory.CreateLogger("TestCategory");

        Assert.NotNull(logger);
    }

    [Fact]
    public void CreateLogger_ReturnsQueueLogger()
    {
        var factory = new QueueLoggerFactory();

        ILogger logger = factory.CreateLogger("TestCategory");

        Assert.IsType<QueueLogger>(logger);
    }

    [Fact]
    public void AddProvider_DoesNotThrow()
    {
        var factory = new QueueLoggerFactory();

        var ex = Record.Exception(() => factory.AddProvider(null!));

        Assert.Null(ex);
    }

    [Fact]
    public void Dispose_DoesNotThrow()
    {
        var factory = new QueueLoggerFactory();

        var ex = Record.Exception(() => factory.Dispose());

        Assert.Null(ex);
    }

    [Fact]
    public void CreateLogger_MultipleTimes_ReturnsSameInstance()
    {
        var factory = new QueueLoggerFactory();

        ILogger logger1 = factory.CreateLogger("Cat1");
        ILogger logger2 = factory.CreateLogger("Cat2");

        // Same underlying QueueLogger instance (category is mutable)
        Assert.Same(logger1, logger2);
    }
}
