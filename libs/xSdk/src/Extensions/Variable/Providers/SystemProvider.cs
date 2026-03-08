using xSdk.Shared;

namespace xSdk.Extensions.Variable.Providers;

internal sealed class SystemProvider : VariableProviderBase
{
    protected override bool ExistsVariable(IVariable variable)
    {
        if (variable != null)
        {
            var result = EnvironmentTools.ReadEnvironmentVariable(Cast(variable).KeyForSystem);
            return !string.IsNullOrEmpty(result);
        }
        return false;
    }

    protected override object ReadVariable(IVariable variable) => EnvironmentTools.ReadEnvironmentVariable(Cast(variable).KeyForSystem);
}
