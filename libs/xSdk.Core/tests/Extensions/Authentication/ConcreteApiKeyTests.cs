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

using System.Security.Claims;
using xSdk.Extensions.Authentication;

namespace xSdk.Extensions.Authentication;

public class ConcreteApiKeyTests
{
    [Fact]
    public void DefaultConstructor_SetsDefaultValues()
    {
        var apiKey = new ConcreteApiKey();

        Assert.Equal(string.Empty, apiKey.Key);
        Assert.Equal(string.Empty, apiKey.OwnerName);
        Assert.NotNull(apiKey.Claims);
    }

    [Fact]
    public void Key_CanBeSet()
    {
        var apiKey = new ConcreteApiKey { Key = "my-key" };

        Assert.Equal("my-key", apiKey.Key);
    }

    [Fact]
    public void OwnerName_CanBeSet()
    {
        var apiKey = new ConcreteApiKey { OwnerName = "alice" };

        Assert.Equal("alice", apiKey.OwnerName);
    }

    [Fact]
    public void Claims_CanBeAssigned()
    {
        var claims = new List<Claim> { new Claim("type", "value") };
        var apiKey = new ConcreteApiKey { Claims = claims };

        Assert.Single(apiKey.Claims);
    }
}
