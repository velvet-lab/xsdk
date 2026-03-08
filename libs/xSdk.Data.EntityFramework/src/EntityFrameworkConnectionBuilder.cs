namespace xSdk.Data;

internal sealed class EntityFrameworkConnectionBuilder : ConnectionBuilder
{
    public override object Create(IDatabaseSetup setup) => setup;
}
