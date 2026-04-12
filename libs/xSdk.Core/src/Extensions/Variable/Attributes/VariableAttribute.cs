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

using xSdk.Data.Annotations;

namespace xSdk.Extensions.Variable.Attributes;

/// <summary>
/// Default primaryKey attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class VariableAttribute : DataAnnotationAttribute
{
    /// <summary>
    /// Initializes a new instance of the DefaultAttribute class.
    /// </summary>
    /// <param name="value">The default primaryKey.</param>
    public VariableAttribute(
        string name = default!,
        object defaultValue = default!,
        string template = default!,
        string prefix = default!,
        string helpText = default!,
        bool protect = false,
        bool hidden = false,
        string[] resourceNames = default!,
        bool noPrefix = false
    )
        : base(defaultValue)
    {
        Name = name;
        DefaultValue = defaultValue;
        Template = template;
        HelpText = helpText;
        Protect = protect;
        Prefix = prefix;
        Hidden = hidden;
        ResourceNames = resourceNames;
        NoPrefix = noPrefix;
    }

    public string Name { get; }

    public object DefaultValue { get; }

    public string Template { get; }

    public string Prefix { get; }

    public bool NoPrefix { get; }

    public string HelpText { get; }

    public bool Protect { get; }

    public bool CopySlimValue { get; }

    public bool Hidden { get; }

    public string[] ResourceNames { get; }

    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return false;
        }

        return true;
    }
}
