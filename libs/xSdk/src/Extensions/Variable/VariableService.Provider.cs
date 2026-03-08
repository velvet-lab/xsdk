using System.Collections.Concurrent;
using xSdk.Extensions.Variable.Providers;
using xSdk.Shared;

namespace xSdk.Extensions.Variable;

internal partial class VariableService
{
    private Dictionary<string, VariableProvider> _systemProviders;
    private readonly Dictionary<string, VariableProvider> _registeredProviders = new Dictionary<string, VariableProvider>();

    public void RegisterProvider(Type providerType)
    {
        var providerName = providerType.Name;
        if (!_registeredProviders.ContainsKey(providerName))
        {
            var concreteProvider = Activator.CreateInstance(providerType) as VariableProvider;
            if (concreteProvider != null)
            {
                _registeredProviders.Add(providerName, concreteProvider);
            }
        }
    }

    private void InitProviders()
    {
        _systemProviders = new Dictionary<string, VariableProvider>
        {
            { nameof(FileProvider), new FileProvider() },
            { nameof(SystemProvider), new SystemProvider() },
            { nameof(CommandlineProvider), new CommandlineProvider() },
            { nameof(FallbackProvider), new FallbackProvider() },
            { nameof(MemoryProvider), new MemoryProvider() },
        };

        if (_config != null)
        {
            _systemProviders.Add(nameof(OptionProvider), new OptionProvider(_config));
        }
    }

    private Dictionary<string, VariableProvider> Providers => _systemProviders;

    public bool ExistsVariable(string name)
    {
        var variable = LoadVariable(name);

        return Providers
            .Where(x =>
                string.Compare(x.Key, nameof(FileProvider), true) == 0
                || string.Compare(x.Key, nameof(SystemProvider), true) == 0
                || string.Compare(x.Key, nameof(CommandlineProvider), true) == 0
                || string.Compare(x.Key, nameof(OptionProvider), true) == 0
                || string.Compare(x.Key, nameof(MemoryProvider), true) == 0
            )
            .Any(x => x.Value.ContainsVariable(variable));
    }

    public TType ReadVariableValue<TType>(string name, bool shouldThrowIfNotFound = false) =>
        ReadVariableValueInternal<TType>(name, shouldThrowIfNotFound, true);

    internal bool TryReadVariableValue<TType>(string name, out TType value)
    {
        if (ExistsVariable(name))
        {
            var tmpValue = ReadVariableValueInternal<TType>(name, false, false, out bool valueFound);
            if (valueFound)
            {
                value = tmpValue;
                return true;
            }
        }

        value = default;
        return false;
    }

    internal ConcurrentDictionary<string, object> Export()
    {
        var provider = Providers[nameof(MemoryProvider)];
        if (provider != null && provider is MemoryProvider memoryProvider)
        {
            return memoryProvider._variables;
        }
        return new();
    }

    internal void Import(ConcurrentDictionary<string, object> variables)
    {
        var provider = Providers[nameof(MemoryProvider)];
        if (provider != null && provider is MemoryProvider memoryProvider)
        {
            memoryProvider._variables = variables;
        }
    }

    private TType ReadVariableValueInternal<TType>(string name, bool shouldThrowIfNotFound, bool saveVariable) =>
        ReadVariableValueInternal<TType>(name, shouldThrowIfNotFound, saveVariable, out _);

    private TType ReadVariableValueInternal<TType>(string name, bool shouldThrowIfNotFound, bool saveVariable, out bool valueFound)
    {
        valueFound = true;

        var variable = LoadVariableInternal(name);
        if (variable != null)
        {
            // First we try to read already loaded Variable
            if (!Providers[nameof(MemoryProvider)].TryReadVariable(variable, out TType value))
            {
                // then read from Commandline
                if (!Providers[nameof(CommandlineProvider)].TryReadVariable(variable, out value))
                {
                    // then from System Environment
                    if (!Providers[nameof(SystemProvider)].TryReadVariable(variable, out value))
                    {
                        // then from File
                        if (!Providers[nameof(FileProvider)].TryReadVariable(variable, out value))
                        {
                            const string optionsProviderKey = nameof(OptionProvider);
                            if (Providers.ContainsKey(optionsProviderKey))
                            {
                                if (!Providers[optionsProviderKey].TryReadVariable(variable, out value))
                                {
                                    // Last Chance, try to load the Fallback
                                    if (!Fallback(variable, shouldThrowIfNotFound, out value))
                                    {
                                        valueFound = false;
                                    }
                                }
                            }
                            else
                            {
                                // Last Chance, try to load the Fallback
                                if (!Fallback(variable, shouldThrowIfNotFound, out value))
                                {
                                    valueFound = false;
                                }
                            }
                        }
                    }
                }
            }

            if (!valueFound)
            {
                var defaultValue = TryReadDefaultValue<TType>(variable);
                if (defaultValue != null)
                {
                    value = TypeConverter.ConvertTo<TType>(defaultValue);
                }
            }

            if (value == null)
            {
                valueFound = false;
            }
            else
            {
                if (saveVariable)
                {
                    SaveValueToMemoryProvider(variable, value);
                }

                valueFound = true;
                return value;
            }
        }
        else
        {
            valueFound = false;
        }

        if (!valueFound && shouldThrowIfNotFound)
        {
            throw new SdkException($"Automation variable '{name}' has no value.");
        }

        return default;
    }

    private bool Fallback<TType>(IVariable variable, bool shouldThrowIfNotFound, out TType value)
    {
        // Before we use the Fallback Trial, we try to load from registered providers
        if (TryLoadFromRegisteredProviders(variable, out value))
        {
            return true;
        }

        // Last Chance, try to load the Fallback
        if (!Providers[nameof(FallbackProvider)].TryReadVariable(variable, out value))
        {
            if (shouldThrowIfNotFound)
            {
                throw new SdkException($"Automation variable '{variable.Name}' could not found/loaded.");
            }

            return false;
        }

        return true;
    }

    private static object? TryReadDefaultValue<TType>(IVariable variable)
    {
        var typedVariable = variable as Variable<TType>;
        if (typedVariable == null)
        {
            if (variable.ValueType == typeof(bool))
            {
                var item = variable as Variable<bool>;
                return item?.DefaultValue;
            }
            else if (variable.ValueType == typeof(string))
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
        value = default;

        foreach (var registeredProvider in _registeredProviders.Values)
        {
            if (registeredProvider.TryReadVariable(variable, out value))
            {
                found = true;
                break;
            }
        }

        return found;
    }
}
