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

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace xSdk.Extensions.Commands;

public class ComponentModelExtensionsTests
{
    private sealed class SampleModel
    {
        [Description("The name of the item")]
        public string? NameWithDescription { get; set; }

        public string? NameWithoutDescription { get; set; }

        [Required]
        public string? RequiredField { get; set; }

        public string? OptionalField { get; set; }

        [Description("Both required and described")]
        [Required]
        public string? RequiredWithDescription { get; set; }
    }

    private static PropertyInfo GetProperty(string name)
        => typeof(SampleModel).GetProperty(name)!;

    [Fact]
    public void Description_WithDescriptionAttribute_ReturnsDescriptionText()
    {
        PropertyInfo prop = GetProperty(nameof(SampleModel.NameWithDescription));

        string? result = prop.Description();

        Assert.Equal("The name of the item", result);
    }

    [Fact]
    public void Description_WithoutDescriptionAttribute_ReturnsNull()
    {
        PropertyInfo prop = GetProperty(nameof(SampleModel.NameWithoutDescription));

        string? result = prop.Description();

        Assert.Null(result);
    }

    [Fact]
    public void IsRequired_WithRequiredAttribute_ReturnsTrue()
    {
        PropertyInfo prop = GetProperty(nameof(SampleModel.RequiredField));

        bool result = prop.IsRequired();

        Assert.True(result);
    }

    [Fact]
    public void IsRequired_WithoutRequiredAttribute_ReturnsFalse()
    {
        PropertyInfo prop = GetProperty(nameof(SampleModel.OptionalField));

        bool result = prop.IsRequired();

        Assert.False(result);
    }

    [Fact]
    public void Description_WithBothAttributes_ReturnsDescriptionText()
    {
        PropertyInfo prop = GetProperty(nameof(SampleModel.RequiredWithDescription));

        string? desc = prop.Description();
        bool req = prop.IsRequired();

        Assert.Equal("Both required and described", desc);
        Assert.True(req);
    }
}
