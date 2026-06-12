using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace xSdk.Extensions.Logging;

public interface ILogBuilder
{
    LogLevel LogLevel { get; }

    void IsLoggingAllowed<TProvider>(string category, Func<LogLevel, bool> filter) where TProvider : ILoggerProvider;

    void IsLoggingAllowed<TProvider>(string category, LogLevel level) where TProvider : ILoggerProvider;

    void IsLoggingAllowed(string category, Func<LogLevel, bool> filter);

    void IsLoggingAllowed(string category, LogLevel level);

    void IsLoggingAllowed<TProvider>(Func<string?, LogLevel, bool> filter) where TProvider : ILoggerProvider;

    void IsLoggingAllowed<TProvider>(Func<LogLevel, bool> filter) where TProvider : ILoggerProvider;

    void IsLoggingAllowed(Func<string?, LogLevel, bool> filter);

    void IsLoggingAllowed(Func<LogLevel, bool> filter);
}
