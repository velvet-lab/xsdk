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

//namespace xSdk.Extensions.Options;

//public class ServiceDescriptionTests
//{
//    [Fact]
//    public void Create_WithAllParameters_UsesProvidedValues()
//    {
//        // Parameter order: serviceName, serviceNamespace, serviceVersion
//        var description = ServiceDescription.Create("MyService", "my.namespace", "1.2.3");

//        Assert.Equal("MyService", description.ServiceName);
//        Assert.Equal("1.2.3", description.ServiceVersion);
//        Assert.Equal("my.namespace", description.ServiceNamespace);
//    }

//    [Fact]
//    public void Create_ServiceFullName_CombinesNamespaceAndName()
//    {
//        var description = ServiceDescription.Create("MyService", "my.namespace", "1.0.0");

//        Assert.Equal("my.namespace.MyService", description.ServiceFullName);
//    }

//    [Fact]
//    public void Create_WithNullName_UsesDefaultValue()
//    {
//        var description = ServiceDescription.Create(null, null, null);

//        Assert.Equal(EnvironmentOptions.Definitions.ServiceName.DefaultValue, description.ServiceName);
//    }

//    [Fact]
//    public void Create_WithNullVersion_UsesDefaultValue()
//    {
//        var description = ServiceDescription.Create(null, null, null);

//        Assert.Equal(EnvironmentOptions.Definitions.ServiceVersion.DefaultValue, description.ServiceVersion);
//    }

//    [Fact]
//    public void Create_WithNullNamespace_UsesDefaultValue()
//    {
//        var description = ServiceDescription.Create(null, null, null);

//        Assert.Equal(EnvironmentOptions.Definitions.ServiceNamespace.DefaultValue, description.ServiceNamespace);
//    }

//    [Fact]
//    public void Create_Overload_WithNameOnly_UsesDefaultVersionAndNamespace()
//    {
//        var description = ServiceDescription.Create("TestApp");

//        Assert.Equal("TestApp", description.ServiceName);
//        Assert.Equal(EnvironmentOptions.Definitions.ServiceVersion.DefaultValue, description.ServiceVersion);
//        Assert.Equal(EnvironmentOptions.Definitions.ServiceNamespace.DefaultValue, description.ServiceNamespace);
//    }

//    [Fact]
//    public void Create_Overload_WithNameAndVersion_UsesDefaultNamespace()
//    {
//        // 2-param overload is Create(serviceName, serviceVersion)
//        var description = ServiceDescription.Create("TestApp", "2.0.0");

//        Assert.Equal("TestApp", description.ServiceName);
//        Assert.Equal("2.0.0", description.ServiceVersion);
//        Assert.Equal(EnvironmentOptions.Definitions.ServiceNamespace.DefaultValue, description.ServiceNamespace);
//    }

//    [Fact]
//    public void Create_NoArgs_ReturnsNonNullDescription()
//    {
//        var description = ServiceDescription.Create();

//        Assert.NotNull(description);
//    }

//    [Fact]
//    public void ServiceFullName_WithEmptyNamespace_ContainsServiceName()
//    {
//        // 3-param order: serviceName, serviceNamespace, serviceVersion
//        var description = ServiceDescription.Create("MyService", "", "1.0.0");

//        Assert.Contains("MyService", description.ServiceFullName);
//    }
//}
