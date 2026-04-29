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

using Microsoft.AspNetCore.Http;
using xSdk.Data;
using xSdk.Extensions.Links;

namespace xSdk.Extensions.AspNetCore.Links.Tests;

public class RoutedLinkBuilderTests
{
    private class TestModel : Model
    {
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void Build_WhenDescriptionIsNull_ReturnsNull()
    {
        var link = new RoutedLink<TestModel>("self", "GetById", null);
        // Description is not set, so Build should return null
        var builder = new RoutedLinkBuilder();

        var result = builder.Build(link);

        Assert.Null(result);
    }

    [Fact]
    public void Build_WhenContextIsNull_ReturnsNull()
    {
        var link = new RoutedLink<TestModel>("self", "GetById", null);
        link.Description = new MethodDescription
        {
            ControllerType = typeof(TestModel),
            MethodName = "GetById",
            HttpMethod = HttpMethod.Get
        };
        // Context is null, so CreateBaseUrl returns null → Build returns null
        var builder = new RoutedLinkBuilder();

        var result = builder.Build(link);

        Assert.Null(result);
    }

    [Fact]
    public void Build_WhenModelIsNull_ReturnsNull()
    {
        var link = new RoutedLink<TestModel>("self", "GetById", null);
        link.Description = new MethodDescription
        {
            ControllerType = typeof(TestModel),
            MethodName = "GetById",
            HttpMethod = HttpMethod.Get
        };
        link.Context = new DefaultHttpContext
        {
            Request = { Scheme = "https", Host = new HostString("localhost") }
        };
        // ConcreteModel is null (Model not set) → ReplaceValue returns null
        var builder = new RoutedLinkBuilder();

        var result = builder.Build(link);

        Assert.Null(result);
    }
}
