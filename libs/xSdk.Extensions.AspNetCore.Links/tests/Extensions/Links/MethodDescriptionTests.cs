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

namespace xSdk.Extensions.Links;

public class MethodDescriptionTests
{
    [Fact]
    public void MethodDescription_ToString_ReturnsMethodName()
    {
#pragma warning disable CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.
        var desc = new MethodDescription
        {
            Action = default,
            ControllerType = default,
            HttpMethod = default,
            MethodName = "GetById"
        };
#pragma warning restore CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.

        Assert.Equal("GetById", desc.ToString());
    }

    [Fact]
    public void MethodDescription_ToString_WhenMethodNameIsNull_ReturnsTypeName()
    {
#pragma warning disable CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.
        var desc = new MethodDescription
        {
            Action = default,
            ControllerType = default,
            HttpMethod = default,
        };
#pragma warning restore CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.

        string result = desc.ToString();

        Assert.Equal(typeof(MethodDescription).Name, result);
    }

    [Fact]
    public void MethodDescription_ShouldAuthorize_WhenAuthRolesNotEmpty_ReturnsTrue()
    {
#pragma warning disable CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.
        var desc = new MethodDescription
        {
            Action = default,
            ControllerType = default,
            HttpMethod = default,
            AuthRoles = ["Admin"]
        };
#pragma warning restore CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.

        Assert.True(desc.ShouldAuthorize);
    }

    [Fact]
    public void MethodDescription_ShouldAuthorize_WhenAuthPolicyNotEmpty_ReturnsTrue()
    {
#pragma warning disable CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.
        var desc = new MethodDescription
        {
            Action = default,
            ControllerType = default,
            HttpMethod = default,
            AuthPolicy = "RequireAuthenticated"
        };
#pragma warning restore CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.

        Assert.True(desc.ShouldAuthorize);
    }

    [Fact]
    public void MethodDescription_ShouldAuthorize_WhenNeitherSet_ReturnsFalse()
    {
#pragma warning disable CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.
        var desc = new MethodDescription
        {
            Action = default,
            ControllerType = default,
            HttpMethod = default,
            AuthRoles = [],
            AuthPolicy = null
        };
#pragma warning restore CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.

        Assert.False(desc.ShouldAuthorize);
    }

    [Fact]
    public void MethodDescription_DefaultAuthRoles_IsEmptyArray()
    {
#pragma warning disable CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.
        var desc = new MethodDescription
        {
            Action = default,
            ControllerType = default,
            HttpMethod = default,
        };
#pragma warning restore CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.

        Assert.NotNull(desc.AuthRoles);
        Assert.Empty(desc.AuthRoles);
    }
}
