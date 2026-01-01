using xSdk.Extensions.Variable;

namespace xSdk.Data
{
    public class FlatFileDatabaseSetup : DatabaseSetup, IFlatFileDatabaseSetup
    {
        public FlatFileDatabaseSetup()
        {
            UseLowerCamelCase = true;
            ReloadBeforeGetCollection = false;
        }

        public string FilePath { get; set; }

        public bool UseLowerCamelCase { get; set; }

        public bool ReloadBeforeGetCollection { get; set; }

        public string KeyProperty { get; set; }

        public string EncryptionKey { get; set; }

        protected override void ValidateSetup()
        {
            this.ValidateMember(x => string.IsNullOrEmpty(x.FilePath));
            if (!FilePath.EndsWith(".json"))
            {
                FilePath += ".json";
            }
        }
    }
}
