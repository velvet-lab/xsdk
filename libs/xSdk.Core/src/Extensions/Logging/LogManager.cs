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

using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;

namespace xSdk.Extensions.Logging;

/// <summary>
/// Provides static access to <see cref="ILogger"/> instances for SDK code that cannot
/// use constructor injection (static utility classes, pre-DI bootstrap contexts).
/// </summary>
/// <remarks>
/// <para>
/// Uses a single, centrally initialized <see cref="ILoggerFactory"/> as the single source of truth.
/// The factory is created once via <see cref="Initialize"/> with full configuration (base + plugins),
/// then shared by both SlimHost and Host ServiceProviders.
/// </para>
/// <para>
/// Lifecycle:
/// 1. Host calls Initialize() with plugin configurations before building SlimHost
/// 2. SlimHost registers LogManager.Factory in DI
/// 3. Host registers LogManager.Factory in DI
/// 4. Both use the same factory instance
/// </para>
/// </remarks>
public static class LogManager
{
    private static ILoggerFactory? _factory;
    private static QueueLoggerFactory? _queue = new();
    private static readonly Lock _lock = new();

    /// <summary>
    /// Initializes the shared factory with the specified configuration.
    /// Must be called before SlimHost.Build() to ensure both hosts use the same factory.
    /// </summary>
    internal static void Initialize(ILoggerFactory factory)
    {
        Guard.IsNotNull(factory);

        lock (_lock)
        {
            if (_factory is not null)
            {
                return; // Already initialized
            }

            QueueLogger.FlushQueuedMessages(factory);
            _queue?.Dispose();
            _queue = default;

            _factory = factory;
        }
    }

    public static ILogger CreateLogger(string categoryName)
    {
        return Factory.CreateLogger(categoryName);
    }

    public static ILogger<T> CreateLogger<T>()
    {
        return Factory.CreateLogger<T>();
    }

    public static ILogger CreateLogger(Type type)
    {
        return Factory.CreateLogger(type);
    }

    public static ILogger GetCurrentClassLogger()
    {
        string? className = StackTraceUtils.GetClassFullName(new System.Diagnostics.StackFrame(1, false));
        return CreateLogger(className);
    }

    /// <summary>
    /// Gets the single, shared _logger factory.
    /// Lazy-creates with minimal console configuration if not yet initialized via Initialize().
    /// </summary>
    private static ILoggerFactory Factory
    {
        get
        {
            lock (_lock)
            {
                if (_factory is null)
                {
                    return _queue ?? throw new InvalidOperationException("LogQueue is not initialized or already disposed.");
                }

                return _factory;
            }
        }
    }
}
