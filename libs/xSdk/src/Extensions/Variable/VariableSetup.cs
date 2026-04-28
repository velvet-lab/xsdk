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
        var variableService = GetVariableService();
        if (variableService != null)
        {
            var variable = variableService.LoadVariable(name);
            if (variable != null)
            {
                return variableService.ReadVariableValue<TValue>(name, shouldThrowIfNotFound);
            }
        }

        return default;
    }

    protected void SetValue<TValue>(string name, TValue value)
    {
        var variableService = GetVariableService();
        if (variableService != null)
        {
            var variable = variableService.LoadVariable(name);
            if (variable != null)
            {
                variableService.SetVariable(name, value);
            }
        }
    }
}
