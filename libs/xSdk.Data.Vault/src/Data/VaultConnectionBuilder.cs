namespace xSdk.Data
{
    internal sealed class VaultConnectionBuilder : ConnectionBuilder
    {
        public override object Create(IDatabaseSetup setup)
        {
            return setup;
        }
    }
}
