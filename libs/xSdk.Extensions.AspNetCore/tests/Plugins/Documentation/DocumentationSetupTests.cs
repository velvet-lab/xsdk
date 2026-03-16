/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
