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

public class LinksAttributeTests
{
    [Fact]
    public void LinksAttribute_WithPolicyName_SetsPolicyName()
    {
        var attribute = new LinksAttribute("MyPolicy");

        Assert.Equal("MyPolicy", attribute.PolicyName);
    }

    [Fact]
    public void LinksAttribute_WithNullPolicyName_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new LinksAttribute(null!));
    }

    [Fact]
    public void LinksAttribute_IsAttributeOnMethod()
    {
        var usage = typeof(LinksAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), inherit: false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        Assert.NotNull(usage);
        Assert.True(usage.ValidOn.HasFlag(AttributeTargets.Method));
    }
}
