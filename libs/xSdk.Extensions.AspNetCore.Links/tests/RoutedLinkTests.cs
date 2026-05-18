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

namespace xSdk.Extensions.AspNetCore.Links;

public class RoutedLinkTests
{
    private class TestModel : Model
    {
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void RoutedLink_Name_ReturnsConstructorValue()
    {
        var link = new RoutedLink<TestModel>("self", "GetById", null);

        Assert.Equal("self", link.Name);
    }

    [Fact]
    public void RoutedLink_MethodName_ReturnsConstructorValue()
    {
        var link = new RoutedLink<TestModel>("self", "GetById", null);

        Assert.Equal("GetById", link.MethodName);
    }

    [Fact]
    public void RoutedLink_Values_ReturnsConstructorValue()
    {
        static object values(TestModel m) => new { id = m.Id };
        var link = new RoutedLink<TestModel>("self", "GetById", values);

        Assert.NotNull(link.Values);
    }

    [Fact]
    public void RoutedLink_ConcreteModel_WhenModelIsCompatibleType_ReturnsModel()
    {
        var model = new TestModel { Name = "Test" };
        var link = new RoutedLink<TestModel>("self", "GetById", null)
        {
            Model = model
        };

        TestModel? result = link.ConcreteModel;

        Assert.NotNull(result);
        Assert.Same(model, result);
    }

    [Fact]
    public void RoutedLink_ConcreteModel_WhenModelIsNull_ReturnsDefault()
    {
        var link = new RoutedLink<TestModel>("self", "GetById", null);

        TestModel? result = link.ConcreteModel;

        Assert.Null(result);
    }

    [Fact]
    public void RoutedLink_ConcreteModel_WhenModelIsWrongType_ReturnsDefault()
    {
        // Assign a model of a different incompatible type to trigger the default path
        var link = new RoutedLink<TestModel>("self", "GetById", null)
        {
            Model = new OtherModel()
        };

        TestModel? result = link.ConcreteModel;

        Assert.Null(result);
    }

    private class OtherModel : Model { }
}
