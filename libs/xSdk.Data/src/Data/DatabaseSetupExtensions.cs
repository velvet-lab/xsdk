using xSdk.Shared;

namespace xSdk.Data
{
    public static class DatabaseSetupExtensions
    {
        public static IDatabaseSetup AddConnectionProperties(this IDatabaseSetup setup, string key, string value)
        {
            setup.Properties.AddOrNew(key, value);

            return setup;
        }
    }
}
