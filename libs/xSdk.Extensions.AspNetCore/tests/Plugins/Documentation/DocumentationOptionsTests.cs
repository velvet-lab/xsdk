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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using xSdk.Hosting;
using xSdk.Plugins.Documentation.Mocks;
using xSdk.Plugins.WebApi;

namespace xSdk.Plugins.Documentation;

public class DocumentationOptionsTests(WebHostTestFixture fixture) : IClassFixture<WebHostTestFixture>
{
    [Fact]
    public void DocumentationSetup_DefaultRoutePrefix_IsCorrect()
    {
        IHost host = fixture
            .ConfigureBuilder(builder => builder
                .EnableWebApi()
                .EnableDocumentation<DocumentationPluginBuilderMock>())
            .BuildHost();

        DocumentationOptions? options = host.Services
            .GetService<IOptions<DocumentationOptions>>()?.Value;

        Assert.NotNull(options);
        Assert.Equal(DocumentationOptions.Definitions.DocumentPattern.DefaultValue, options.DocumentPattern);
    }

    [Fact]
    public void DocumentationSetup_Definitions_DocumentPatternDefaultValue_IsSet()
    {
        Assert.Equal("openapi/{documentName}.json", DocumentationOptions.Definitions.DocumentPattern.DefaultValue);
    }

    [Fact]
    public void DocumentationSetup_Definitions_DocumentPatternName_IsCorrect()
    {
        Assert.Equal("document-pattern", DocumentationOptions.Definitions.DocumentPattern.Name);
    }

    [Fact]
    public void DocumentationSetup_Definitions_DocumentPatternTemplate_ContainsPattern()
    {
        Assert.Contains("pattern", DocumentationOptions.Definitions.DocumentPattern.Template);
    }
}
