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

namespace xSdk.Extensions.Variable.Providers;

public abstract class VariableProvider
{
    protected abstract bool ExistsVariable(IVariable variable);

    protected abstract object? ReadVariable(IVariable variable);

    public bool TryReadVariable<TType>(IVariable variable, out TType value)
    {
        var result = ReadVariable(variable);
        return TryConvertValue(result, variable, out value);
    }

    public bool ContainsVariable(IVariable variable) => ExistsVariable(variable);

    private bool TryConvertValue<TType>(object? value, IVariable variable, out TType? result)
    {
        result = default;
        if (value != null)
        {
            try
            {
                result = (TType)Convert.ChangeType(value, variable.ValueType);
                return true;
            }
            catch
            {
                if (variable.ValueType != null && variable.ValueType.IsEnum)
                {
                    try
                    {
                        result = (TType)Enum.Parse(variable.ValueType, value.ToString(), true);
                        return true;
                    }
                    catch
                    {
                        // Ignore Exception
                    }
                }
            }
        }

        return false;
    }
}
