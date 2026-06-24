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

using xSdk.Extensions.Variable.Providers;
using xSdk.Tools;

namespace xSdk.Extensions.Variable;

internal partial class VariableService
{
    private readonly Dictionary<string, VariableProvider> _registeredProviders = [];

    private Dictionary<string, VariableProvider> Providers { get; set; } = [];

    public void RegisterProvider(Type providerType)
    {
        string providerName = providerType.Name;
        if (!_registeredProviders.ContainsKey(providerName) && Activator.CreateInstance(providerType) is VariableProvider concreteProvider)
        {
            _registeredProviders.Add(providerName, concreteProvider);
        }
    }

    private void InitProviders()
    {
        Providers = new Dictionary<string, VariableProvider>
        {
            { nameof(FileProvider), new FileProvider() },
            { nameof(EnvironmentProvider), new EnvironmentProvider() },
            { nameof(CommandlineProvider), new CommandlineProvider() },
            { nameof(MemoryProvider), new MemoryProvider() },
        };

        if (_applicationOptions != null)
        {
            Providers.Add(nameof(FallbackProvider), new FallbackProvider(_applicationOptions));
            if (_config != null)
            {
                Providers.Add(nameof(OptionProvider), new OptionProvider(_config, _applicationOptions));
            }
        }
    }

    public bool ExistsVariable(string name)
    {
        IVariable? variable = LoadVariable(name);

        if (variable != null)
        {
            return Providers
                .Where(x =>
                    string.Compare(x.Key, nameof(FileProvider), true) == 0
                    || string.Compare(x.Key, nameof(EnvironmentProvider), true) == 0
                    || string.Compare(x.Key, nameof(CommandlineProvider), true) == 0
                    || string.Compare(x.Key, nameof(OptionProvider), true) == 0
                    || string.Compare(x.Key, nameof(MemoryProvider), true) == 0
                )
                .Any(x => x.Value.ContainsVariable(variable));
        }

        return false;
    }

    public TType? ReadVariableValue<TType>(string name, bool shouldThrowIfNotFound = false) =>
        ReadVariableValueInternal<TType>(name, shouldThrowIfNotFound, true);

    internal bool TryReadVariableValue<TType>(string name, out TType? value)
    {
        if (ExistsVariable(name))
        {
            TType? tmpValue = ReadVariableValueInternal<TType>(name, false, false, out bool valueFound);
            if (valueFound)
            {
                value = tmpValue;
                return true;
            }
        }

        value = default;
        return false;
    }

    private TType? ReadVariableValueInternal<TType>(string name, bool shouldThrowIfNotFound, bool saveVariable) =>
        ReadVariableValueInternal<TType>(name, shouldThrowIfNotFound, saveVariable, out _);

    private TType? ReadVariableValueInternal<TType>(string name, bool shouldThrowIfNotFound, bool saveVariable, out bool valueFound)
    {
        valueFound = true;

        IVariable? variable = LoadVariableInternal(name);
        if (!TryReadValuesFromProviders(variable, shouldThrowIfNotFound, out TType? value))
        {
            object? defaultValue = TryReadDefaultValue<TType>(variable);
            if (defaultValue != null)
            {
                value = TypeConverter.ConvertTo<TType>(defaultValue);
            }
        }

        if (saveVariable && value is not null)
        {
            SaveValueToMemoryProvider(variable, value);
        }

        if (shouldThrowIfNotFound && value is null)
        {
            throw new SdkException($"Automation variable '{name}' has no value.");
        }

        return value;
    }

    private bool Fallback<TType>(IVariable variable, bool shouldThrowIfNotFound, out TType? value)
    {
        // Before we use the Fallback Trial, we try to load from registered providers
        if (TryLoadFromRegisteredProviders(variable, out value))
        {
            return true;
        }

        if (Providers.TryGetValue(nameof(FallbackProvider), out VariableProvider? provider) && provider.TryReadVariable(variable, out value))
        {
            return true;
        }

        if (shouldThrowIfNotFound)
        {
            throw new SdkException($"Automation variable '{variable.Name}' could not found/loaded.");
        }

        return false;
    }

    private static object? TryReadDefaultValue<TType>(IVariable? variable)
    {
        if (variable is not Variable<TType> typedVariable)
        {
            if (variable?.ValueType == typeof(bool))
            {
                var item = variable as Variable<bool>;
                return item?.DefaultValue;
            }
            else if (variable?.ValueType == typeof(string))
            {
                var item = variable as Variable<string>;
                return item?.DefaultValue;
            }
        }
        else
        {
            return typedVariable.DefaultValue;
        }

        return default;
    }

    private bool TryLoadFromRegisteredProviders<TType>(IVariable variable, out TType value)
    {
        bool found = false;
#pragma warning disable CS8601 // Mögliche Nullverweiszuweisung.
        value = default;
#pragma warning restore CS8601 // Mögliche Nullverweiszuweisung.

        foreach (VariableProvider registeredProvider in _registeredProviders.Values)
        {
            if (registeredProvider.TryReadVariable(variable, out TType? tmpValue) && tmpValue is not null)
            {
                value = tmpValue;
                found = true;
                break;
            }
        }


        return found;
    }

    private bool TryReadValuesFromProviders<TType>(IVariable? variable, bool shouldThrowIfNotFound, out TType? value)
    {
        if (variable == null)
        {
            value = default;
            return false;
        }

        // First we try to read already loaded Variable
        if (Providers[nameof(MemoryProvider)].TryReadVariable(variable, out TType? tmpValue1))
        {
            value = tmpValue1;
            return true;
        }

        // then read from Commandline
        if (Providers[nameof(CommandlineProvider)].TryReadVariable(variable, out TType? tmpValue2))
        {
            value = tmpValue2;
            return true;
        }

        // then from System Environment
        if (Providers[nameof(EnvironmentProvider)].TryReadVariable(variable, out TType? tmpValue3))
        {
            value = tmpValue3;
            return true;
        }

        // then from File
        if (Providers[nameof(FileProvider)].TryReadVariable(variable, out TType? tmpValue4))
        {
            value = tmpValue4;
            return true;
        }

        // then from Options
        if (Providers.TryGetValue(nameof(OptionProvider), out VariableProvider? optionsProvider) && optionsProvider.TryReadVariable(variable, out TType? tmpValue5))
        {
            value = tmpValue5;
            return true;
        }

        // last Chance, try to load the Fallback
        if (Fallback(variable, shouldThrowIfNotFound, out TType? tmpValue6))
        {
            value = tmpValue6;
            return true;
        }

        value = default;
        return false;
    }
}
