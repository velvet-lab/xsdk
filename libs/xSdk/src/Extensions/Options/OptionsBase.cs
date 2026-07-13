using xSdk.Extensions.Variable;

namespace xSdk.Extensions.Options;

public abstract class OptionsBase : VariableSetup
{
    public override string ToString() => base.ToString() ?? string.Empty;
}
