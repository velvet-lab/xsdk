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

namespace xSdk.Extensions.Logging;

public class TypeNameHelperTests
{
    // --- GetTypeDisplayName(object?) ---

    [Fact]
    public void GetTypeDisplayName_NullObject_ReturnsNull()
    {
        string? result = TypeNameHelper.GetTypeDisplayName((object?)null);

        Assert.Null(result);
    }

    [Fact]
    public void GetTypeDisplayName_StringObject_ReturnsStringTypeName()
    {
        string? result = TypeNameHelper.GetTypeDisplayName("hello");

        Assert.NotNull(result);
        // TypeNameHelper returns built-in alias "string" for System.String
        Assert.Equal("string", result);
    }

    [Fact]
    public void GetTypeDisplayName_IntObject_ReturnsIntTypeName()
    {
        string? result = TypeNameHelper.GetTypeDisplayName(42);

        Assert.NotNull(result);
        // TypeNameHelper returns built-in alias "int" for System.Int32
        Assert.Equal("int", result);
    }

    // --- GetTypeDisplayName(Type) ---

    [Fact]
    public void GetTypeDisplayName_BuiltInInt_ReturnsInt()
    {
        string result = TypeNameHelper.GetTypeDisplayName(typeof(int), fullName: false);

        Assert.Equal("int", result);
    }

    [Fact]
    public void GetTypeDisplayName_BuiltInString_ReturnsString()
    {
        string result = TypeNameHelper.GetTypeDisplayName(typeof(string), fullName: false);

        Assert.Equal("string", result);
    }

    [Fact]
    public void GetTypeDisplayName_BuiltInBool_ReturnsBool()
    {
        string result = TypeNameHelper.GetTypeDisplayName(typeof(bool), fullName: false);

        Assert.Equal("bool", result);
    }

    [Fact]
    public void GetTypeDisplayName_GenericType_IncludesGenericArguments()
    {
        string result = TypeNameHelper.GetTypeDisplayName(typeof(List<int>), fullName: false);

        Assert.Contains("List", result);
        Assert.Contains("int", result);
    }

    [Fact]
    public void GetTypeDisplayName_ArrayType_IncludesBrackets()
    {
        string result = TypeNameHelper.GetTypeDisplayName(typeof(int[]), fullName: false);

        Assert.Contains("[]", result);
    }

    [Fact]
    public void GetTypeDisplayName_FullName_IncludesNamespace()
    {
        string result = TypeNameHelper.GetTypeDisplayName(typeof(TypeNameHelperTests), fullName: true);

        Assert.Contains("xSdk.Extensions.Logging", result);
    }

    [Fact]
    public void GetTypeDisplayName_ShortName_ExcludesNamespace()
    {
        string result = TypeNameHelper.GetTypeDisplayName(typeof(TypeNameHelperTests), fullName: false);

        Assert.Equal("TypeNameHelperTests", result);
    }

    [Fact]
    public void GetTypeDisplayName_NestedGenericType_ReturnsReadableName()
    {
        string result = TypeNameHelper.GetTypeDisplayName(typeof(Dictionary<string, List<int>>), fullName: false);

        Assert.Contains("Dictionary", result);
        Assert.Contains("string", result);
        Assert.Contains("List", result);
    }

    [Fact]
    public void GetTypeDisplayName_VoidType_ReturnsVoid()
    {
        string result = TypeNameHelper.GetTypeDisplayName(typeof(void), fullName: false);

        Assert.Equal("void", result);
    }

    [Fact]
    public void GetTypeDisplayName_NullableInt_ContainsInt()
    {
        string result = TypeNameHelper.GetTypeDisplayName(typeof(int?), fullName: false);

        Assert.Contains("int", result);
    }

    [Fact]
    public void GetTypeDisplayName_TwoDimensionalArray_ContainsBrackets()
    {
        string result = TypeNameHelper.GetTypeDisplayName(typeof(int[,]), fullName: false);

        Assert.Contains("[,]", result);
    }

    [Fact]
    public void GetTypeDisplayName_GenericTypeWithParameterNames_IncludesParamName()
    {
        string result = TypeNameHelper.GetTypeDisplayName(
            typeof(List<>),
            fullName: false,
            includeGenericParameterNames: true,
            includeGenericParameters: true);

        Assert.Contains("List", result);
    }
}
