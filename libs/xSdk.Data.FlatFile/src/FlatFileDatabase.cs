using JsonFlatFileDataStore;

namespace xSdk.Data
{
    public sealed class FlatFileDatabase : Database
    {
        protected override TConnection Open<TConnection>(Func<object> connectionStringBuilder) => Open<TConnection>(null, connectionStringBuilder);

        protected override TConnection Open<TConnection>(object connection, Func<object> connectionStringBuilder)
        {
            var setup = connectionStringBuilder() as FlatFileDatabaseSetup;

            var datastore = new DataStore(
                setup.FilePath,
                useLowerCamelCase: setup.UseLowerCamelCase,
                keyProperty: setup.KeyProperty,
                reloadBeforeGetCollection: setup.ReloadBeforeGetCollection,
                encryptionKey: setup.EncryptionKey
            );

            return datastore as TConnection;
        }
    }
}
