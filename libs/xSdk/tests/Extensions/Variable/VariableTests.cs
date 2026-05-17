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

namespace xSdk.Extensions.Variable;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2263:Generische Überladung bevorzugen, wenn der Typ bekannt ist", Justification = "<Ausstehend>")]
public class VariableTests()
{
    private const string PREFIX = "MyPrefix";
    private const string NAME = "my_variable";

    private const string PREFIX_SEPERATOR = xSdk.Extensions.Variable.Globals.Constants.PREFIX_SEPERATOR;
    private const string SEPERATOR = xSdk.Extensions.Variable.Globals.Constants.VARIABLE_SEPERATOR;

    [Fact]
    public void CreateVariable()
    {
        string name = "my_variable";
        var variable = Variable.Create(name, typeof(string));

        Assert.NotNull(variable);
        Assert.Equal(name, variable.Name);
        Assert.Equal(typeof(string), variable.ValueType);
    }

    [Fact]
    public void CreateVariableWithPrefix()
    {
        var variable = Variable.Create(NAME, typeof(string));

        variable.SetPrefix(PREFIX);

        Assert.NotNull(variable);
        Assert.Equal(PREFIX.ToLower(), variable.Prefix);

        Assert.Equal($"--{PREFIX}{SEPERATOR}{NAME}".Replace(SEPERATOR, "-").ToLower(), variable.KeyForCommandline);
    }

    [Fact]
    public void CreateVariableWithDisabledPrefix()
    {
        var variable = Variable.Create(NAME, typeof(string));

        variable.SetPrefix(PREFIX).DisablePrefix();

        Assert.NotNull(variable);
        Assert.Equal(PREFIX.ToLower(), variable.Prefix);

        Assert.Equal($"{NAME}{PREFIX_SEPERATOR}FILE".ToUpper(), variable.KeyForFile);
        Assert.Equal($"--{NAME}".Replace(SEPERATOR, "-").ToLower(), variable.KeyForCommandline);
    }

    [Fact]
    public void Variable_GetHashCode_ReturnsConsistentValue()
    {
        var variable = Variable.Create(NAME, typeof(string));

        int hash1 = variable.GetHashCode();
        int hash2 = variable.GetHashCode();

        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void Variable_Equals_SameNameAndType_AreEqual()
    {
        var v1 = Variable.Create(NAME, typeof(string));
        var v2 = Variable.Create(NAME, typeof(string));

        Assert.True(v1.Equals(v2));
    }

    [Fact]
    public void Variable_Equals_DifferentName_AreNotEqual()
    {
        var v1 = Variable.Create("var_one", typeof(string));
        var v2 = Variable.Create("var_two", typeof(string));

        Assert.False(v1.Equals(v2));
    }

    [Fact]
    public void Variable_Equals_Null_ReturnsFalse()
    {
        var variable = Variable.Create(NAME, typeof(string));

        Assert.False(variable.Equals(null));
    }

    [Fact]
    public void Variable_ToString_ReturnsKey()
    {
        var variable = Variable.Create(NAME, typeof(string));

        string str = variable.ToString();

        Assert.NotNull(str);
        Assert.Contains(NAME, str, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Variable_KeyForSystem_ReturnsUpperCase()
    {
        var variable = Variable.Create(NAME, typeof(string));

        string key = variable.KeyForSystem;

        Assert.Equal(key, key.ToUpperInvariant());
    }

    [Fact]
    public void Variable_KeyForFile_ContainsFileMarker()
    {
        var variable = Variable.Create(NAME, typeof(string));

        string key = variable.KeyForFile;

        Assert.Contains("FILE", key, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Variable_Generic_Create_ReturnsTypedVariable()
    {
        var variable = Variable.Create<string>(NAME);

        Assert.NotNull(variable);
        Assert.Equal(typeof(string), variable.ValueType);
    }

    [Fact]
    public void Variable_Generic_Create_WithConfigure_AppliesConfiguration()
    {
        var variable = Variable.Create<string>(NAME, v => v.HelpText = "Help text");

        Assert.Equal("Help text", variable.HelpText);
    }

    [Fact]
    public void Variable_Create_WithNullValueType_ThrowsArgumentNullException() =>
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<ArgumentNullException>(() => Variable.Create(NAME, null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

}
