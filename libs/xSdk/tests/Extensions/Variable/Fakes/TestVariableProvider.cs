using xSdk.Extensions.Variable.Providers;

namespace xSdk.Extensions.Variable.Fakes;

internal class TestVariableProvider : VariableProvider
{
    protected override bool ExistsVariable(IVariable variable)
    {
        return false;
    }

    protected override object? ReadVariable(IVariable variable)
    {
        return default;
    }
}
