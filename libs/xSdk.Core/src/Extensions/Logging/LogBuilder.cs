using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Spectre.Console.Rendering;

namespace xSdk.Extensions.Logging;

internal sealed class LogBuilder(ILoggingBuilder builder, LogLevel currentLogLevel) : ILogBuilder
{
    private readonly List<FilterItem> _filters = new ();

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
            if(!string.Equals(provider, Provider))
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

    //public void AddFilterTemp()
    //{
    //    builder.IsLoggingAllowed(level => true);
    //    builder.IsLoggingAllowed((category, level) => true);
    //    builder.IsLoggingAllowed((provider, category, level) => true);
    //    builder.IsLoggingAllowed<ConsoleLoggerProvider>(level => true);
    //    builder.IsLoggingAllowed<ConsoleLoggerProvider>((category, level) => true);

    //    builder.IsLoggingAllowed("category", LogLevel.Information);
    //    builder.IsLoggingAllowed("category", level => true);
    //    builder.IsLoggingAllowed<ConsoleLoggerProvider>("category", LogLevel.Information);
    //    builder.IsLoggingAllowed<ConsoleLoggerProvider>("category", level => true);

    //}    
}
