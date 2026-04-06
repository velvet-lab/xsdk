///*
// * Copyright 2026 Roland Breitschaft
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//using xSdk.Hosting;

//namespace xSdk.Extensions.Variable;

//public class EnvironmentSetupTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
//{
//    [Fact]
//    public void EnvironmentSetup_AppName_CanBeSet()
//    {
//        var setup = new EnvironmentSetup();

//        setup.Name = "my-app";

//        Assert.Equal("my-app", setup.Name);
//    }

//    [Fact]
//    public void EnvironmentSetup_AppDescription_CanBeSet()
//    {
//        var setup = new EnvironmentSetup();

//        setup.Description = "My application description";

//        Assert.Equal("My application description", setup.Description);
//    }

//    [Fact]
//    public void EnvironmentSetup_AppCompany_CanBeSet()
//    {
//        var setup = new EnvironmentSetup();

//        setup.Company = "Acme Corp";

//        Assert.Equal("Acme Corp", setup.Company);
//    }

//    [Fact]
//    public void EnvironmentSetup_AppPrefix_CanBeSet()
//    {
//        var setup = new EnvironmentSetup();

//        setup.AppPrefix = "MYAPP";

//        Assert.Equal("MYAPP", setup.AppPrefix);
//    }

//    [Fact]
//    public void EnvironmentSetup_IsDemo_CanBeSet()
//    {
//        var setup = new EnvironmentSetup();

//        setup.IsDemo = true;

//        Assert.True(setup.IsDemo);
//    }

//    [Fact]
//    public void EnvironmentSetup_Stage_CanBeSet()
//    {
//        var setup = new EnvironmentSetup();

//        setup.Stage = Stage.Production;

//        Assert.Equal(Stage.Production, setup.Stage);
//    }

//    [Fact]
//    public void EnvironmentSetup_ContentRoot_CanBeSet()
//    {
//        var setup = new EnvironmentSetup();

//        setup.ContentRoot = "/app/content";

//        Assert.Equal("/app/content", setup.ContentRoot);
//    }

//    [Fact]
//    public void EnvironmentSetup_LogLevel_CanBeSet()
//    {
//        var setup = new EnvironmentSetup();

//        setup.LogLevel = "Debug";

//        Assert.Equal("Debug", setup.LogLevel);
//    }

//    [Fact]
//    public void EnvironmentSetup_Definitions_AppName_IsCorrect()
//    {
//        Assert.Equal("app-name", EnvironmentSetup.Definitions.Name.Name);
//        Assert.Equal("xsdk", EnvironmentSetup.Definitions.Name.DefaultValue);
//    }

//    [Fact]
//    public void EnvironmentSetup_Definitions_AppCompany_IsCorrect()
//    {
//        Assert.Equal("app-company", EnvironmentSetup.Definitions.Company.Name);
//        Assert.Equal("xcom", EnvironmentSetup.Definitions.Company.DefaultValue);
//    }

//    [Fact]
//    public void EnvironmentSetup_Definitions_AppPrefix_IsCorrect()
//    {
//        Assert.Equal("app-prefix", EnvironmentSetup.Definitions.AppPrefix.Name);
//        Assert.Equal("XSDK", EnvironmentSetup.Definitions.AppPrefix.DefaultValue);
//    }

//    [Fact]
//    public void EnvironmentSetup_Definitions_ServiceNamespace_HasDefaultValue()
//    {
//        Assert.Equal("xSdk", EnvironmentSetup.Definitions.ServiceNamespace.DefaultValue);
//    }

//    [Fact]
//    public void EnvironmentSetup_IsSlimMode_WhenUsedStandalone_IsTrue()
//    {
//        var setup = new EnvironmentSetup();
//        _ = setup.Name;

//        Assert.True(setup.IsSlimMode);
//    }
//}
