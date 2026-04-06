using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.Options;

[VariableNoPrefix]
public sealed partial class EnvironmentOptions : VariableSetup
{
    protected override void OnInitialize()
    {
        InitializeSystem();
        InitializeService();

        ContentRoot = DetermineContentRoot();
    }
}
