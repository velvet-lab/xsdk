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

/// <summary>
/// Provides static access to <see cref="ILogger"/> instances for SDK code that cannot
/// use constructor injection (static utility classes, pre-DI bootstrap contexts).
/// Uses a Console logger by default so bootstrap messages are always visible.
/// Replaced by the full DI-provided <see cref="ILoggerFactory"/> once the host is running.
/// </summary>
/// <remarks>
/// Replaced automatically via <see cref="LogManagerInitializer"/> which is
/// registered as an <see cref="IHostedService"/> during <c>HostLoggingManager.ConfigureLogging</c>.
/// </remarks>
public static class LogManager
{
    private static ILoggerFactory _factory = LoggerFactory.Create(b => b.AddConsole());
    private static readonly object _lock = new();

    internal static void Initialize(ILoggerFactory? factory)
    {
        lock (_lock)
        {
            _factory = factory ?? LoggerFactory.Create(b => b.AddConsole());
        }
    }

    internal static void Reset()
    {
        lock (_lock)
        {
            _factory = LoggerFactory.Create(b => b.AddConsole());
        }
    }

    public static ILogger CreateLogger(string categoryName)
    {
        lock (_lock)
        {
            return _factory.CreateLogger(categoryName);
        }
    }

    public static ILogger<T> CreateLogger<T>()
    {
        lock (_lock)
        {
            return _factory.CreateLogger<T>();
        }
    }

    public static ILogger CreateLogger(Type type)
    {
        lock (_lock)
        {
            return _factory.CreateLogger(type);
        }
    }

    public static ILogger GetCurrentClassLogger()
    {
        string? className = StackTraceUtils.GetClassFullName(new System.Diagnostics.StackFrame(1, false));
        return CreateLogger(className);
    }
}
