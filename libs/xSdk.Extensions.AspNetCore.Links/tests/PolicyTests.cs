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

public class PolicyTests
{
    private class TestModel : Model
    {
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void Policy_DefaultConstructor_HasEmptyLinksList()
    {
        var policy = new Policy<TestModel>();

        Assert.NotNull(policy.Links);
        Assert.Empty(policy.Links);
    }

    [Fact]
    public void RequireRoutedLink_WithNameAndRoute_AddsLinkToPolicy()
    {
        var policy = new Policy<TestModel>();

        policy.RequireRoutedLink("get", "GetById");

        Assert.Single(policy.Links);
    }

    [Fact]
    public void RequireRoutedLink_WithNameAndRoute_SetsCorrectName()
    {
        var policy = new Policy<TestModel>();

        policy.RequireRoutedLink("list", "GetAll");

        var link = policy.Links[0] as RoutedLink<TestModel>;
        Assert.NotNull(link);
        Assert.Equal("list", link.Name);
    }

    [Fact]
    public void RequireRoutedLink_WithNameAndRoute_SetsCorrectMethodName()
    {
        var policy = new Policy<TestModel>();

        policy.RequireRoutedLink("create", "CreateItem");

        var link = policy.Links[0] as RoutedLink<TestModel>;
        Assert.NotNull(link);
        Assert.Equal("CreateItem", link.MethodName);
    }

    [Fact]
    public void RequireRoutedLink_ChainedCalls_AddsMultipleLinks()
    {
        var policy = new Policy<TestModel>();

        policy
            .RequireRoutedLink("get", "GetById")
            .RequireRoutedLink("list", "GetAll")
            .RequireRoutedLink("delete", "DeleteById");

        Assert.Equal(3, policy.Links.Count);
    }

    [Fact]
    public void RequireRoutedLink_WithValues_AddsLinkWithValues()
    {
        var policy = new Policy<TestModel>();

        policy.RequireRoutedLink("get", "GetById", (TestModel m) => new { id = m.Id });

        Assert.Single(policy.Links);
        var link = policy.Links[0] as RoutedLink<TestModel>;
        Assert.NotNull(link);
        Assert.NotNull(link.Values);
    }
}
