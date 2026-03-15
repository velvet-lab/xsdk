using xSdk.Hosting;

namespace xSdk.Plugins.DataProtection;

public class DataProtectionSetupTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void DataProtectionSetup_DefaultProperties_AreEmpty()
    {
        var setup = new DataProtectionSetup();

        Assert.NotNull(setup);
        Assert.True(string.IsNullOrEmpty(setup.ApplicationDiscriminator));
        Assert.True(string.IsNullOrEmpty(setup.ApplicationName));
        Assert.True(string.IsNullOrEmpty(setup.KeyLifetime));
    }

    [Fact]
    public void DataProtectionSetup_SetApplicationDiscriminator_StoresValue()
    {
        var setup = new DataProtectionSetup();

        setup.ApplicationDiscriminator = "my-discriminator";

        Assert.Equal("my-discriminator", setup.ApplicationDiscriminator);
    }

    [Fact]
    public void DataProtectionSetup_SetApplicationName_StoresValue()
    {
        var setup = new DataProtectionSetup();

        setup.ApplicationName = "my-app";

        Assert.Equal("my-app", setup.ApplicationName);
    }

    [Fact]
    public void DataProtectionSetup_SetKeyLifetime_StoresValue()
    {
        var setup = new DataProtectionSetup();

        setup.KeyLifetime = "30d";

        Assert.Equal("30d", setup.KeyLifetime);
    }

    [Fact]
    public void DataProtectionSetup_Definitions_ApplicationDiscriminatorName_IsCorrect()
    {
        Assert.Equal("discriminator", DataProtectionSetup.Definitions.ApplicationDiscriminator.Name);
    }

    [Fact]
    public void DataProtectionSetup_Definitions_ApplicationNameName_IsCorrect()
    {
        Assert.Equal("name", DataProtectionSetup.Definitions.ApplicationName.Name);
    }

    [Fact]
    public void DataProtectionSetup_Definitions_KeyLifetimeName_IsCorrect()
    {
        Assert.Equal("lifetime", DataProtectionSetup.Definitions.KeyLifetime.Name);
    }
}
