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
using xSdk.Hosting;

namespace xSdk.Extensions.Variable;

public class VariableServiceTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void GetService_IVariableService_IsRegistered()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        Assert.NotNull(service);
    }

    [Fact]
    public void ToDictionary_ReturnsNonNull()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        var dict = service.ToDictionary();

        Assert.NotNull(dict);
    }

    [Fact]
    public void Variables_InitiallyRegistered_ContainsEntries()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        var dict = service.ToDictionary();

        // After BuildHost, framework variables (stage, log-level, etc.) should be registered
        Assert.NotNull(dict);
    }

    [Fact]
    public void RegisterProvider_WithValidType_DoesNotThrow()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        // Registering a valid provider type should not throw
        var ex = Record.Exception(() => service.RegisterProvider(typeof(Fakes.TestVariableProvider)));

        Assert.Null(ex);
    }
}
