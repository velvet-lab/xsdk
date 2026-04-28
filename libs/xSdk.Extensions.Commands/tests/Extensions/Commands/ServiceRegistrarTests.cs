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

public class ServiceRegistrarTests
{
    [Fact]
    public void Build_WithEmptyServices_ReturnsServiceResolver()
    {
        var services = new ServiceCollection();
        var registrar = new ServiceRegistrar(services);

        var resolver = registrar.Build();

        Assert.NotNull(resolver);
    }

    [Fact]
    public void Register_AddsServiceToCollection_CanBeResolved()
    {
        var services = new ServiceCollection();
        var registrar = new ServiceRegistrar(services);

        registrar.Register(typeof(ITestService), typeof(TestService));
        var resolver = registrar.Build();
        var result = resolver.Resolve(typeof(ITestService));

        Assert.NotNull(result);
        Assert.IsType<TestService>(result);
    }

    [Fact]
    public void RegisterInstance_WithInstance_CanBeResolved()
    {
        var services = new ServiceCollection();
        var registrar = new ServiceRegistrar(services);
        var instance = new TestService();

        registrar.RegisterInstance(typeof(ITestService), instance);
        var resolver = registrar.Build();
        var result = resolver.Resolve(typeof(ITestService));

        Assert.NotNull(result);
        Assert.Same(instance, result);
    }

    [Fact]
    public void RegisterLazy_WithFactory_CanBeResolved()
    {
        var services = new ServiceCollection();
        var registrar = new ServiceRegistrar(services);
        var called = false;

        registrar.RegisterLazy(typeof(ITestService), () =>
        {
            called = true;
            return new TestService();
        });
        var resolver = registrar.Build();
        resolver.Resolve(typeof(ITestService));

        Assert.True(called);
    }

    [Fact]
    public void RegisterLazy_WithNullFactory_ThrowsArgumentNullException()
    {
        var services = new ServiceCollection();
        var registrar = new ServiceRegistrar(services);

        Assert.Throws<ArgumentNullException>(() => registrar.RegisterLazy(typeof(ITestService), null!));
    }

    private interface ITestService { }

    private class TestService : ITestService { }
}
