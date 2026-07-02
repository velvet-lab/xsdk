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

namespace xSdk.Extensions.Variable;

public class VariableSetup : IVariableSetup
{
    private IVariableService? _variableService;

    internal void Initialize(IVariableService? variableService)
    {
        if (variableService != null)
        {
            _variableService = variableService;
            variableService.ParseForVariables(this);

            OnInitialize();
        }
    }

    private IVariableService? GetVariableService()
    {
        if (_variableService == null)
        {
            var variableService = new VariableService(null, null);
            _variableService = variableService;

            variableService.ParseForVariables(this);

            OnInitialize();
        }

        return _variableService;
    }

    protected virtual void OnInitialize()
    {
    }

    protected TValue? ReadValue<TValue>(string name)
        => ReadValue<TValue>(name, false);

    protected TValue? ReadValue<TValue>(string name, bool shouldThrowIfNotFound)
    {
        IVariableService? variableService = GetVariableService();
        if (variableService != null)
        {
            IVariable? variable = variableService.LoadVariable(name);
            if (variable != null)
            {
                return variableService.ReadVariableValue<TValue>(variable.Name, shouldThrowIfNotFound);
            }
        }

        return default;
    }

    protected void SetValue<TValue>(string name, TValue value)
    {
        IVariableService? variableService = GetVariableService();
        if (variableService != null)
        {
            IVariable? variable = variableService.LoadVariable(name);
            if (variable != null)
            {
                variableService.SetVariable(variable.Name, value);
            }
        }
    }
}
