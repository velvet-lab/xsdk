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

using xSdk.Extensions.Commands;

namespace xSdk.Plugin.Tests.Extensions.Commands;

public class CommandlineParserTests
{
    [Fact]
    public void Parse_StringInput_ReturnsArgumentsArray()
    {
        var result = CommandlineParser.Parse("--host localhost --port 8080");

        Assert.NotEmpty(result.Arguments);
        Assert.Contains("--host", result.Arguments);
        Assert.Contains("localhost", result.Arguments);
    }

    [Fact]
    public void Parse_NullInput_ReturnsEmptyArguments()
    {
        var result = CommandlineParser.Parse((string?)null);

        Assert.NotNull(result.Arguments);
    }

    [Fact]
    public void Parse_EmptyInput_ReturnsEmptyArguments()
    {
        var result = CommandlineParser.Parse(string.Empty);

        Assert.NotNull(result.Arguments);
        Assert.Empty(result.Arguments);
    }

    [Fact]
    public void Parse_ArrayInput_JoinsAndParsesArguments()
    {
        var args = new[] { "--host", "localhost", "--port", "8080" };

        var result = CommandlineParser.Parse(args);

        Assert.Contains("--host", result.Arguments);
        Assert.Contains("localhost", result.Arguments);
    }

    [Fact]
    public void ContainsPattern_ExistingPattern_ReturnsTrue()
    {
        var parser = CommandlineParser.Parse("--host localhost");

        Assert.True(parser.ContainsPattern("host"));
    }

    [Fact]
    public void ContainsPattern_WithDashPrefix_ReturnsTrue()
    {
        var parser = CommandlineParser.Parse("--host localhost");

        Assert.True(parser.ContainsPattern("--host"));
    }

    [Fact]
    public void ContainsPattern_NonExistingPattern_ReturnsFalse()
    {
        var parser = CommandlineParser.Parse("--host localhost");

        Assert.False(parser.ContainsPattern("port"));
    }

    [Fact]
    public void ContainsPattern_EmptyPattern_ReturnsFalse()
    {
        var parser = CommandlineParser.Parse("--host localhost");

        Assert.False(parser.ContainsPattern(string.Empty));
    }

    [Fact]
    public void ReadPattern_ExistingPattern_ReturnsValue()
    {
        var parser = CommandlineParser.Parse("--host localhost");

        var value = parser.ReadPattern("host");

        Assert.Equal("localhost", value);
    }

    [Fact]
    public void ReadPattern_NonExistingPattern_ReturnsDefault()
    {
        var parser = CommandlineParser.Parse("--host localhost");

        var value = parser.ReadPattern("port", "9090");

        Assert.Equal("9090", value);
    }

    [Fact]
    public void ReadPattern_NonExistingPatternWithoutDefault_ReturnsNull()
    {
        var parser = CommandlineParser.Parse("--host localhost");

        var value = parser.ReadPattern("port");

        Assert.Null(value);
    }

    [Fact]
    public void AddDefaultArgs_AddsNewArgs_WhenNotAlreadyPresent()
    {
        var parser = CommandlineParser.Parse("--host localhost");

        parser.AddDefaultArgs(["--port", "8080"]);

        Assert.True(parser.ContainsPattern("port"));
    }

    [Fact]
    public void AddDefaultArgs_DoesNotAddExistingArgs()
    {
        var parser = CommandlineParser.Parse("--host localhost");

        parser.AddDefaultArgs(["--host", "remotehost"]);

        Assert.Equal("localhost", parser.ReadPattern("host"));
    }

    [Fact]
    public void AddDefaultArgs_WithNullArgs_ReturnsUnchanged()
    {
        var parser = CommandlineParser.Parse("--host localhost");
        var before = parser.Arguments.Length;

        parser.AddDefaultArgs(null);

        Assert.Equal(before, parser.Arguments.Length);
    }
}
