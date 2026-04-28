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

using Microsoft.Extensions.Logging;
using xSdk.Hosting;

namespace xSdk.Extensions.Plugin;

public class PluginDescriptionTests
{
    private sealed class ConcretePluginDescription : PluginDescription
    {
        public ConcretePluginDescription()
        {
            Name = "TestPlugin";
            Version = new Version(1, 2, 3);
            Description = "A test plugin";
            ProductVersion = "1.2.3-rc.1";
            Tag = "test";
        }
    }

    [Fact]
    public void PluginDescription_Name_CanBeSetAndRead()
    {
        var desc = new ConcretePluginDescription();

        Assert.Equal("TestPlugin", desc.Name);
    }

    [Fact]
    public void PluginDescription_Version_CanBeSetAndRead()
    {
        var desc = new ConcretePluginDescription();

        Assert.Equal(new Version(1, 2, 3), desc.Version);
    }

    [Fact]
    public void PluginDescription_Description_CanBeSetAndRead()
    {
        var desc = new ConcretePluginDescription();

        Assert.Equal("A test plugin", desc.Description);
    }

    [Fact]
    public void PluginDescription_ProductVersion_CanBeSetAndRead()
    {
        var desc = new ConcretePluginDescription();

        Assert.Equal("1.2.3-rc.1", desc.ProductVersion);
    }

    [Fact]
    public void PluginDescription_Tag_CanBeSetAndRead()
    {
        var desc = new ConcretePluginDescription();

        Assert.Equal("test", desc.Tag);
    }

    [Fact]
    public void PluginDescription_Tags_IsEmptyByDefault()
    {
        var desc = new ConcretePluginDescription();

        Assert.NotNull(desc.Tags);
        Assert.Empty(desc.Tags);
    }

    [Fact]
    public void PluginDescription_DefaultOrder_IsLarge()
    {
        Assert.Equal(99999, PluginDescription.DefaultOrder);
    }
}
