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

public class BuiltInCommandsTests
{
    // --- ExitCommand ---

    [Fact]
    public void ExitCommand_Definitions_Name_IsExit()
    {
        Assert.Equal("exit", ExitCommand.Definitions.Name);
    }

    [Fact]
    public void ExitCommand_Definitions_HelpText_IsNotEmpty()
    {
        Assert.False(string.IsNullOrWhiteSpace(ExitCommand.Definitions.HelpText));
    }

    [Fact]
    public void ExitCommand_Execute_ReturnsZero()
    {
        var command = new ExitCommand();

        int result = command.Execute();

        Assert.Equal(0, result);
    }

    // --- ClearCommand ---

    [Fact]
    public void ClearCommand_Definitions_Name_IsClear()
    {
        Assert.Equal("clear", ClearCommand.Definitions.Name);
    }

    [Fact]
    public void ClearCommand_Definitions_HelpText_IsNotEmpty()
    {
        Assert.False(string.IsNullOrWhiteSpace(ClearCommand.Definitions.HelpText));
    }

    // --- HelpCommand ---

    [Fact]
    public void HelpCommand_Definitions_Name_IsHelp()
    {
        Assert.Equal("help", HelpCommand.Definitions.Name);
    }

    [Fact]
    public void HelpCommand_Definitions_HelpText_IsNotEmpty()
    {
        Assert.False(string.IsNullOrWhiteSpace(HelpCommand.Definitions.HelpText));
    }
}
