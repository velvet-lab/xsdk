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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xSdk.Tools;

namespace xSdk.Data;

public abstract class Database : IDatabase
{
    private readonly Dictionary<string, string> _connectionProperties = new();
    private readonly ILogger<Database> _logger;

    public string? DatalayerName { get; internal set; }

    public IServiceProvider? Services { get; internal set; }

    protected Database(ILogger<Database> logger)
    {
        _logger = logger;

        AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
        {
            Close();
        };
    }

    protected void AddConnectionProperty(string name, string value)
    {
        _connectionProperties.AddOrNew(name, value);
    }

    protected void RemoveConnectionProperty(string name)
    {
        _connectionProperties.Remove(name);
    }

    protected TOptions? GetOptions<TOptions>(OptionsScope scope = OptionsScope.Default)
    {
        var options = Services?.GetService<IOptionsMonitor<TOptions>>();
        if (options != null)
        {
            if (scope == OptionsScope.Datalayer)
            {
                return options.Get(DatalayerName);
            }
            else
            {
                return options.CurrentValue;
            }
        }
        return default;
    }

    #region Dispose Handling

    ~Database() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            Close();
    }

    #endregion

    /// <summary>
    /// Reset the database to a neutral state, semantically similar to when the object was first constructed.
    /// </summary>
    /// <returns><see langword="true" /> if the database was able to reset itself, otherwise <see langword="false" />.</returns>
    /// <remarks>
    /// In general, this method is not expected to be thread-safe.
    /// </remarks>
    public bool TryReset() => Close();

    public abstract TDatabaseObject? Open<TDatabaseObject>()
        where TDatabaseObject : class;

    public virtual bool Close()
    {
        return false;
    }

    protected string ResolvePlaceholders(string content)
    {
        // string name, string fileName
        foreach (var kvp in _connectionProperties)
        {
            var placeholder = kvp.Key;
            var value = kvp.Value;

            if (!placeholder.StartsWith("{"))
                placeholder = "{" + kvp.Key + "}";

            if (content.IndexOf(placeholder, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                content = content.Replace(placeholder, value, StringComparison.InvariantCultureIgnoreCase);
            }

            if (content.IndexOf("{") > -1 && content.IndexOf("}") > -1)
            {
                var leftIndex = content.IndexOf("{");
                if (leftIndex > -1)
                {
                    var rightIndex = content.IndexOf("}", leftIndex);
                    if (rightIndex > -1)
                    {
                        var left = content.Substring(0, leftIndex + 1);
                        var right = content.Substring(rightIndex + 1);

                        content = $"{left}{value}{right}";
                    }
                }
            }
        }
        return content;
    }
}
