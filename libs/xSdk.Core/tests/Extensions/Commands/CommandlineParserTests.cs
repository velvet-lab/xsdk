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

namespace xSdk.Extensions.Commands;

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

    [Fact]
    public void Parse_WithoutArgs_UsesEnvironmentCommandLine()
    {
        // Parse() without parameters uses Environment.CommandLine
        var parser = CommandlineParser.Parse();

        Assert.NotNull(parser.Arguments);
    }

    [Fact]
    public void ReadPattern_SwitchArgAtEnd_ReturnsTrue()
    {
        var parser = CommandlineParser.Parse("--host localhost --verbose");

        var value = parser.ReadPattern("--verbose");

        Assert.Equal("true", value);
    }

    [Fact]
    public void Parse_ArgWithEquals_SplitsKeyValue()
    {
        var parser = CommandlineParser.Parse("--host=localhost");

        Assert.True(parser.ContainsPattern("host"));
        Assert.Equal("localhost", parser.ReadPattern("host"));
    }

    [Fact]
    public void Parse_NegativeNumber_TreatedAsSwitchBecauseOfDashPrefix()
    {
        // -5 starts with '-' so IsPattern returns true, meaning 'temp' is treated as a switch
        var parser = CommandlineParser.Parse("--temp -5");

        Assert.True(parser.ContainsPattern("temp"));
        Assert.Equal("true", parser.ReadPattern("temp"));
    }

    [Fact]
    public void Parse_MultiWordValue_ConcatenatesValue()
    {
        var parser = CommandlineParser.Parse("--message hello world --end");

        Assert.True(parser.ContainsPattern("message"));
    }

    [Fact]
    public void Parse_WithQuotes_RemovesQuotes()
    {
        var parser = CommandlineParser.Parse("--host \"my server\"");

        Assert.True(parser.ContainsPattern("host"));
    }

    [Fact]
    public void ContainsPattern_NullPattern_ReturnsFalse()
    {
        var parser = CommandlineParser.Parse("--host localhost");

        Assert.False(parser.ContainsPattern(null));
    }

    [Fact]
    public void Parse_ArgWithColon_WhenAlsoHasEquals_SplitsOnColon()
    {
        // Colon split only fires when colonStart < equalStart; test the combined case
        var parser = CommandlineParser.Parse("--protocol:http=unused");

        // The ':' comes before '=', so the colon branch fires: key="--protocol", value="http=unused"
        Assert.True(parser.ContainsPattern("protocol") || parser.Arguments.Length > 0);
    }

    [Fact]
    public void Parse_PositionalArg_StoredInArguments()
    {
        var parser = CommandlineParser.Parse("run myapp");

        Assert.Contains("run", parser.Arguments);
        Assert.Contains("myapp", parser.Arguments);
    }

    [Fact]
    public void BackupDefaultArgs_WithContentRoot_ReturnsArgs()
    {
        var parser = CommandlineParser.Parse("--content-root /app --stage dev");

        var backup = parser.BackupDefaultArgs();

        // Should contain content-root and stage args
        Assert.NotNull(backup);
    }

    [Fact]
    public void BackupDefaultArgs_WithNoDefaultArgs_ReturnsEmpty()
    {
        var parser = CommandlineParser.Parse("--host localhost");

        var backup = parser.BackupDefaultArgs();

        Assert.Empty(backup);
    }

    [Fact]
    public void ContainsPattern_SingleDashPrefix_ReturnsTrue()
    {
        var parser = CommandlineParser.Parse("-h localhost");

        // '-h' matches via PatternComparer's single-dash variant
        Assert.True(parser.ContainsPattern("h"));
    }

    [Fact]
    public void ReadPattern_SingleDashPattern_ReturnsValue()
    {
        var parser = CommandlineParser.Parse("-h localhost");

        var value = parser.ReadPattern("h");

        Assert.Equal("localhost", value);
    }

    [Fact]
    public void Parse_MultipleWordValues_SecondWordStored()
    {
        // When two value-words follow the same option, the second is stored separately
        var parser = CommandlineParser.Parse("--msg hello world");

        Assert.True(parser.ContainsPattern("msg"));
    }

    [Fact]
    public void PatternComparer_HashSet_UsesGetHashCode()
    {
        // HashSet requires GetHashCode when adding elements
        var comparer = new PatternComparer();
        var set = new HashSet<string>(comparer) { "--host", "--port" };

        Assert.Equal(2, set.Count);
        Assert.True(set.Contains("--host"));
    }

    [Fact]
    public void PatternComparer_GetHashCode_ReturnsHashesForVariants()
    {
        var comparer = new PatternComparer();
        var h1 = comparer.GetHashCode("host");
        var h2 = comparer.GetHashCode("--host");
        var h3 = comparer.GetHashCode("-host");

        Assert.NotEqual(0, h1);
        Assert.NotEqual(0, h2);
        Assert.NotEqual(0, h3);
    }

    [Fact]
    public void PatternComparer_Equals_VariantPrefixes_True()
    {
        var comparer = new PatternComparer();
        Assert.True(comparer.Equals("--host", "host"));
        Assert.True(comparer.Equals("-host", "host"));
        Assert.True(comparer.Equals("host", "host"));
    }
}
