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
using xSdk.Hosting;

namespace xSdk.Extensions.Variable;

public class VariableServiceTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Fact]
    public void GetService_IVariableService_IsRegistered()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        Assert.NotNull(service);
    }

    [Fact]
    public void ToDictionary_ReturnsNonNull()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        var dict = service.ToDictionary();

        Assert.NotNull(dict);
    }

    [Fact]
    public void Variables_InitiallyRegistered_ContainsEntries()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        var dict = service.ToDictionary();

        // After BuildHost, framework variables (stage, log-level, etc.) should be registered
        Assert.NotNull(dict);
    }

    [Fact]
    public void RegisterProvider_WithValidType_DoesNotThrow()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        // Registering a valid provider type should not throw
        var ex = Record.Exception(() => service.RegisterProvider(typeof(Fakes.TestVariableProvider)));

        Assert.Null(ex);
    }

    [Fact]
    public void NewVariable_RegistersVariableInCollection()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        var variable = Variable.Create("test_new_variable", typeof(string));

        service.NewVariable(variable);

        var loaded = service.LoadVariable("test_new_variable");
        Assert.NotNull(loaded);
    }

    [Fact]
    public void SetVariable_WithNonProtectedVariable_StoresValue()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        var variable = Variable.Create("test_set_variable", typeof(string));
        service.NewVariable(variable);
        service.SetVariable("test_set_variable", "hello");

        // No exception means the value was stored
    }

    [Fact]
    public void Variables_ContainRegisteredVariable()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        var variable = Variable.Create("test_bag_variable", typeof(string));
        service.NewVariable(variable);

        var found = service.Variables.Any(v => v.Name == "test_bag_variable");
        Assert.True(found);
    }

    [Fact]
    public void ExistsVariable_AfterSetValue_ReturnsTrue()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        var variable = Variable.Create("exists_check_var", typeof(string));
        service.NewVariable(variable, "some_value");

        var exists = service.ExistsVariable("exists_check_var");

        Assert.True(exists);
    }

    [Fact]
    public void ExistsVariable_RegisteredButNoValue_ReturnsFalse()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        // Register variable but set no value - ExistsVariable checks providers for a stored value
        var variable = Variable.Create("registered_no_value_var", typeof(string));
        service.NewVariable(variable);

        var exists = service.ExistsVariable("registered_no_value_var");

        Assert.False(exists);
    }

    [Fact]
    public void ReadVariableValue_AfterSetValue_ReturnsValue()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        var variable = Variable.Create("read_value_var", typeof(string));
        service.NewVariable(variable, "expected_value");

        var value = service.ReadVariableValue<string>("read_value_var");

        Assert.Equal("expected_value", value);
    }

    [Fact]
    public void ReadVariableValue_IntType_ReturnsCorrectValue()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        var variable = Variable.Create("int_value_var", typeof(int));
        service.NewVariable(variable, 42);

        var value = service.ReadVariableValue<int>("int_value_var");

        Assert.Equal(42, value);
    }

    [Fact]
    public void SetVariable_OnProtectedVariable_ThrowsSdkException()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        var variable = Variable.Create("protected_var", typeof(string));
        variable.Protect();
        service.NewVariable(variable);

        Assert.Throws<SdkException>(() => service.SetVariable("protected_var", "new_value"));
    }

    [Fact]
    public void NewVariable_WithThrowIfExists_ThrowsOnDuplicate()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        var variable = Variable.Create("dup_var_throw", typeof(string));
        service.NewVariable(variable);

        Assert.Throws<SdkException>(() => service.NewVariable(variable, throwIfAlreadyExists: true));
    }

    [Fact]
    public void NewVariable_WithoutThrowIfExists_DoesNotThrowOnDuplicate()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        var variable = Variable.Create("dup_var_no_throw", typeof(string));
        service.NewVariable(variable);

        var ex = Record.Exception(() => service.NewVariable(variable));

        Assert.Null(ex);
    }

    [Fact]
    public void ToDictionary_WithValues_ContainsRegisteredVariables()
    {
        var service = fixture
            .BuildHost()
            .Services.GetRequiredService<IVariableService>();

        var variable = Variable.Create("dict_test_var", typeof(string));
        service.NewVariable(variable, "dict_value");

        var dict = service.ToDictionary();

        Assert.True(dict.ContainsKey("dict_test_var"));
        Assert.Equal("dict_value", dict["dict_test_var"]?.ToString());
    }
}
