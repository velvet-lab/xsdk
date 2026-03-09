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
