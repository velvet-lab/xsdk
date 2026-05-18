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

namespace xSdk.Extensions.Links;

public class RoutedLinkBuilderTests
{
    private class TestModel : Model
    {
        public string Name { get; set; } = string.Empty;
    }

    // Dummy controller used to test controller-name stripping in CreateBaseUrl
    private class TestController
    {
        public TestController()
        {
            // No implementation needed for testing purposes
        }
    }

#pragma warning disable CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.

    [Fact]
    public void Build_WhenDescriptionIsNull_ReturnsNull()
    {
        var link = new RoutedLink<TestModel>("self", "GetById", null!);

        IHateoasItem? result = RoutedLinkBuilder.Build(link);

        Assert.Null(result);
    }

    [Fact]
    public void Build_WhenContextIsNull_ReturnsNull()
    {
        var link = new RoutedLink<TestModel>("self", "GetById", null!)
        {
            Description = new MethodDescription
            {
                Action = default,
                ControllerType = typeof(TestModel),
                MethodName = "GetById",
                HttpMethod = HttpMethod.Get
            }
        };
        // Context is null → CreateBaseUrl returns null → Build returns null
        IHateoasItem? result = RoutedLinkBuilder.Build(link);

        Assert.Null(result);
    }

    [Fact]
    public void Build_WhenModelIsNull_ReturnsNull()
    {
        var link = new RoutedLink<TestModel>("self", "GetById", null!)
        {
            Description = new MethodDescription
            {
                Action = default,
                ControllerType = typeof(TestModel),
                MethodName = "GetById",
                HttpMethod = HttpMethod.Get
            },

            Context = new DefaultHttpContext
            {
                Request = { Scheme = "https", Host = new HostString("localhost") }
            }
        };
        // ConcreteModel is null (Model not set) → ReplaceValue returns null
        IHateoasItem? result = RoutedLinkBuilder.Build(link);

        Assert.Null(result);
    }

    [Fact]
    public void Build_WhenAllConditionsMet_NoAuthRequired_ReturnsHateoasItem()
    {
        var model = new TestModel { Name = "Item1" };
        var link = new RoutedLink<TestModel>("self", "GetById", null)
        {
            Description = new MethodDescription
            {
                Action = default,
                ControllerType = typeof(TestController),
                MethodName = "GetById",
                HttpMethod = HttpMethod.Get,
                AuthRoles = [],
                AuthPolicy = null
            },
            Context = new DefaultHttpContext
            {
                Request = { Scheme = "https", Host = new HostString("localhost"), Path = "/api" }
            },
            Model = model
        };
        IHateoasItem? result = RoutedLinkBuilder.Build(link);

        Assert.NotNull(result);
        Assert.Equal("TestController/GetById", result.Rel);
        Assert.Contains("test", result.Href, StringComparison.OrdinalIgnoreCase);
        Assert.Equal("GET", result.Method);
    }

    [Fact]
    public void Build_WhenValuesProvided_RunsHandlebarsReplacement()
    {
        var model = new TestModel { Name = "my-item" };
        var link = new RoutedLink<TestModel>("self", "GetById", m => new { id = m.Name })
        {
            Description = new MethodDescription
            {
                Action = default,
                ControllerType = typeof(TestController),
                MethodName = "GetById",
                HttpMethod = HttpMethod.Get,
                AuthRoles = [],
                AuthPolicy = null
            },
            Context = new DefaultHttpContext
            {
                Request = { Scheme = "https", Host = new HostString("localhost"), Path = "/api" }
            },
            Model = model
        };
        IHateoasItem? result = RoutedLinkBuilder.Build(link);

        Assert.NotNull(result);
        Assert.Equal("GET", result.Method);
    }

    [Fact]
    public void Build_WhenPathContainsControllerName_TrimsPathAtController()
    {
        var model = new TestModel { Name = "x" };
        var link = new RoutedLink<TestModel>("self", "Get", null)
        {
            Description = new MethodDescription
            {
                Action = default,
                ControllerType = typeof(TestController),
                MethodName = "Get",
                HttpMethod = HttpMethod.Get,
                AuthRoles = [],
                AuthPolicy = null
            },
            Context = new DefaultHttpContext
            {
                // path contains "test" → path is trimmed at that position
                Request = { Scheme = "https", Host = new HostString("localhost"), Path = "/api/test/123" }
            },
            Model = model
        };
        IHateoasItem? result = RoutedLinkBuilder.Build(link);

        Assert.NotNull(result);
        Assert.Contains("test", result.Href, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Build_WhenRouteTemplateSet_IsIncludedInUrl()
    {
        var model = new TestModel { Name = "x" };
        var link = new RoutedLink<TestModel>("self", "GetById", null)
        {
            Description = new MethodDescription
            {
                Action = default,
                ControllerType = typeof(TestController),
                MethodName = "GetById",
                HttpMethod = HttpMethod.Put,
                RouteTemplate = "{id}",
                AuthRoles = [],
                AuthPolicy = null
            },
            Context = new DefaultHttpContext
            {
                Request = { Scheme = "http", Host = new HostString("example.com"), Path = "/v1" }
            },
            Model = model
        };
        IHateoasItem? result = RoutedLinkBuilder.Build(link);

        Assert.NotNull(result);
        Assert.Equal("PUT", result.Method);
    }

#pragma warning restore CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.
}
