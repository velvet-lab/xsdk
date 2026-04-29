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

using Microsoft.AspNetCore.Http;
using xSdk.Extensions.Links;

namespace xSdk.Extensions.AspNetCore.Links.Tests;

public class MethodAnalyzerTests
{
    [Fact]
    public void Analyze_WithNullContext_ReturnsEmptyList()
    {
        var result = MethodAnalyzer.Analyze(null);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Analyze_WithContextHavingNoEndpoint_ReturnsEmptyList()
    {
        var context = new DefaultHttpContext();

        var result = MethodAnalyzer.Analyze(context);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
