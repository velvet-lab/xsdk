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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Plugin;

namespace xSdk.Hosting;

[CLSCompliant(false)]
public class WebHostPluginBase : PluginDescription, IPlugin
{
    public virtual void ConfigureServices(WebHostBuilderContext context, IServiceCollection services) { }

    public virtual void ConfigureDefaults(WebHostBuilderContext context, IApplicationBuilder app) { }

    public virtual void Configure(WebHostBuilderContext context, IApplicationBuilder app) { }

    public virtual void ConfigureEndpoint(IEndpointRouteBuilder builder) { }
}
