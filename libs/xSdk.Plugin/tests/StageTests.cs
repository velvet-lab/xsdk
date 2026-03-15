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

namespace xSdk.Plugin.Tests;

public class StageTests
{
    [Fact]
    public void None_HasZeroValue()
    {
        Assert.Equal(0, (int)Stage.None);
    }

    [Fact]
    public void Development_HasExpectedValue()
    {
        Assert.Equal(0b_0000_0001, (int)Stage.Development);
    }

    [Fact]
    public void Integration_HasExpectedValue()
    {
        Assert.Equal(0b_0000_0010, (int)Stage.Integration);
    }

    [Fact]
    public void PreProduction_HasExpectedValue()
    {
        Assert.Equal(0b_0000_0100, (int)Stage.PreProduction);
    }

    [Fact]
    public void Production_HasExpectedValue()
    {
        Assert.Equal(0b_0000_1000, (int)Stage.Production);
    }

    [Fact]
    public void All_ContainsDevelopment()
    {
        Assert.True(Stage.All.HasFlag(Stage.Development));
    }

    [Fact]
    public void All_ContainsIntegration()
    {
        Assert.True(Stage.All.HasFlag(Stage.Integration));
    }

    [Fact]
    public void All_ContainsPreProduction()
    {
        Assert.True(Stage.All.HasFlag(Stage.PreProduction));
    }

    [Fact]
    public void All_ContainsProduction()
    {
        Assert.True(Stage.All.HasFlag(Stage.Production));
    }

    [Fact]
    public void FlagsCombination_WorksCorrectly()
    {
        var combined = Stage.Development | Stage.Production;

        Assert.True(combined.HasFlag(Stage.Development));
        Assert.True(combined.HasFlag(Stage.Production));
        Assert.False(combined.HasFlag(Stage.Integration));
    }

    [Theory]
    [InlineData("Development", Stage.Development)]
    [InlineData("Integration", Stage.Integration)]
    [InlineData("PreProduction", Stage.PreProduction)]
    [InlineData("Production", Stage.Production)]
    [InlineData("None", Stage.None)]
    public void Parse_FromString_ReturnsExpectedStage(string input, Stage expected)
    {
        var result = Enum.Parse<Stage>(input);

        Assert.Equal(expected, result);
    }
}
