using xSdk.Extensions.Variable;

namespace xSdk.Extensions.Plugin;

public abstract class PluginOptionsBase : VariableSetup
{    
    public override string ToString() => base.ToString() ?? string.Empty;
}
