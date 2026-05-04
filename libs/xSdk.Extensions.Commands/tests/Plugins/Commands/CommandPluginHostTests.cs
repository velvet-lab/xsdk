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
using Spectre.Console.Cli;
using xSdk.Extensions.Plugin;
using xSdk.Hosting;

namespace xSdk.Plugins.Commands;

public class CommandPluginHostTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void EnableCommands_CommandApp_IsRegistered()
    {
        IHost host = fixture
            .ConfigureBuilder(builder => builder.EnableCommands())
            .BuildHost();

        var app = host.Services.GetService<ICommandApp>();

        Assert.NotNull(app);
    }

    [Fact]
    public void EnableCommands_WithDefaultBuilder_CommandPluginIsCreated()
    {
        IHost host = fixture
            .ConfigureBuilder(builder => builder.EnableCommands())
            .BuildHost();

        IPluginService service = host.Services.GetRequiredService<IPluginService>();
        CommandPluginHost? plugin = service.GetPlugin<CommandPluginHost>();

        Assert.NotNull(plugin);
    }
}
