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

public class SpecificCommandlineParserTests
{
    [Fact]
    public void Create_WithEmptyArgs_ReturnsParserInstance()
    {
        var parser = SpecificCommandlineParser.Create([]);

        Assert.NotNull(parser);
    }

    [Fact]
    public void Create_WithArgs_ReturnsParserWithArguments()
    {
        var parser = SpecificCommandlineParser.Create(["--host", "localhost"]);

        Assert.NotNull(parser);
        Assert.Contains("--host", parser.Arguments);
        Assert.Contains("localhost", parser.Arguments);
    }

    [Fact]
    public void Reparse_WithNewArgs_UpdatesArguments()
    {
        var parser = SpecificCommandlineParser.Create(["--host", "localhost"]);

        parser.Reparse("--port 8080");

        Assert.Contains("--port", parser.Arguments);
        Assert.Contains("8080", parser.Arguments);
    }

    [Fact]
    public void Reparse_WithNull_DoesNotThrow()
    {
        var parser = SpecificCommandlineParser.Create([]);

        var ex = Record.Exception(() => parser.Reparse(null));

        Assert.Null(ex);
    }
}
