namespace xSdk.Data
{
    public interface IFlatFileDatabaseSetup
    {
        string FilePath { get; set; }

        bool UseLowerCamelCase { get; set; }

        bool ReloadBeforeGetCollection { get; set; }

        string KeyProperty { get; set; }

        string EncryptionKey { get; set; }
    }
}
