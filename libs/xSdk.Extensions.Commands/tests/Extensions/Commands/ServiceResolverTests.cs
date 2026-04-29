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

namespace xSdk.Extensions.Commands.Tests.Extensions.Commands;

public class ServiceResolverTests
{
    [Fact]
    public void Resolve_WithRegisteredService_ReturnsInstance()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ITestService, TestService>();
        var provider = services.BuildServiceProvider();
        var resolver = new ServiceResolver(provider);

        var result = resolver.Resolve(typeof(ITestService));

        Assert.NotNull(result);
        Assert.IsType<TestService>(result);
    }

    [Fact]
    public void Resolve_WithNullType_ReturnsNull()
    {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        var resolver = new ServiceResolver(provider);

        var result = resolver.Resolve(null);

        Assert.Null(result);
    }

    [Fact]
    public void Resolve_WithUnregisteredService_ReturnsNull()
    {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        var resolver = new ServiceResolver(provider);

        var result = resolver.Resolve(typeof(ITestService));

        Assert.Null(result);
    }

    private interface ITestService { }

    private class TestService : ITestService { }
}
