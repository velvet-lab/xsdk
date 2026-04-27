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

public class PolicyExtensionsTests
{
    private class TestModel : Model
    {
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void RequireRoutedLink_WithNameAndRoute_AddsLinkToPolicy()
    {
        var policy = new Policy<TestModel>();

        var result = policy.RequireRoutedLink("get", "GetById");

        Assert.Same(policy, result);
        Assert.Single(policy.Links);
    }

    [Fact]
    public void RequireRoutedLink_ChainedCalls_AddsMultipleLinks()
    {
        var policy = new Policy<TestModel>();

        policy
            .RequireRoutedLink("get", "GetById")
            .RequireRoutedLink("list", "GetAll")
            .RequireRoutedLink("delete", "Delete");

        Assert.Equal(3, policy.Links.Count);
    }

    [Fact]
    public void RequireRoutedLink_WithValues_AddsLinkWithValues()
    {
        var policy = new Policy<TestModel>();

        policy.RequireRoutedLink("get", "GetById", m => new { id = m.Id });

        Assert.Single(policy.Links);
    }

    [Fact]
    public void RequireRoutedLink_WithNullValues_DoesNotThrow()
    {
        var policy = new Policy<TestModel>();

        policy.RequireRoutedLink("get", "GetById", null);

        Assert.Single(policy.Links);
    }
}

public class LinksOptionsExtensionsTests
{
    private class TestModel : Model
    {
        
    }

    [Fact]
    public void AddPolicy_DoesNotThrow()
    {
        var options = new LinksOptions();

        var ex = Record.Exception(() => options.AddPolicy<TestModel>(p => p.RequireRoutedLink("get", "GetById")));

        Assert.Null(ex);
    }

    [Fact]
    public void AddPolicy_WithNullConfigure_DoesNotThrow()
    {
        var options = new LinksOptions();

        var ex = Record.Exception(() => options.AddPolicy<TestModel>(null!));

        Assert.Null(ex);
    }

    [Fact]
    public void AddPolicy_ReturnsOptions_ForChaining()
    {
        var options = new LinksOptions();

        var result = options.AddPolicy<TestModel>(p => { });

        Assert.Same(options, result);
    }

    [Fact]
    public void AddPolicy_ConfigureActionIsCalled()
    {
        var options = new LinksOptions();
        var called = false;

        options.AddPolicy<TestModel>(p => { called = true; });

        Assert.True(called);
    }
}
