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
using xSdk.Data;
using xSdk.Extensions.Links;
using xSdk.Hosting;
using xSdk.Plugins.Links;
using xSdk.Plugins.Links.Mocks;
using xSdk.Plugins.WebApi;

namespace xSdk.Extensions.AspNetCore.Links.Tests;

public class LinksServiceTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    private class TestModel : Model
    {
        public string Name { get; set; } = string.Empty;
    }

    private ILinksService BuildService()
    {
        var host = fixture
            .ConfigureBuilder(builder =>
            {
                builder
                    .EnableWebApi()
                    .EnableLinks<LinksPluginBuilderMock>();
            })
            .BuildHost();

        return host.Services.GetRequiredService<ILinksService>();
    }

    [Fact]
    public async Task AddLinksAsync_SingleModel_DoesNotThrow()
    {
        var service = BuildService();
        var model = new TestModel { Name = "Test" };

        var ex = await Record.ExceptionAsync(() => service.AddLinksAsync(model));

        Assert.Null(ex);
    }

    [Fact]
    public async Task AddLinksAsync_Collection_DoesNotThrow()
    {
        var service = BuildService();
        var models = new List<TestModel>
        {
            new TestModel { Name = "A" },
            new TestModel { Name = "B" },
        };

        var ex = await Record.ExceptionAsync(() => service.AddLinksAsync(models));

        Assert.Null(ex);
    }

    [Fact]
    public async Task AddLinksAsync_WhenNoHttpContext_NoLinksAdded()
    {
        var service = BuildService();
        var model = new TestModel { Name = "Test" };

        await service.AddLinksAsync(model);

        // With no active HttpContext (accessor returns null), _links will be an empty dict
        var linksEntry = model.AdditionalData?["_links"];
        var linksDict = linksEntry as IDictionary<string, IHateoasItem>;
        Assert.NotNull(linksDict);
        Assert.Empty(linksDict);
    }
}
