using System.Globalization;

namespace xSdk.Data;

public class NoSqlDatabaseSetupTests
{
    [Fact]
    public void DefaultPath_IsCurrentDirectory()
    {
        var setup = new NoSqlDatabaseSetup();

        Assert.Equal(System.Environment.CurrentDirectory, setup.Path);
    }

    [Fact]
    public void DefaultInitialSize_IsZero()
    {
        var setup = new NoSqlDatabaseSetup();

        Assert.Equal(0L, setup.InitialSize);
    }

    [Fact]
    public void DefaultUpgrade_IsFalse()
    {
        var setup = new NoSqlDatabaseSetup();

        Assert.False(setup.Upgrade);
    }

    [Fact]
    public void DefaultReadOnly_IsFalse()
    {
        var setup = new NoSqlDatabaseSetup();

        Assert.False(setup.ReadOnly);
    }

    [Fact]
    public void DefaultCollation_IsNotNull()
    {
        var setup = new NoSqlDatabaseSetup();

        Assert.NotNull(setup.Collation);
    }

    [Fact]
    public void SetFileName_RetainsValue()
    {
        var setup = new NoSqlDatabaseSetup
        {
            FileName = "test.db"
        };

        Assert.Equal("test.db", setup.FileName);
    }

    [Fact]
    public void SetPassword_RetainsValue()
    {
        var setup = new NoSqlDatabaseSetup
        {
            Password = "s3cret"
        };

        Assert.Equal("s3cret", setup.Password);
    }

    [Fact]
    public void SetReadOnly_True_RetainsValue()
    {
        var setup = new NoSqlDatabaseSetup
        {
            ReadOnly = true
        };

        Assert.True(setup.ReadOnly);
    }

    [Fact]
    public void SetUpgrade_True_RetainsValue()
    {
        var setup = new NoSqlDatabaseSetup
        {
            Upgrade = true
        };

        Assert.True(setup.Upgrade);
    }

    [Fact]
    public void SetInitialSize_RetainsValue()
    {
        var setup = new NoSqlDatabaseSetup
        {
            InitialSize = 4096L
        };

        Assert.Equal(4096L, setup.InitialSize);
    }
}
