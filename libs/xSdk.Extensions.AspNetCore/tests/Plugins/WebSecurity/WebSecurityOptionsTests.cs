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
using xSdk.Extensions.Plugin;
using xSdk.Hosting;
using xSdk.Plugins.WebApi;

namespace xSdk.Plugins.WebSecurity;

public class WebSecurityOptionsTests(WebHostTestFixture fixture) : IClassFixture<WebHostTestFixture>
{
    [Fact]
    public void WebSecuritySetup_DefaultProperties_AreEmpty()
    {
        var setup = new WebSecurityOptions();

        Assert.NotNull(setup);
        Assert.True(string.IsNullOrEmpty(setup.Origins));
    }

    [Fact]
    public void WebSecurityPlugin_CreatedViaHostBuilder()
    {
        IHost host = fixture
            .ConfigureBuilder(builder => builder
                .EnableWebSecurity()
                .EnableWebApi())
            .BuildHost();

        var service = host.Services.GetRequiredService<IPluginService>();
        var plugin = service.GetPlugin<WebSecurityPluginHost>();

        Assert.NotNull(plugin);
    }

    [Fact]
    public void WebSecuritySetup_Definitions_OriginsName_IsCorrect()
    {
        Assert.Equal("origins", WebSecurityOptions.Definitions.Origins.Name);
    }

    [Fact]
    public void WebSecuritySetup_Definitions_OriginsTemplate_IsCorrect()
    {
        Assert.Contains("origins", WebSecurityOptions.Definitions.Origins.Template);
    }

    [Fact]
    public void WebSecurityOptions_IsCorsEnabled_WhenOriginsIsEmpty_ReturnsFalse()
    {
        var setup = new WebSecurityOptions();

        Assert.False(setup.IsCorsEnabled);
    }

    [Fact]
    public void WebSecurityOptions_Definitions_OriginsHelpText_IsNotEmpty()
    {
        Assert.False(string.IsNullOrEmpty(WebSecurityOptions.Definitions.Origins.HelpText));
    }
}
