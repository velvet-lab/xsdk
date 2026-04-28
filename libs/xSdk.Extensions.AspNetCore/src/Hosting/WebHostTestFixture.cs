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

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace xSdk.Hosting;

public class WebHostTestFixture : TestHostFixture
{
    private readonly List<Action<WebHostBuilderContext, IServiceCollection>> _webhostServicesDelegates = new();

    public WebHostTestFixture() { }

    public WebHostTestFixture ConfigureWebHostServices(Action<WebHostBuilderContext, IServiceCollection> configure)
    {
        _webhostServicesDelegates.Add(configure);
        return this;
    }

    protected override IHostBuilder CreateHostBuilder()
        => TestWebHost.CreateBuilder();

    protected override void Initialize()
    {
        ConfigureBuilder(builder =>
        {
            builder
                .ConfigureWebHost(webhostBuilder =>
                {
                    webhostBuilder
                        .ConfigureServices((context, services) =>
                        {
                            foreach (var configure in _webhostServicesDelegates)
                            {
                                configure?.Invoke(context, services);
                            }
                        });
                });
        });
    }
}
