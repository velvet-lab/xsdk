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

public class ConsoleOptionsTests
{
    [Fact]
    public void DefaultInstance_DisableDefaultHelp_IsFalse()
    {
        var options = new ConsoleOptions();

        Assert.False(options.DisableDefaultHelp);
    }

    [Fact]
    public void SetDisableDefaultHelp_WithoutVariableService_IsNoOp()
    {
        // VariableSetup.SetValue is a no-op without an IVariableService.
        // Reading the value after setting it returns the default (false).
        var options = new ConsoleOptions();
        options.DisableDefaultHelp = true;

        Assert.False(options.DisableDefaultHelp);
    }

    [Fact]
    public void Definitions_DisableDefaultHelp_Name_IsCorrect()
    {
        Assert.Equal("DisableDefaultHelp", ConsoleOptions.Definitions.DisableDefaultHelp.Name);
    }

    [Fact]
    public void Definitions_DisableDefaultHelp_Template_IsCorrect()
    {
        Assert.Equal("--disable-help", ConsoleOptions.Definitions.DisableDefaultHelp.Template);
    }

    [Fact]
    public void Definitions_DisableDefaultHelp_HelpText_IsNotEmpty()
    {
        Assert.False(string.IsNullOrWhiteSpace(ConsoleOptions.Definitions.DisableDefaultHelp.HelpText));
    }
}
