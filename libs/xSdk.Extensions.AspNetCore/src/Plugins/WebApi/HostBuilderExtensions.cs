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
using xSdk.Extensions.WebApi;
using xSdk.Hosting;

namespace xSdk.Plugins.WebApi;

public static class HostBuilderExtensions
{
    extension(IHostBuilder builder)
    {
        public IHostBuilder EnableWebApi()
            => builder.EnableWebApi<WebApiBuilder>(_ => { });

        public IHostBuilder EnableWebApi(Action<WebApiBuilder> configure)
            => builder.EnableWebApi<WebApiBuilder>(configure);

        public IHostBuilder EnableWebApi<TBuilder>()
            where TBuilder : WebApiBuilder
            => builder.EnableWebApi<TBuilder>(_ => { });

        private IHostBuilder EnableWebApi<TBuilder>(Action<TBuilder> configure)
            where TBuilder : WebApiBuilder
            => builder
                .RegisterPluginHost<PluginHost<TBuilder>>()
                .RegisterBuilder<TBuilder>(configure);
    }
}
