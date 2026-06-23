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

using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.Documentation;

[VariablePrefix("openapi")]
public sealed class DocumentationPluginOptions : PluginOptions
{
    [Variable(
        name: Definitions.DocumentPattern.Name,
        template: Definitions.DocumentPattern.Template,
        helpText: Definitions.DocumentPattern.HelpText,
        defaultValue: Definitions.DocumentPattern.DefaultValue
    )]
    public string? DocumentPattern
    {
        get => ReadValue<string>(Definitions.DocumentPattern.Name);
        set => SetValue(Definitions.DocumentPattern.Name, value);
    }

    [Variable(
        name: Definitions.Enabled.Name,
        template: Definitions.Enabled.Template,
        helpText: Definitions.Enabled.HelpText
    )]
    public bool Enabled
    {
        get => ReadValue<bool>(Definitions.Enabled.Name);
        set => SetValue(Definitions.Enabled.Name, value);
    }

    public static class Definitions
    {
        public static class Enabled
        {
            public const string Name = nameof(Enabled);
            public const string Template = "--enabled";
            public const string HelpText = "Enabled OpenAPI document generation and UI";
        }

        public static class DocumentPattern
        {
            public const string Name = nameof(DocumentPattern);
            public const string Template = "--document-pattern <pattern>";
            public const string HelpText = "DocumentPattern prefix for the api";
            public const string DefaultValue = "openapi/{documentName}.json";
        }
    }
}
