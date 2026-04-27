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

using xSdk.Hosting;

namespace xSdk.Plugins.DataProtection;

public class DataProtectionOptionsTests(WebHostTestFixture fixture) : IClassFixture<WebHostTestFixture>
{
    [Fact]
    public void DataProtectionSetup_DefaultProperties_AreEmpty()
    {
        var setup = new DataProtectionOptions();

        Assert.NotNull(setup);
        Assert.True(string.IsNullOrEmpty(setup.Discriminator));
        Assert.True(string.IsNullOrEmpty(setup.KeyLifetime));
    }

    [Fact]
    public void DataProtectionSetup_SetApplicationDiscriminator_StoresValue()
    {
        var setup = new DataProtectionOptions();

        setup.Discriminator = "my-discriminator";

        Assert.Equal("my-discriminator", setup.Discriminator);
    }    

    [Fact]
    public void DataProtectionSetup_SetKeyLifetime_StoresValue()
    {
        var setup = new DataProtectionOptions();

        setup.KeyLifetime = "30d";

        Assert.Equal("30d", setup.KeyLifetime);
    }

    [Fact]
    public void DataProtectionSetup_Definitions_ApplicationDiscriminatorName_IsCorrect()
    {
        Assert.Equal("discriminator", DataProtectionOptions.Definitions.Discriminator.Name);
    }

    [Fact]
    public void DataProtectionSetup_Definitions_KeyLifetimeName_IsCorrect()
    {
        Assert.Equal("lifetime", DataProtectionOptions.Definitions.KeyLifetime.Name);
    }
}
