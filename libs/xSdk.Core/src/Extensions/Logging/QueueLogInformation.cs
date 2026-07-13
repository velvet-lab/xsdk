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

internal class QueueLogInformation(string? categoryName, LogLevel logLevel, EventId eventId, Exception? exception, string? message)
{
    public LogLevel LogLevel => logLevel;

    public EventId EventId => eventId;

    public Exception? Exception => exception;

    public string? CategoryName => categoryName;

    public string? Message => message;
}

internal sealed class QueueLogInformation<TState>(
    string? categoryName,
    LogLevel logLevel,
    EventId eventId,
    TState state,
    Exception? exception,
    Func<TState, Exception?, string> formatter,
    string? message) : QueueLogInformation(categoryName, logLevel, eventId, exception, message)
{
    public TState State => state;

    public Func<TState, Exception?, string> Formatter => formatter;
}
