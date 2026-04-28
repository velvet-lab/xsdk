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

using xSdk.Extensions.Variable;

namespace xSdk.Extensions.Variable.Providers;

public class VariableProviderTests
{
    private class TestVariable : IVariable
    {
        public bool IsHidden { get; set; }
        public bool IsProtected { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Prefix { get; set; } = string.Empty;
        public bool NoPrefix { get; set; }
        public string Template { get; set; } = string.Empty;
        public Type ValueType { get; set; } = typeof(string);
        public string HelpText { get; set; } = string.Empty;
    }

    private class DictionaryVariableProvider : VariableProvider
    {
        private readonly Dictionary<string, object?> _data;

        public DictionaryVariableProvider(Dictionary<string, object?> data)
        {
            _data = data;
        }

        protected override bool ExistsVariable(IVariable variable) => _data.ContainsKey(variable.Name);

        protected override object? ReadVariable(IVariable variable) =>
            _data.TryGetValue(variable.Name, out var value) ? value : null;
    }

    [Fact]
    public void ContainsVariable_ExistingVariable_ReturnsTrue()
    {
        var data = new Dictionary<string, object?> { ["key"] = "value" };
        var provider = new DictionaryVariableProvider(data);
        var variable = new TestVariable { Name = "key", ValueType = typeof(string) };

        Assert.True(provider.ContainsVariable(variable));
    }

    [Fact]
    public void ContainsVariable_NonExistingVariable_ReturnsFalse()
    {
        var data = new Dictionary<string, object?>();
        var provider = new DictionaryVariableProvider(data);
        var variable = new TestVariable { Name = "missing", ValueType = typeof(string) };

        Assert.False(provider.ContainsVariable(variable));
    }

    [Fact]
    public void TryReadVariable_ExistingStringValue_ReturnsTrueAndValue()
    {
        var data = new Dictionary<string, object?> { ["host"] = "localhost" };
        var provider = new DictionaryVariableProvider(data);
        var variable = new TestVariable { Name = "host", ValueType = typeof(string) };

        var found = provider.TryReadVariable<string>(variable, out var value);

        Assert.True(found);
        Assert.Equal("localhost", value);
    }

    [Fact]
    public void TryReadVariable_IntValue_ReturnsTrueAndConverted()
    {
        var data = new Dictionary<string, object?> { ["port"] = "8080" };
        var provider = new DictionaryVariableProvider(data);
        var variable = new TestVariable { Name = "port", ValueType = typeof(int) };

        var found = provider.TryReadVariable<int>(variable, out var value);

        Assert.True(found);
        Assert.Equal(8080, value);
    }

    [Fact]
    public void TryReadVariable_NonExistingVariable_ReturnsFalse()
    {
        var data = new Dictionary<string, object?>();
        var provider = new DictionaryVariableProvider(data);
        var variable = new TestVariable { Name = "missing", ValueType = typeof(string) };

        var found = provider.TryReadVariable<string>(variable, out var value);

        Assert.False(found);
        Assert.Null(value);
    }

    [Fact]
    public void TryReadVariable_NullValue_ReturnsFalse()
    {
        var data = new Dictionary<string, object?> { ["nullable"] = null };
        var provider = new DictionaryVariableProvider(data);
        var variable = new TestVariable { Name = "nullable", ValueType = typeof(string) };

        var found = provider.TryReadVariable<string>(variable, out var value);

        Assert.False(found);
    }

    [Fact]
    public void TryReadVariable_EnumValue_ReturnsTrueAndParsed()
    {
        var data = new Dictionary<string, object?> { ["day"] = "Monday" };
        var provider = new DictionaryVariableProvider(data);
        var variable = new TestVariable { Name = "day", ValueType = typeof(DayOfWeek) };

        var found = provider.TryReadVariable<DayOfWeek>(variable, out var value);

        Assert.True(found);
        Assert.Equal(DayOfWeek.Monday, value);
    }

    [Fact]
    public void TryReadVariable_InvalidEnumValue_ReturnsFalse()
    {
        var data = new Dictionary<string, object?> { ["day"] = "NotADay" };
        var provider = new DictionaryVariableProvider(data);
        var variable = new TestVariable { Name = "day", ValueType = typeof(DayOfWeek) };

        var found = provider.TryReadVariable<DayOfWeek>(variable, out var value);

        Assert.False(found);
    }
}
