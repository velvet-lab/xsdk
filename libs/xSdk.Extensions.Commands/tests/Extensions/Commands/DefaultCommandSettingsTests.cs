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

using xSdk.Extensions.Options;

namespace xSdk.Extensions.Commands;

public class DefaultCommandSettingsTests
{
    [Fact]
    public void DefaultCommandSettings_LogLevel_CanBeSetAndRead()
    {
        var settings = new DefaultConsoleCommandSettings();

        settings.LogLevel = "Debug";

        Assert.Equal("Debug", settings.LogLevel);
    }

    [Fact]
    public void DefaultCommandSettings_Stage_CanBeSetAndRead()
    {
        var settings = new DefaultConsoleCommandSettings();

        settings.Stage = Stage.Production;

        Assert.Equal(Stage.Production, settings.Stage);
    }

    [Fact]
    public void DefaultCommandSettings_IsDemo_DefaultsFalse()
    {
        var settings = new DefaultConsoleCommandSettings();

        Assert.False(settings.IsDemo);
    }

    [Fact]
    public void DefaultCommandSettings_IsDemo_CanBeSetAndRead()
    {
        var settings = new DefaultConsoleCommandSettings();

        settings.IsDemo = true;

        Assert.True(settings.IsDemo);
    }

    [Fact]
    public void DefaultCommandSettings_ContentRoot_CanBeSetAndRead()
    {
        var settings = new DefaultConsoleCommandSettings();

        settings.ContentRoot = "/app/content";

        Assert.Equal("/app/content", settings.ContentRoot);
    }

    [Fact]
    public void Definitions_LogLevel_HasExpectedValues()
    {
        Assert.Equal("log-level", EnvironmentOptions.Definitions.LogLevel.Name);
        Assert.Equal("Info", EnvironmentOptions.Definitions.LogLevel.DefaultValue);
    }

    [Fact]
    public void Definitions_Stage_HasExpectedValues()
    {
        Assert.Equal("stage", EnvironmentOptions.Definitions.Stage.Name);
        Assert.Equal(Stage.Development, EnvironmentOptions.Definitions.Stage.DefaultValue);
    }

    [Fact]
    public void Definitions_Demo_HasExpectedName()
    {
        Assert.Equal("demo", EnvironmentOptions.Definitions.Demo.Name);
    }

    [Fact]
    public void Definitions_ContentRoot_HasExpectedName()
    {
        Assert.Equal("content-root", EnvironmentOptions.Definitions.ContentRoot.Name);
    }

    [Fact]
    public void Definitions_LogLevel_HasTemplate()
    {
        Assert.False(string.IsNullOrEmpty(EnvironmentOptions.Definitions.LogLevel.Template));
    }

    [Fact]
    public void Definitions_Stage_HasHelpText()
    {
        Assert.False(string.IsNullOrEmpty(EnvironmentOptions.Definitions.Stage.HelpText));
    }
}
