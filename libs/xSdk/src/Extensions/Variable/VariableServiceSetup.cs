namespace xSdk.Extensions.Variable;

public sealed class VariableServiceSetup
{
    internal bool AddEnvironmentVariablesWithoutSetup { get; set; }

    //internal bool AddCommanlineVariablesWithoutSetup { get; set; }

    internal List<Type> Providers { get; } = new List<Type>();
}
