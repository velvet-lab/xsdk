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

using System.Collections.Concurrent;
using xSdk.Shared;

namespace xSdk.Extensions.Variable.Providers;

internal sealed class MemoryProvider : VariableProviderBase
{
    internal ConcurrentDictionary<string, object> _variables = new ConcurrentDictionary<string, object>();

    protected override bool ExistsVariable(IVariable variable)
    {
        return _variables.ContainsKey(Cast(variable).KeyForSystem);
    }

    protected override object? ReadVariable(IVariable variable)
    {
        if (ExistsVariable(variable))
        {
            return _variables[Cast(variable).KeyForSystem];
        }

        return null;
    }

    internal void SaveVariableValue(IVariable variable, object value)
    {
        if (value != null)
        {
            _variables.AddOrNew(Cast(variable).KeyForSystem, value);
        }
    }

    internal override void Reset()
    {
        _variables = new ConcurrentDictionary<string, object>();

        //var variables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
        //if (variables != null && variables.Count > 0)
        //{
        //    foreach (DictionaryEntry item in variables)
        //    {
        //        var key = item.Key.ToString();
        //        if (key.EndsWith($"{Globals.Constants.VARIABLE_SEPERATOR}LOADED"))
        //        {
        //            Environment.SetEnvironmentVariable(key, null, EnvironmentVariableTarget.Process);

        //            key = key.Replace($"{Globals.Constants.VARIABLE_SEPERATOR}LOADED", "");
        //            Environment.SetEnvironmentVariable(key, null, EnvironmentVariableTarget.Process);
        //        }
        //    }
        //}
    }
}
