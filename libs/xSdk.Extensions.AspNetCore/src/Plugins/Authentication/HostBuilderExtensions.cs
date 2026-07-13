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
using xSdk.Extensions.Authentication;
using xSdk.Hosting;

namespace xSdk.Plugins.Authentication;

public static class HostBuilderExtensions
{
    extension(IHostBuilder builder)
    {
        public IHostBuilder EnableAuthentication()
            => builder.EnableAuthentication<AuthBuilder>(_ => { }, _ => { });

        public IHostBuilder EnableAuthentication(Action<AuthBuilder> configure)
            => builder.EnableAuthentication<AuthBuilder>(configure, _ => { });

        public IHostBuilder EnableAuthentication(Action<AuthBuilder> configure, Action<AuthOptions> optionsConfigure)
            => builder.EnableAuthentication<AuthBuilder>(configure, optionsConfigure);

        public IHostBuilder EnableAuthentication<TBuilder>()
            where TBuilder : AuthBuilder
            => builder.EnableAuthentication<TBuilder>(_ => { }, _ => { });

        public IHostBuilder EnableAuthentication<TBuilder>(Action<AuthOptions> configure)
            where TBuilder : AuthBuilder
            => builder.EnableAuthentication<TBuilder>(_ => { }, configure);

        private IHostBuilder EnableAuthentication<TBuilder>(Action<TBuilder> configure, Action<AuthOptions> optionsConfigure)
            where TBuilder : AuthBuilder
            => builder
                .RegisterPluginHost<PluginHost<TBuilder>>()
                .RegisterPluginHostOptions<AuthOptions>(optionsConfigure)
                .RegisterBuilder<TBuilder>(configure);
    }
}
