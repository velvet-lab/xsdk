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
using xSdk.Extensions.Plugin;

namespace xSdk.Hosting;

public class PluginHostExtensionsTests
{
    private sealed class ConcretePluginHost : PluginHost
    {
    }

    private sealed class FakePluginHost : PluginDescription, IPluginHost
    {
        public new IServiceProvider Services => null!;
        public void ConfigureServices(IServiceCollection services) { }
        public void ConfigureServices(Microsoft.Extensions.Hosting.HostBuilderContext context, IServiceCollection services) { }
    }

    [Fact]
    public void SetServiceProvider_OnConcretePluginHost_SetsServices()
    {
        var host = new ConcretePluginHost();
        var services = new ServiceCollection().BuildServiceProvider();

        host.SetServiceProvider(services);

        Assert.Same(services, host.Services);
    }

    [Fact]
    public void SetServiceProvider_OnNonPluginHost_ThrowsInvalidOperationException()
    {
        IPluginHost fakeHost = new FakePluginHost();
        var services = new ServiceCollection().BuildServiceProvider();

        Assert.Throws<InvalidOperationException>(() => fakeHost.SetServiceProvider(services));
    }
}
