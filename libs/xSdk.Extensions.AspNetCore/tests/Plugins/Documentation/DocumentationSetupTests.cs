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

        Assert.Equal(DocumentationSetup.Definitions.DocumentPattern.DefaultValue, setup.DocumentPattern);
    }

    [Fact]
    public void DocumentationSetup_Definitions_DocumentPatternDefaultValue_IsSet()
    {
        Assert.Equal("openapi/{documentName}.json", DocumentationSetup.Definitions.DocumentPattern.DefaultValue);
    }

    [Fact]
    public void DocumentationSetup_Definitions_DocumentPatternName_IsCorrect()
    {
        Assert.Equal("document-pattern", DocumentationSetup.Definitions.DocumentPattern.Name);
    }

    [Fact]
    public void DocumentationSetup_Definitions_DocumentPatternTemplate_ContainsPattern()
    {
        Assert.Contains("pattern", DocumentationSetup.Definitions.DocumentPattern.Template);
    }
}
