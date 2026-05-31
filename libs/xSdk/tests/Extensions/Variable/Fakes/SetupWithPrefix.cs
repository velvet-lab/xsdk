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

namespace xSdk.Extensions.Variable.Fakes;

[VariablePrefix("my-plugin")]
internal class SetupWithPrefix : VariableSetup
{
    [Variable(
        name: Definitions.StringValue.Name,
        template: Definitions.StringValue.Template,
        helpText: Definitions.StringValue.HelpText,
        defaultValue: Definitions.StringValue.DefaultValue
    )]
    public string StringValue
    {
        get => ReadValue<string>(Definitions.StringValue.Name) ?? string.Empty;
        set => SetValue(Definitions.StringValue.Name, value);
    }

    [Variable(
        name: Definitions.BoolValue.Name,
        template: Definitions.BoolValue.Template,
        helpText: Definitions.BoolValue.HelpText,
        defaultValue: Definitions.BoolValue.DefaultValue
    )]
    public bool BoolValue
    {
        get => ReadValue<bool>(Definitions.BoolValue.Name);
        set => SetValue(Definitions.BoolValue.Name, value);
    }

    internal static class Definitions
    {
        internal static class StringValue
        {
            public const string Name = "string-value";
            public const string Template = "--value <val>";
            public const string HelpText = "A string value for testing.";
            public const string DefaultValue = "default-string";
        }

        internal static class BoolValue
        {
            public const string Name = "bool-value";
            public const string Template = "--bool-value";
            public const string HelpText = "A bool value for testing.";
            public const bool DefaultValue = true;
        }
    }
}
