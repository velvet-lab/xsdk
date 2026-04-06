namespace xSdk.Extensions.Variable;

public class VariableSetup
{
    private IVariableService? _variableService;

    internal void Initialize(IVariableService? variableService)
    {
        if (variableService != null)
        {
            variableService.ParseForVariables(this);
            _variableService = variableService;
        }
        OnInitialize();
    }

    protected virtual void OnInitialize()
    {
    }

    protected TValue? ReadValue<TValue>(string name)
        => ReadValue<TValue>(name, false);

    protected TValue? ReadValue<TValue>(string name, bool shouldThrowIfNotFound)
    {
        if (_variableService != null)
        {
            var variable = _variableService.LoadVariable(name);
            if (variable != null)
            {
                return _variableService.ReadVariableValue<TValue>(name, shouldThrowIfNotFound);
            }
        }

        return default;
    }

    protected void SetValue<TValue>(string name, TValue value)
    {
        if (_variableService != null)
        {
            var variable = _variableService?.LoadVariable(name);
            if (variable != null)
            {
                _variableService?.SetVariable(name, value);
            }
        }
    }
}
