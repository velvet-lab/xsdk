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
using xSdk.Plugins.Authentication;
using xSdk.Plugins.WebApi;

namespace xSdk.Extensions.Authentication;

public class ApiKeyOptionsTests(WebHostTestFixture fixture) : IClassFixture<WebHostTestFixture>
{
    [Fact]
    public void ApiKeySetup_DefaultRealm_HasDefaultValue()
    {
        var setup = new AuthOptions();

        Assert.Equal(AuthOptions.Definitions.Realm.DefaultValue, setup.Realm);
    }

    [Fact]
    public void ApiKeySetup_SetRealm_StoresValue()
    {
        var setup = new AuthOptions
        {
            Realm = "my-test-realm"
        };

        Assert.Equal("my-test-realm", setup.Realm);
    }

    [Fact]
    public void ApiKeySetup_Definitions_RealmName_IsCorrect()
    {
        Assert.Equal("realm", AuthOptions.Definitions.Realm.Name);
    }

    [Fact]
    public void ApiKeySetup_Definitions_RealmTemplate_ContainsRealm()
    {
        Assert.Contains("realm", AuthOptions.Definitions.Realm.Template);
    }

    [Fact]
    public void ApiKeySetup_Definitions_RealmDefaultValue_IsNotEmpty()
    {
        Assert.False(string.IsNullOrEmpty(AuthOptions.Definitions.Realm.DefaultValue));
    }

    [Fact]
    public void AuthenticationPlugin_CreatedViaHostBuilder()
    {
        IHost host = fixture
            .ConfigureBuilder(builder => builder
                .EnableWebApi()
                .EnableAuthentication(builder => { }))
            .BuildHost();

        IPluginService service = host.Services.GetRequiredService<IPluginService>();
        PluginHostBase? plugin = service.GetPlugin<PluginHostBase>();

        Assert.NotNull(plugin);
    }
}
