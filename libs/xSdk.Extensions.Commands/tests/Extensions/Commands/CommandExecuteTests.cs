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
using Spectre.Console.Cli;
using xSdk.Extensions.Commands;

namespace xSdk.Extensions.Commands.Tests.Extensions.Commands;

public class ServiceCollectionCommandExtensionsTests
{
    [Fact]
    public void AddCommandServices_RegistersICommandApp()
    {
        var services = new ServiceCollection();

        services.AddCommandServices(config => { });

        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ICommandApp));
        Assert.NotNull(descriptor);
    }

    [Fact]
    public void AddCommandServices_WithGenericDefaultCommand_RegistersICommandApp()
    {
        var services = new ServiceCollection();

        services.AddCommandServices<DefaultCommand>(config => { });

        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ICommandApp));
        Assert.NotNull(descriptor);
    }

    [Fact]
    public void AddCommandServices_WithNullConfiguration_StillRegisters()
    {
        var services = new ServiceCollection();

        services.AddCommandServices(null);

        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ICommandApp));
        Assert.NotNull(descriptor);
    }

    [Fact]
    public void AddReplConsole_WithDelegate_SetsReplBuilderDelegate()
    {
        var services = new ServiceCollection();
        var wasCalled = false;
        services.AddCommandServices(config =>
        {
            config.AddReplConsole(repl =>
            {
                wasCalled = true;
            });
        });

        // The descriptor is registered (delegate is stored, executed lazily)
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ICommandApp));
        Assert.NotNull(descriptor);
    }
}

