// Copyright 2026 Roland Breitschaft
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections;
using Microsoft.Extensions.Logging;

namespace xSdk.Extensions.Logging;

/// <summary>
/// ILogger implementation that queues log messages until a real _logger is available.
/// Used during SlimHost bootstrapping before the full Host is initialized.
/// </summary>
internal sealed class QueueLogger<T> : ILogger<T>
{
    private readonly ILogger _logger;

    private static string GetCategoryName() => TypeNameHelper.GetTypeDisplayName(typeof(T), includeGenericParameters: false, nestedTypeDelimiter: '.');

    public QueueLogger(ILoggerFactory factory)
    {
        string categoryName = GetCategoryName();
        _logger = factory.CreateLogger(categoryName);
    }
    
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
        => _logger.BeginScope<TState>(state);

    public bool IsEnabled(LogLevel logLevel)
        => _logger.IsEnabled(logLevel);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        => _logger.Log(logLevel, eventId, state, exception, formatter);
}
