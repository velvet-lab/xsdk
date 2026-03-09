using System.Collections.Concurrent;
using xSdk.Extensions.Variable.Providers;

namespace xSdk.Extensions.Variable;

internal partial class VariableService
{
    public ConcurrentBag<IVariable> Variables { get; private set; } = new ConcurrentBag<IVariable>();

    internal IVariable LoadVariable(string name) => LoadVariableInternal(name);

    internal void SetVariable<TValueType>(string name, TValueType value)
    {
        // Sets a Value for a existing Variable
        var variable = LoadVariableInternal(name);
        if (value != null)
        {
            if (!variable.IsProtected)
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
        var exists = LoadVariable(variable.Name);
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

    public void NewVariable<TValueType>(IVariable variable, TValueType value, bool throwIfAlreadyExists)
    {
        AddVariableInternal(variable, throwIfAlreadyExists);
        if (value != null)
        {
            SetVariable(variable.Name, value);
        }
    }

    private void ReplaceVariable(Variable variable) => ReplaceVariable<object>(variable, null, false);

    private void ReplaceVariable(Variable variable, bool ignoreWriteProtection) => ReplaceVariable<object>(variable, null, ignoreWriteProtection);

    private void ReplaceVariable<TValueType>(Variable variable, TValueType value, bool ignoreWriteProtection)
    {
        // Replace a existing Variable
        var item = LoadVariableInternal(variable?.Name);
        if (item != null)
        {
            if (!item.IsProtected || ignoreWriteProtection)
            {
                var tmp = Variables.ToList();
                tmp.Remove(item);
                tmp.Add(variable);
                Variables = new ConcurrentBag<IVariable>(tmp);

                if (value != null)
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

    private Variable<TType> LoadVariable<TType>(string name)
    {
        var variable = LoadVariableInternal(name) as Variable<TType>;
        if (variable != null)
        {
            return variable;
        }

        return Variable.Create<TType>(name, v => v.SetHelpText("This is a dynamic generated Variable"));
    }

    private IVariable LoadVariableInternal(string name)
    {
        var result = Variables.Where(x => string.Compare(x.Name, name, true) == 0);
        if (result != null && result.Any())
        {
            if (result.Count() > 1)
            {
                throw new SdkException($"More than one variable with name '{name}' exists");
            }
            else
            {
                var variable = result.SingleOrDefault();
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
        var item = LoadVariableInternal(variable?.Name);
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

    private void SaveValueToMemoryProvider(IVariable variable, object value)
    {
        var memoryProvider = Providers[nameof(MemoryProvider)] as MemoryProvider;
        if (memoryProvider != null)
        {
            memoryProvider.SaveVariableValue(variable, value);
        }
    }
}
