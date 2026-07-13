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

using xSdk.Data;
using xSdk.Extensions.Links;

namespace xSdk.Extensions.AspNetCore.Links.Tests;

public class MultiPolicyTests
{
    private class TestModel : Model
    {
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void AddPolicy_ChainedCalls_AddsMultiplePolicies()
    {
        var options = new LinksOptions();

        options
            .AddPolicy<TestModel>(policy => policy.RequireRoutedLink("self", "GetById"))
            .AddPolicy<TestModel>(policy => policy.RequireRoutedLink("list", "GetAll"));

        Assert.Equal(2, options.Policies.Count);
    }

    [Fact]
    public void AddPolicy_ConfigureActionIsCalled_LinkCountMatches()
    {
        var options = new LinksOptions();

        options.AddPolicy<TestModel>(policy =>
        {
            policy.RequireRoutedLink("get", "GetById");
            policy.RequireRoutedLink("list", "GetAll");
        });

        Assert.Single(options.Policies);
        Assert.Equal(2, options.Policies[0].Links.Count);
    }
}
