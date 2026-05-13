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

using xSdk.Extensions.Links;

namespace xSdk.Extensions.AspNetCore.Links.Tests;

public class MethodDescriptionTests
{
    [Fact]
    public void MethodDescription_ToString_ReturnsMethodName()
    {
        var desc = new MethodDescription { MethodName = "GetById" };

        Assert.Equal("GetById", desc.ToString());
    }

    [Fact]
    public void MethodDescription_ToString_WhenMethodNameIsNull_ReturnsTypeName()
    {
        var desc = new MethodDescription { Action = default };

        var result = desc.ToString();

        Assert.Equal(typeof(MethodDescription).Name, result);
    }

    [Fact]
    public void MethodDescription_ShouldAuthorize_WhenAuthRolesNotEmpty_ReturnsTrue()
    {
        var desc = new MethodDescription { AuthRoles = ["Admin"] };

        Assert.True(desc.ShouldAuthorize);
    }

    [Fact]
    public void MethodDescription_ShouldAuthorize_WhenAuthPolicyNotEmpty_ReturnsTrue()
    {
        var desc = new MethodDescription { AuthPolicy = "RequireAuthenticated" };

        Assert.True(desc.ShouldAuthorize);
    }

    [Fact]
    public void MethodDescription_ShouldAuthorize_WhenNeitherSet_ReturnsFalse()
    {
        var desc = new MethodDescription
        {
            AuthRoles = [],
            AuthPolicy = null
        };

        Assert.False(desc.ShouldAuthorize);
    }

    [Fact]
    public void MethodDescription_DefaultAuthRoles_IsEmptyArray()
    {
        var desc = new MethodDescription();

        Assert.NotNull(desc.AuthRoles);
        Assert.Empty(desc.AuthRoles);
    }
}
