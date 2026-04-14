///*
// * Copyright 2026 Roland Breitschaft
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using xSdk.Extensions.Plugin;
//using xSdk.Hosting;
//using xSdk.Plugins.Commands;

//namespace xSdk.Extensions.Commands.Tests.Plugins.Commands;

//public class CommandPluginTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
//{
//    [Fact]
//    public void EnableCommands_CreatesCommandPlugin()
//    {
//        IHost host = fixture            
//            .EnablePlugin(x => x.EnableCommands())
//            .BuildHost();

//        IPluginService service = host.Services.GetRequiredService<IPluginService>();
//        CommandPluginHost? plugin = service.GetPlugin<CommandPluginHost>();

//        Assert.NotNull(plugin);
//    }
//}
