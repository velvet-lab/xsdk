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

using System.Text.Json.Serialization;
using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Data;

[VariablePrefix("Vault")]
public class VaultOptions : VariableSetup
{
    [Variable(
        name: Definitions.Endpoint.Name,
        template: Definitions.Endpoint.Template,
        helpText: Definitions.Endpoint.HelpText,
        hidden: true
    )]
    [JsonPropertyName(Definitions.Endpoint.Name)]
    public string? Endpoint
    {
        get => ReadValue<string>(Definitions.Endpoint.Name);
        set => SetValue(Definitions.Endpoint.Name, value);
    }

    private static class Definitions
    {
        public static class Endpoint
        {
            public const string Name = "endpoint";
            public const string Template = $"--vault-endpoint <endpoint>";
            public const string HelpText = "Endpoint where hashicorp vault lives";
        }
    }
}
