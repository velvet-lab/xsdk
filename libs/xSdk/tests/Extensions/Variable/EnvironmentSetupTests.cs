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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using xSdk.Extensions.Options;
using xSdk.Hosting;

namespace xSdk.Extensions.Variable;

public class EnvironmentSetupTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    private EnvironmentOptions GetEnvironmentOptions()
        => fixture.BuildHost().Services.GetRequiredService<IOptions<EnvironmentOptions>>().Value;

    [Fact]
    public void EnvironmentOptions_Stage_DefaultsToDevlopment()
    {
        var options = GetEnvironmentOptions();

#if DEBUG

        Assert.Equal(Stage.Development, options.Stage);
#else

        Assert.Equal(Stage.Production, options.Stage);
#endif
    }

    [Fact]
    public void EnvironmentOptions_ContentRoot_IsNotEmpty()
    {
        var options = GetEnvironmentOptions();

        Assert.False(string.IsNullOrEmpty(options.ContentRoot));
    }

    [Fact]
    public void EnvironmentOptions_ServiceName_IsNotEmpty()
    {
        var options = GetEnvironmentOptions();

        Assert.False(string.IsNullOrEmpty(options.ServiceName));
    }

    [Fact]
    public void EnvironmentOptions_ServiceNamespace_IsNotEmpty()
    {
        var options = GetEnvironmentOptions();

        Assert.False(string.IsNullOrEmpty(options.ServiceNamespace));
    }

    [Fact]
    public void EnvironmentOptions_ServiceVersion_IsNotEmpty()
    {
        var options = GetEnvironmentOptions();

        Assert.False(string.IsNullOrEmpty(options.ServiceVersion));
    }

    [Fact]
    public void EnvironmentOptions_ServiceFullName_IsNotEmpty()
    {
        var options = GetEnvironmentOptions();

        Assert.False(string.IsNullOrEmpty(options.ServiceFullName));
    }

    [Fact]
    public void EnvironmentOptions_Stage_CanBeSetAndRead()
    {
        var options = GetEnvironmentOptions();

        options.Stage = Stage.Production;

        Assert.Equal(Stage.Production, options.Stage);
    }

    [Fact]
    public void EnvironmentOptions_IsDemo_CanBeSetAndRead()
    {
        var options = GetEnvironmentOptions();

        options.IsDemo = true;

        Assert.True(options.IsDemo);
    }

    [Fact]
    public void EnvironmentOptions_ContentRoot_CanBeSetAndRead()
    {
        var options = GetEnvironmentOptions();

        options.ContentRoot = "/custom/content";

        Assert.Equal("/custom/content", options.ContentRoot);
    }

    [Fact]
    public void EnvironmentOptions_LogLevel_CanBeSetAndRead()
    {
        var options = GetEnvironmentOptions();

        options.LogLevel = "Debug";

        Assert.Equal("Debug", options.LogLevel);
    }
}
