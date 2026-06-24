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

namespace xSdk.Extensions.Commands.Tests.Extensions.Commands;

public class CommandDefinitionsTests
{
    [Fact]
    public void ClearCommand_DefinitionsName_IsClear()
    {
        Assert.Equal("clear", ClearCommand.Definitions.Name);
    }

    [Fact]
    public void ClearCommand_DefinitionsHelpText_IsNotEmpty()
    {
        Assert.False(string.IsNullOrEmpty(ClearCommand.Definitions.HelpText));
    }

    [Fact]
    public void ExitCommand_DefinitionsName_IsExit()
    {
        Assert.Equal("exit", ExitCommand.Definitions.Name);
    }

    [Fact]
    public void ExitCommand_DefinitionsHelpText_IsNotEmpty()
    {
        Assert.False(string.IsNullOrEmpty(ExitCommand.Definitions.HelpText));
    }
}
