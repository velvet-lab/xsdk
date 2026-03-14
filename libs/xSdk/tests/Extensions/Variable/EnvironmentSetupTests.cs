namespace xSdk.Extensions.Variable;

public class EnvironmentSetupTests
{
    [Fact]
    public void EnvironmentSetup_AppName_CanBeSet()
    {
        var setup = new EnvironmentSetup();

        setup.AppName = "my-app";

        Assert.Equal("my-app", setup.AppName);
    }

    [Fact]
    public void EnvironmentSetup_AppDescription_CanBeSet()
    {
        var setup = new EnvironmentSetup();

        setup.AppDescription = "My application description";

        Assert.Equal("My application description", setup.AppDescription);
    }

    [Fact]
    public void EnvironmentSetup_AppCompany_CanBeSet()
    {
        var setup = new EnvironmentSetup();

        setup.AppCompany = "Acme Corp";

        Assert.Equal("Acme Corp", setup.AppCompany);
    }

    [Fact]
    public void EnvironmentSetup_AppPrefix_CanBeSet()
    {
        var setup = new EnvironmentSetup();

        setup.AppPrefix = "MYAPP";

        Assert.Equal("MYAPP", setup.AppPrefix);
    }

    [Fact]
    public void EnvironmentSetup_IsDemo_CanBeSet()
    {
        var setup = new EnvironmentSetup();

        setup.IsDemo = true;

        Assert.True(setup.IsDemo);
    }

    [Fact]
    public void EnvironmentSetup_Stage_CanBeSet()
    {
        var setup = new EnvironmentSetup();

        setup.Stage = Stage.Production;

        Assert.Equal(Stage.Production, setup.Stage);
    }

    [Fact]
    public void EnvironmentSetup_ContentRoot_CanBeSet()
    {
        var setup = new EnvironmentSetup();

        setup.ContentRoot = "/app/content";

        Assert.Equal("/app/content", setup.ContentRoot);
    }

    [Fact]
    public void EnvironmentSetup_LogLevel_CanBeSet()
    {
        var setup = new EnvironmentSetup();

        setup.LogLevel = "Debug";

        Assert.Equal("Debug", setup.LogLevel);
    }

    [Fact]
    public void EnvironmentSetup_Definitions_AppName_IsCorrect()
    {
        Assert.Equal("app-name", EnvironmentSetup.Definitions.AppName.Name);
        Assert.Equal("xsdk", EnvironmentSetup.Definitions.AppName.DefaultValue);
    }

    [Fact]
    public void EnvironmentSetup_Definitions_AppCompany_IsCorrect()
    {
        Assert.Equal("app-company", EnvironmentSetup.Definitions.AppCompany.Name);
        Assert.Equal("xcom", EnvironmentSetup.Definitions.AppCompany.DefaultValue);
    }

    [Fact]
    public void EnvironmentSetup_Definitions_AppPrefix_IsCorrect()
    {
        Assert.Equal("app-prefix", EnvironmentSetup.Definitions.AppPrefix.Name);
        Assert.Equal("XSDK", EnvironmentSetup.Definitions.AppPrefix.DefaultValue);
    }

    [Fact]
    public void EnvironmentSetup_Definitions_ServiceNamespace_HasDefaultValue()
    {
        Assert.Equal("xSdk", EnvironmentSetup.Definitions.ServiceNamespace.DefaultValue);
    }

    [Fact]
    public void EnvironmentSetup_IsSlimMode_WhenUsedStandalone_IsTrue()
    {
        var setup = new EnvironmentSetup();
        _ = setup.AppName;

        Assert.True(setup.IsSlimMode);
    }
}
