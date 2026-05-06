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

namespace xSdk.Hosting;

public class HostTests
{
    [Fact]
    public void CreateBuilder_WithNullArgs_ReturnsNonNull()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        IHostBuilder builder = Host.CreateBuilder(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        Assert.NotNull(builder);
    }

    [Fact]
    public void CreateBuilder_WithEmptyArgs_ReturnsNonNull()
    {
        IHostBuilder builder = Host.CreateBuilder(Array.Empty<string>());

        Assert.NotNull(builder);
    }

    [Fact]
    public void CreateBuilder_WithAppName_ReturnsNonNull()
    {
        IHostBuilder builder = Host.CreateBuilder([], "myapp");

        Assert.NotNull(builder);
    }

    [Fact]
    public void CreateBuilder_WithAppNameAndPrefix_ReturnsNonNull()
    {
        IHostBuilder builder = Host.CreateBuilder(new string[] { }, "myapp", "MYAPP");

        Assert.NotNull(builder);
    }

    [Fact]
    public void CreateBuilder_WithAllParams_ReturnsNonNull()
    {
        IHostBuilder builder = Host.CreateBuilder(new string[] { }, "myapp", "mycompany", "MYAPP");

        Assert.NotNull(builder);
    }
}
