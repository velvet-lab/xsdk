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
using xSdk.Extensions.AI;
using xSdk.Hosting;

namespace xSdk.Plugins.AI;

public static class HostBuilderExtensions
{
    extension(IHostBuilder builder)
    {
        public IHostBuilder EnableAI(Action<AIBuilder> configure)
            => builder.EnableAI(configure, _ => { });

        public IHostBuilder EnableAI(Action<AIOptions> optionsConfigure)
            => builder.EnableAI(_ => { }, optionsConfigure);

        public IHostBuilder EnableAI(Action<AIBuilder> configure, Action<AIOptions> optionsConfigure)            
        {
            return builder
                .RegisterPluginHost<PluginHost>()
                .RegisterPluginHostOptions<AIOptions>(optionsConfigure)
                .RegisterPluginServices(services =>
                {
                    services
                        .AddSingleton<YamlDeclarationLoader>();
                })
                .RegisterBuilder<AIBuilder>(configure)
                .RegisterBuilder<ClientBuilder>(ServiceLifetime.Transient)
                .RegisterBuilder<AgentBuilder>(ServiceLifetime.Transient)
                .RegisterBuilder<ToolBuilder>(ServiceLifetime.Transient);
        }
    }
}
