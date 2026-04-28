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
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Data;

[VariablePrefix("Vault")]
public class VaultDatabaseOptions : VariableSetup
{
    protected override void OnInitialize()
    {
        AuthMethod = AuthMethods.None;
    }

    [
        Variable(
            name: Definitions.Endpoint.Name,
            template: Definitions.Endpoint.Template,
            helpText: Definitions.Endpoint.HelpText)
    ]
    public string? Endpoint
    {
        get => ReadValue<string>(Definitions.Endpoint.Name);
        set => SetValue(Definitions.Endpoint.Name, value);
    }

    [
        Variable(
            name: Definitions.BasePath.Name,
            template: Definitions.BasePath.Template,
            helpText: Definitions.BasePath.HelpText)
    ]
    public string? BasePath
    {
        get => ReadValue<string>(Definitions.BasePath.Name);
        set => SetValue(Definitions.BasePath.Name, value);
    }

    [
        Variable(
            name: Definitions.AuthMethod.Name,
            template: Definitions.AuthMethod.Template,
            helpText: Definitions.AuthMethod.HelpText)
    ]
    public AuthMethods AuthMethod
    {
        get => ReadValue<AuthMethods>(Definitions.AuthMethod.Name);
        set => SetValue(Definitions.AuthMethod.Name, value);
    }

    public Func<Stage, string, string?> PathFormatFactory { get; set; } = (stage, basePath) => $"{basePath}/{stage.ToString().ToLower()}";

    private static class Definitions
    {
        public static class Endpoint
        {
            public const string Name = "endpoint";
            public const string Template = $"--vault-endpoint <endpoint>";
            public const string HelpText = "Endpoint where vault lives";
        }

        public static class BasePath
        {
            public const string Name = "basePath";
            public const string Template = $"--vault-basePath <basePath>";
            public const string HelpText = "Base path where hashicorp vault lives";
        }

        public static class AuthMethod
        {
            public const string Name = "authMethod";
            public const string Template = $"--vault-authMethod <authMethod>";
            public const string HelpText = "Authentication method to access vault. Supported values are: None, AppRole, Jwt, Ldap, Oidc, UsernamePassword, Token and Cert.";
        }
    }
}
