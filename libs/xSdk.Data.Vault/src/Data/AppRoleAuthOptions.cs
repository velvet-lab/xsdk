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

[VariablePrefix("vault-app-role-auth")]
public class AppRoleAuthOptions : VariableSetup
{
    [Variable(
        name: Definitions.RoleId.Name,
        template: Definitions.RoleId.Template,
        helpText: Definitions.RoleId.HelpText,
        hidden: true)
    ]
    public string? RoleId
    {
        get => ReadValue<string>(Definitions.RoleId.Name);
        set => SetValue(Definitions.RoleId.Name, value);
    }

    [Variable(
        name: Definitions.Secret.Name,
        template: Definitions.Secret.Template,
        helpText: Definitions.Secret.HelpText,
        hidden: true)]
    public string? Secret
    {
        get => ReadValue<string>(Definitions.Secret.Name);
        set => SetValue(Definitions.Secret.Name, value);
    }

    private static class Definitions
    {
        public static class RoleId
        {
            public const string Name = nameof(RoleId);
            public const string Template = $"--role-id <role>";
            public const string HelpText = "RoleId for approle based auth to access vault";
        }

        public static class Secret
        {
            public const string Name = nameof(Secret);
            public const string Template = $"--role-secret <secret>";
            public const string HelpText = "Secret for approle based auth to access vault";
        }
    }
}
