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

using System.Collections;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xSdk.Extensions.Options;
using xSdk.Hosting;
using xSdk.Tools;

namespace xSdk.Extensions.Variable;

internal partial class VariableService : IVariableService
{
    private readonly IConfiguration? _config;
    private readonly ApplicationOptions _applicationOptions;

    private static readonly ILogger _logger = LogManager.CreateLogger<VariableService>();

    public VariableService(IOptions<ApplicationOptions>? options, IConfiguration? config)
    {
        _config = config;
        _applicationOptions = options?.Value;

        InitProviders();
    }

    public Dictionary<string, object> ToDictionary()
    {
        var result = new Dictionary<string, object>();
        foreach (IVariable variable in Variables)
        {
            try
            {
                if (TryReadVariableValue<object>(variable.Name, out object? value))
                {
                    result.AddOrNew(variable.Name, value);
                }
                else
                {
                    _logger.LogWarning("Variable Value '{0}' not found", variable.Name);
                }
            }
            catch
            {
                // Nothing to tell
            }
        }

        return result;
    }

    internal void AddEnvironmentVariables()
    {
        IDictionary items = Environment.GetEnvironmentVariables();

        // GetPrimaryKey Items to Dictionary
        var dic = new ConcurrentDictionary<string, object>();
        foreach (DictionaryEntry item in items)
        {
            dic?.AddOrNew(item.Key.ToString(), item.Value);            
        }

        // Execute in Parallel
        Parallel.ForEach(
            dic,
            item =>
            {
                string? value = item.Value?.ToString();
                Type valueType = TypeConverter.GetValueType(value);

                if (valueType != null)
                {
                    string name = item.Key.ToString();
                    IVariable? variable = LoadVariableInternal(name);
                    if (variable == null)
                    {
                        variable = Variable.Create(name, valueType).Protect().DisablePrefix().Hide();

                        NewVariable(variable);
                    }
                }
            }
        );
    }
}
