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

using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Documentation;
using xSdk.Hosting;
using xSdk.Plugins.DataProtection;

namespace xSdk.Plugins.Documentation;

public static class HostBuilderExtensions
{   
    extension(IHostBuilder builder)
    {
        public IHostBuilder EnableDocumentation()
            => builder.EnableDocumentation<DocumentationBuilder>(_ => { }, _ => { });

        public IHostBuilder EnableDocumentation(Action<DocumentationBuilder> configure)
            => builder.EnableDocumentation<DocumentationBuilder>(configure, _ => { });

        public IHostBuilder EnableDocumentation(Action<DocumentationBuilder> configure, Action<DocumentationOptions> optionsConfigure)
            => builder.EnableDocumentation<DocumentationBuilder>(configure, optionsConfigure);

        public IHostBuilder EnableDocumentation<TBuilder>()
            where TBuilder : DocumentationBuilder
            => builder.EnableDocumentation<TBuilder>(_ => { }, _ => { });

        public IHostBuilder EnableDocumentation<TBuilder>(Action<DocumentationOptions> configure)
            where TBuilder : DocumentationBuilder
            => builder.EnableDocumentation<TBuilder>(_ => { }, configure);

        private IHostBuilder EnableDocumentation<TBuilder>(Action<TBuilder> configure, Action<DocumentationOptions> optionsConfigure)
            where TBuilder : DocumentationBuilder
            => builder
                .RegisterPluginHost<PluginHost<TBuilder>>()
                .RegisterPluginHostOptions<DocumentationOptions>(optionsConfigure)
                .RegisterBuilder<TBuilder>(configure);
    }
}
