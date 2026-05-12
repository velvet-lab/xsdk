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

namespace xSdk.Data.Models;

public class VariableModelTests
{
    [Fact]
    public void VariableModel_DefaultConstructor_HasEmptyProperties()
    {
        var model = new VariableModel();

        Assert.Equal(string.Empty, model.Name);
        Assert.Equal(string.Empty, model.HelpText);
        Assert.Equal(string.Empty, model.Prefix);
        Assert.False(model.IsHidden);
        Assert.False(model.IsProtected);
        Assert.False(model.NoPrefix);
    }

    [Fact]
    public void VariableModel_SetName_StoresValue()
    {
        var model = new VariableModel
        {
            Name = "my-variable"
        };

        Assert.Equal("my-variable", model.Name);
    }

    [Fact]
    public void VariableModel_SetHelpText_StoresValue()
    {
        var model = new VariableModel
        {
            HelpText = "This is help text"
        };

        Assert.Equal("This is help text", model.HelpText);
    }

    [Fact]
    public void VariableModel_SetPrefix_StoresValue()
    {
        var model = new VariableModel
        {
            Prefix = "app"
        };

        Assert.Equal("app", model.Prefix);
    }

    [Fact]
    public void VariableModel_SetIsHidden_StoresValue()
    {
        var model = new VariableModel
        {
            IsHidden = true
        };

        Assert.True(model.IsHidden);
    }

    [Fact]
    public void VariableModel_SetIsProtected_StoresValue()
    {
        var model = new VariableModel
        {
            IsProtected = true
        };

        Assert.True(model.IsProtected);
    }

    [Fact]
    public void VariableModel_SetNoPrefix_StoresValue()
    {
        var model = new VariableModel
        {
            NoPrefix = true
        };

        Assert.True(model.NoPrefix);
    }
}
