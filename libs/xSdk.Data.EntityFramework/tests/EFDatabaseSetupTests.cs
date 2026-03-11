namespace xSdk.Data;

public class EFDatabaseSetupTests
{
    [Fact]
    public void DefaultTransactionsEnabled_IsTrue()
    {
        var setup = new EntityFrameworkDatabaseSetup();

        Assert.True(setup.TransactionsEnabled);
    }

    [Fact]
    public void SetTransactionsEnabled_False_RetainsFalse()
    {
        var setup = new EntityFrameworkDatabaseSetup
        {
            TransactionsEnabled = false
        };

        Assert.False(setup.TransactionsEnabled);
    }
}
