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

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace xSdk.Extensions.Logging;

[ExcludeFromCodeCoverage(Justification = "Logging builder infrastructure — requires a live ILoggingBuilder (DI-only).")]
internal sealed class LogBuilder(ILoggingBuilder builder, LogLevel currentLogLevel) : ILogBuilder
{
    private readonly List<FilterItem> _filters = new();

    private class FilterItem
    {
        public string? Category { get; set; }

        public string? Provider { get; set; }

        public LogLevel LogLevel { get; set; }

        public Func<LogLevel, bool>? Filter { get; set; }

        public Func<string?, LogLevel, bool>? CategoryFilter { get; set; }

        public Func<string?, string?, LogLevel, bool>? ProviderAndCategoryFilter { get; set; }

        public bool IsLoggingAllowed(LogLevel level)
        {
            if (Filter is not null)
            {
                return Filter(level);
            }

            return false;
        }

        public bool IsLoggingAllowed(string? category, LogLevel level)
        {
            if (!string.Equals(category, Category))
            {
                return true;
            }

            if (CategoryFilter is not null)
            {
                return CategoryFilter(category, level);
            }
            else if (Filter is not null)
            {
                return Filter(level);
            }

            return false;
        }
        public bool IsLoggingAllowed(string? provider, string? category, LogLevel level)
        {
            if (!string.Equals(provider, Provider))
            {
                return true;
            }

            if (ProviderAndCategoryFilter is not null)
            {
                return ProviderAndCategoryFilter(provider, category, level);
            }
            else if (CategoryFilter is not null)
            {
                return CategoryFilter(category, level);
            }
            else if (Filter is not null)
            {
                return Filter(level);
            }

            return false;
        }
    }

    public LogLevel LogLevel => currentLogLevel;

    public void IsLoggingAllowed(Func<LogLevel, bool> filter)
    {
        _filters.Add(new FilterItem
        {
            Filter = filter
        });
    }

    public void IsLoggingAllowed(Func<string?, LogLevel, bool> filter)
    {
        _filters.Add(new FilterItem
        {
            CategoryFilter = filter
        });
    }

    public void IsLoggingAllowed<TProvider>(Func<LogLevel, bool> filter)
        where TProvider : ILoggerProvider
    {
        _filters.Add(new FilterItem
        {
            Provider = typeof(TProvider).FullName,
            Filter = filter
        });
    }

    public void IsLoggingAllowed<TProvider>(Func<string?, LogLevel, bool> filter)
        where TProvider : ILoggerProvider
    {
        _filters.Add(new FilterItem
        {
            Provider = typeof(TProvider).FullName,
            CategoryFilter = filter
        });
    }

    public void IsLoggingAllowed(string category, LogLevel level)
    {
        _filters.Add(new FilterItem
        {
            Category = category,
            LogLevel = level
        });
    }

    public void IsLoggingAllowed(string category, Func<LogLevel, bool> filter)
    {
        _filters.Add(new FilterItem
        {
            Category = category,
            Filter = filter
        });
    }

    public void IsLoggingAllowed<TProvider>(string category, LogLevel level)
        where TProvider : ILoggerProvider
    {
        _filters.Add(new FilterItem
        {
            Provider = typeof(TProvider).FullName,
            Category = category,
            LogLevel = level
        });
    }

    public void IsLoggingAllowed<TProvider>(string category, Func<LogLevel, bool> filter)
            where TProvider : ILoggerProvider
    {
        _filters.Add(new FilterItem
        {
            Provider = typeof(TProvider).FullName,
            Category = category,
            Filter = filter
        });
    }

    internal void Build()
    {
        builder.AddFilter(level =>
        {
            var isAllowed = _filters.Any(x => x.IsLoggingAllowed(level));
            return isAllowed;
        });

        builder.AddFilter((category, level) =>
        {
            var isAllowed = _filters.Any(x => x.IsLoggingAllowed(category, level));
            return isAllowed;
        });

        builder.AddFilter((provider, category, level) =>
        {
            var isAllowed = _filters.All(x => x.IsLoggingAllowed(provider, category, level));
            return isAllowed;
        });
    }
}
