using xSdk.Extensions.Variable;
using xSdk.Hosting;
using xSdk.Plugins.Documentation.Mocks;
using xSdk.Plugins.WebApi;

namespace xSdk.Plugins.Documentation;

public class DocumentationSetupTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void DocumentationSetup_DefaultRoutePrefix_IsCorrect()
    {
        fixture.EnablePlugin(b => b.EnableWebApi().EnableDocumentation());
        var setup = fixture.GetRequiredService<IVariableService>().GetSetup<DocumentationSetup>();

        Assert.Equal("api/documentation", setup.RoutePrefix);
    }

    [Fact]
    public void DocumentationSetup_Definitions_RoutePrefixDefaultValue_IsSet()
    {
        Assert.Equal("api/documentation", DocumentationSetup.Definitions.RoutePrefix.DefaultValue);
    }

    [Fact]
    public void DocumentationSetup_Definitions_RoutePrefixName_IsCorrect()
    {
        Assert.Equal("route-prefix", DocumentationSetup.Definitions.RoutePrefix.Name);
    }

    [Fact]
    public void DocumentationSetup_Definitions_RoutePrefixTemplate_ContainsRoute()
    {
        Assert.Contains("route", DocumentationSetup.Definitions.RoutePrefix.Template);
    }
}
