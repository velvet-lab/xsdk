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
using xSdk.Extensions.Variable.Providers;
using xSdk.Tools;

namespace xSdk.Extensions.Variable;

internal partial class VariableService
{
    public ConcurrentBag<IVariable> Variables { get; private set; } = [];

    public IVariable? LoadVariable(string name) => LoadVariableInternal(name);

    public void SetVariable<TValueType>(string name, TValueType value)
    {
        // Sets a Value for a existing Variable
        IVariable? variable = LoadVariableInternal(name);
        if (!TypeConverter.IsEmpty(value, typeof(TValueType)))
        {
            if (variable != null && !variable.IsProtected)
            {
                SaveValueToMemoryProvider(variable, value);
            }
            else
            {
                throw new SdkException($"Value for variable '{name}' could not setted, because variable is write protected");
            }
        }
    }

    internal void AddVariableFromSetupInitialize(Variable variable)
    {
        IVariable? exists = LoadVariable(variable.Name);
        if (exists == null)
        {
            NewVariable(variable);
        }
        else
        {
            ReplaceVariable(variable, true);
        }
    }

    public void NewVariable(IVariable variable) => NewVariable<object>(variable, default, false);

    public void NewVariable(IVariable variable, bool throwIfAlreadyExists) => NewVariable<object>(variable, default, throwIfAlreadyExists);

    public void NewVariable<TValueType>(IVariable variable, TValueType value) => NewVariable<TValueType>(variable, value, false);

    public void NewVariable<TValueType>(IVariable variable, TValueType? value, bool throwIfAlreadyExists)
    {
        AddVariableInternal(variable, throwIfAlreadyExists);
        if (value is not null)
        {
            SetVariable(variable.Name, value);
        }
    }

    private void ReplaceVariable(Variable variable, bool ignoreWriteProtection) => ReplaceVariable<object>(variable, null, ignoreWriteProtection);

    private void ReplaceVariable<TValueType>(Variable variable, TValueType? value, bool ignoreWriteProtection)
    {
        if (variable is null)
        {
            throw new SdkException($"Variable could not replaced, because variable is null");
        }

        // Replace a existing Variable
        IVariable? item = LoadVariableInternal(variable.Name);
        if (item != null)
        {
            if (!item.IsProtected || ignoreWriteProtection)
            {
                var tmp = Variables.ToList();
                tmp.Remove(item);
                tmp.Add(variable);
                Variables = [.. tmp];

                if (!TypeConverter.IsEmpty(value, typeof(TValueType)))
                {
                    SaveValueToMemoryProvider(variable, value);
                }
            }
            else
            {
                throw new SdkException($"Variable '{variable.Name}' could not replaced, because variable is write protected");
            }
        }
        else
        {
            throw new SdkException($"Variable '{variable.Name}' could not replaced, because it does not exist");
        }
    }

    private IVariable? LoadVariableInternal(string name)
    {
        IEnumerable<IVariable> result = Variables.Where(x => string.Compare(x.Name, name, true) == 0);
        if (result.Any())
        {
            if (result.Count() > 1)
            {
                throw new SdkException($"More than one variable with name '{name}' exists");
            }
            else
            {
                IVariable? variable = result.Single();
                if (variable != null)
                {
                    return variable;
                }
            }
        }

        return default;
    }

    private void AddVariableInternal(IVariable variable, bool throwIfAlreadyExists)
    {
        // Adds a variable if not exists
        IVariable? item = LoadVariableInternal(variable.Name);
        if (item == null)
        {
            Variables.Add(variable);
        }
        else
        {
            if (throwIfAlreadyExists)
            {
                throw new SdkException($"Variable '{variable.Name}' could not added, because its already exists");
            }
        }
    }

    private void SaveValueToMemoryProvider(IVariable? variable, object? value)
    {
        if (variable != null)
        {
            var memoryProvider = Providers[nameof(MemoryProvider)] as MemoryProvider;
            memoryProvider?.SaveVariableValue(variable, value);
        }
    }
}
