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

namespace xSdk.Data;

public class NoSqlAttributeTests
{
    // NoSqlIndexAttribute tests

    [Fact]
    public void NoSqlIndexAttribute_DefaultCtor_HasAttributeUsage()
    {
        var attr = new NoSqlIndexAttribute();

        Assert.NotNull(attr);
    }

    [Fact]
    public void NoSqlIndexAttribute_WithUnique_IsApplicable()
    {
        var attr = new NoSqlIndexAttribute(unique: true);

        Assert.NotNull(attr);
    }

    [Fact]
    public void NoSqlIndexAttribute_WithExpressionAndUnique_IsApplicable()
    {
        var attr = new NoSqlIndexAttribute("$.Name", true);

        Assert.NotNull(attr);
    }

    [Fact]
    public void NoSqlIndexAttribute_AttributeTarget_IsPropertyOrField()
    {
        var usage = typeof(NoSqlIndexAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), inherit: false)
            .Cast<AttributeUsageAttribute>()
            .Single();

        Assert.True(usage.ValidOn.HasFlag(AttributeTargets.Property));
        Assert.True(usage.ValidOn.HasFlag(AttributeTargets.Field));
    }

    // NoSqlIgnoreAttribute tests

    [Fact]
    public void NoSqlIgnoreAttribute_DefaultCtor_IsNotNull()
    {
        var attr = new NoSqlIgnoreAttribute();

        Assert.NotNull(attr);
    }

    [Fact]
    public void NoSqlIgnoreAttribute_AttributeTarget_IsPropertyOrField()
    {
        var usage = typeof(NoSqlIgnoreAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), inherit: false)
            .Cast<AttributeUsageAttribute>()
            .Single();

        Assert.True(usage.ValidOn.HasFlag(AttributeTargets.Property));
        Assert.True(usage.ValidOn.HasFlag(AttributeTargets.Field));
    }

    // NoSqlFieldAttribute tests

    [Fact]
    public void NoSqlFieldAttribute_DefaultCtor_IsNotNull()
    {
        var attr = new NoSqlFieldAttribute();

        Assert.NotNull(attr);
    }

    [Fact]
    public void NoSqlFieldAttribute_WithName_IsNotNull()
    {
        var attr = new NoSqlFieldAttribute("myField");

        Assert.NotNull(attr);
    }

    [Fact]
    public void NoSqlFieldAttribute_AttributeTarget_IsPropertyOrField()
    {
        var usage = typeof(NoSqlFieldAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), inherit: false)
            .Cast<AttributeUsageAttribute>()
            .Single();

        Assert.True(usage.ValidOn.HasFlag(AttributeTargets.Property));
        Assert.True(usage.ValidOn.HasFlag(AttributeTargets.Field));
    }
}
