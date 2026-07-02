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

using xSdk.Extensions.Variable.Attributes;
using xSdk.Tools;

namespace xSdk.Extensions.Variable;

public class Variable : IVariable
{
    private readonly string _applicationPrefix;

    private readonly string _name;

    protected Variable(string name, Type valueType)
    {
        _name = name;
        ArgumentNullException.ThrowIfNull(valueType);

        ValueType = valueType;

        _applicationPrefix = "none";// SlimHost.Instance.AppPrefix;
    }

    /// <summary>
    /// Returns the qualified variable name. When a prefix is set and the raw name
    /// does not already start with it, the prefix is prepended: e.g. "flat-file-file-path".
    /// </summary>
    public string Name =>
        !NoPrefix && !string.IsNullOrEmpty(Prefix) && !_name.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase)
            ? $"{Prefix}-{_name}"
            : _name;

    /// <summary>The raw (unqualified) variable name as passed to the constructor.</summary>
    internal string RawName => _name;

    public Type ValueType { get; private set; }

    public string? Prefix { get; internal set; }

    public bool NoPrefix { get; internal set; }

    public string? Template
    {
        get => CreateTemplate(field);
        internal set;
    }

    public string? HelpText { get; internal set; }

    public bool IsProtected { get; internal set; }

    public bool IsHidden { get; internal set; }

    internal Func<object?>? TelemetryResourceValue { get; set; }

    internal VariableAttribute? Attribute { get; set; }

    protected internal string KeyForSystem => CreateKey(false, true).Trim().ToUpperInvariant();

    protected internal string KeyForFile => $"{KeyForSystem}{Globals.Constants.PREFIX_SEPERATOR}file".Trim().ToUpperInvariant();

    protected internal string KeyForCommandline => CreateKey(true, false).Trim().ToLowerInvariant();

    public static Variable Create(string name, Type type) => Create(name, type, default);

    public static Variable Create(string name, Type type, Action<Variable>? configure)
    {
        var result = new Variable(name, type);

        configure?.Invoke(result);

        return result;
    }

    public static Variable<TType> Create<TType>(string name) => Create<TType>(name, default);

    public static Variable<TType> Create<TType>(string name, Action<Variable<TType>>? configure)
    {
        var result = new Variable<TType>(name);

        configure?.Invoke(result);

        return result;
    }

    internal virtual Variable SetDefaultValue(object defaultValue) => this;

    public override int GetHashCode() => ObjectTools.CreateHashCode(ToString());

    public override string ToString() => CreateKey(false, false);

    public override bool Equals(object? obj) =>
        ObjectTools.Equals<Variable>(this, obj, (source, dest) => string.CompareOrdinal(source.ToString(), dest.ToString()) == 0);

    internal string CreateKey(bool forCommandline, bool withApplicationPrefix)
    {
        string? variableName = Name;
        string? appPrefix = _applicationPrefix;
        string? prefix = Prefix;

        if (NoPrefix)
        {
            appPrefix = string.Empty;
            prefix = string.Empty;
        }

        string? result = $"{prefix}{Globals.Constants.PREFIX_SEPERATOR}";
        if (!string.IsNullOrEmpty(result) && result.StartsWith(Globals.Constants.PREFIX_SEPERATOR))
        {
            result = result.Substring(Globals.Constants.PREFIX_SEPERATOR.Length);
        }

        if (forCommandline)
        {
            result = "";
        }

        string? key = variableName;
        if (!string.IsNullOrEmpty(prefix) && !string.IsNullOrEmpty(key) && key.StartsWith(prefix))
        {
            key = key.Substring(prefix.Length).Trim();
            key = RemoveUnwantedCharsOnFirstPosition(key);
        }

        result = $"{result}{key}";
        if (forCommandline)
        {
            if (!string.IsNullOrEmpty(prefix))
            {
                result = $"{prefix}{Globals.Constants.VARIABLE_SEPERATOR}{result}";
            }

            result = $"--{result.Replace(Globals.Constants.VARIABLE_SEPERATOR, "-")}";
        }
        else
        {
            if (withApplicationPrefix && !string.IsNullOrEmpty(appPrefix))
            {
                result = $"{appPrefix}{Globals.Constants.PREFIX_SEPERATOR}{result}";
            }

            result = result.Replace("-", Globals.Constants.VARIABLE_SEPERATOR);
        }

        return result;
    }

    private string? CreateTemplate(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        string trimmed = value.Trim();

        // Split into command part (e.g. "--path") and optional arg placeholder (e.g. "<path>")
        string commandPart;
        string argPart = string.Empty;
        int spaceIdx = trimmed.IndexOf(' ');
        if (spaceIdx > -1)
        {
            commandPart = trimmed[..spaceIdx];
            argPart = $" {trimmed[(spaceIdx + 1)..].Trim()}";
        }
        else
        {
            commandPart = trimmed;
        }

        // Strip "--" to get the bare command name (e.g. "path")
        string commandName = commandPart.StartsWith("--") ? commandPart[2..] : commandPart;

        // Prepend the class-level prefix when present, so "--path" becomes "--flat-file-path"
        if (!NoPrefix && !string.IsNullOrEmpty(Prefix))
        {
            commandName = $"{Prefix}-{commandName}";
        }

        return $"--{commandName}{argPart}".Trim();
    }

    private static string RemoveUnwantedCharsOnFirstPosition(string name)
    {
        var tokens = new List<string> { "-", "." };
        tokens.ForEach(x =>
        {
            if (name.StartsWith(x))
            {
                name = name.Substring(1);
            }
        });
        return name;
    }
}

public sealed partial class Variable<TType> : Variable
{
    internal Variable(string name)
        : base(name, typeof(TType)) { }

    public TType? DefaultValue { get; private set; }

    internal override Variable SetDefaultValue(object defaultValue)
    {
        try
        {
            if (defaultValue.GetType() != ValueType)
            {
                throw new SdkException("Value Type is different from Variable Type");
            }

            DefaultValue = (TType)defaultValue;
        }
        catch
        {
            // Ignore Exception
        }

        return this;
    }
}
