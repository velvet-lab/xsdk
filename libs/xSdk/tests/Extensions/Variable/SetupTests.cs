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

namespace xSdk.Extensions.Variable;

public class SetupTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void LoadSetup()
    {
        var service = fixture
            .ConfigureServices(services => services.AddVariableServices())
            .GetService<IVariableService>();

        var setup = service.GetSetup<EnvironmentSetup>();

        Assert.NotNull(setup);
    }

    [Fact]
    public void LoadSetupFromInterface()
    {
        var service = fixture
            .ConfigureServices(services => services.AddVariableServices())
            .GetService<IVariableService>();

        var setup = service.GetSetup<IEnvironmentSetup>();

        Assert.NotNull(setup);
    }

    [Fact]
    public void LoadSetupWithoutSlimHost()
    {
        var setup = new EnvironmentSetup();

        setup.AppCompany = "MyCompany";

        Assert.NotNull(setup);
        Assert.Equal("MyCompany", setup.AppCompany);
    }
}
