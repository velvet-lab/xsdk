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

namespace xSdk.Hosting;

public class HostBuilderExtensionsTests
{
    [Fact]
    public void AddHost_WithHostedService_RegistersService()
    {
        var builder = Host.CreateBuilder(new string[] { }, "testapp", "TEST");

        builder.AddHost<FakeHostedService>();

        var host = builder.Build();

        var service = host.Services.GetService<IHostedService>();
        Assert.NotNull(service);
    }

    [Fact]
    public void AddHost_WithFactory_RegistersService()
    {
        var builder = Host.CreateBuilder(new string[] { }, "testapp", "TEST");

        builder.AddHost<FakeHostedService>(_ => new FakeHostedService());

        var host = builder.Build();

        var service = host.Services.GetService<IHostedService>();
        Assert.NotNull(service);
    }

    private sealed class FakeHostedService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
