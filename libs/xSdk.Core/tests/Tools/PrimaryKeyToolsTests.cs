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

namespace xSdk.Tools;

public class PrimaryKeyToolsTests
{
    [Fact]
    public void Generate_Guid_ReturnsNonEmptyGuid()
    {
        var result = PrimaryKeyTools.Generate<Guid>();

        Assert.NotEqual(Guid.Empty, result);
    }

    [Fact]
    public void Generate_Guid_ReturnsDifferentValuesOnEachCall()
    {
        var result1 = PrimaryKeyTools.Generate<Guid>();
        var result2 = PrimaryKeyTools.Generate<Guid>();

        Assert.NotEqual(result1, result2);
    }

    [Fact]
    public void Generate_Int_ReturnsInt()
    {
        var result = PrimaryKeyTools.Generate<int>();

        Assert.IsType<int>(result);
    }

    [Fact]
    public void Generate_Long_ReturnsLong()
    {
        var result = PrimaryKeyTools.Generate<long>();

        Assert.IsType<long>(result);
    }

    [Fact]
    public void Generate_UnsupportedType_ThrowsNotSupportedException()
    {
        Assert.Throws<NotSupportedException>(() => PrimaryKeyTools.Generate<string>());
    }

    [Fact]
    public void Convert_StringToGuid_ReturnsExpectedGuid()
    {
        var expected = Guid.NewGuid();

        var result = PrimaryKeyTools.Convert<Guid>(expected.ToString());

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_StringToInt_ReturnsExpectedInt()
    {
        var result = PrimaryKeyTools.Convert<int>("42");

        Assert.Equal(42, result);
    }

    [Fact]
    public void Convert_StringToLong_ReturnsExpectedLong()
    {
        var result = PrimaryKeyTools.Convert<long>("123456789012345");

        Assert.Equal(123456789012345L, result);
    }

    [Fact]
    public void Convert_EmptyString_ReturnsDefault()
    {
        var result = PrimaryKeyTools.Convert<int>(string.Empty);

        Assert.Equal(default, result);
    }

    [Fact]
    public void Convert_NullString_ReturnsDefault()
    {
        var result = PrimaryKeyTools.Convert<int>(null);

        Assert.Equal(default, result);
    }

    [Fact]
    public void Convert_GuidToString_ReturnsStringRepresentation()
    {
        var guid = Guid.NewGuid();

        var result = PrimaryKeyTools.Convert<Guid>(guid);

        Assert.Equal(guid.ToString(), result);
    }

    [Fact]
    public void Convert_IntToString_ReturnsStringRepresentation()
    {
        var result = PrimaryKeyTools.Convert<int>(99);

        Assert.Equal("99", result);
    }

    [Fact]
    public void Convert_LongToString_ReturnsStringRepresentation()
    {
        var result = PrimaryKeyTools.Convert<long>(123456789012345L);

        Assert.Equal("123456789012345", result);
    }
}
