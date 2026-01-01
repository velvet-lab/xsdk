namespace xSdk.Data
{
    public class EntityFrameworkDatabaseSetup : DatabaseSetup
    {
        public EntityFrameworkDatabaseSetup()
        {
            TransactionsEnabled = true;
        }

        public bool TransactionsEnabled { get; set; }
    }
}
