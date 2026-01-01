using xSdk.Extensions.Variable;
using LiteDB;
using System.Globalization;

namespace xSdk.Data
{
    public sealed class NoSqlDatabaseSetup : DatabaseSetup
    {
        public NoSqlDatabaseSetup()
        {
            Path = System.Environment.CurrentDirectory;
            InitialSize = 0;
            Upgrade = false;
            ReadOnly = false;
            Collation = new Collation(CultureInfo.CurrentCulture.LCID, CompareOptions.IgnoreCase);
        }

        public string Path { get; set; } = System.Environment.CurrentDirectory;

        public string FileName { get; set; }

        public string Password { get; set; }

        public long InitialSize { get; set; }

        public bool Upgrade { get; set; }

        public bool ReadOnly { get; set; }

        public Collation Collation { get; set; }

        protected override void ValidateSetup()
        {
            if (string.IsNullOrEmpty(this.Path))
                this.Path = System.Environment.CurrentDirectory;

            this.ValidateMember(x => string.IsNullOrEmpty(x.FileName));
        }
    }
}
