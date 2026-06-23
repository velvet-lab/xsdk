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

namespace xSdk.Extensions.Options;

public sealed class ApplicationOptions
{
    public ApplicationOptions()
    {
        Version? assemblyVersion = GetType().Assembly.GetName().Version;
        if (assemblyVersion != null)
        {
            var version = new SemVer(assemblyVersion.ToString());
            Version = version;
            AppVersion = version.ToString();
        }
        else
        {
            throw new SdkException("Could not determine the application version from the assembly");
        }
    }

    [Variable(
        name: Definitions.AppName.Name,
        helpText: Definitions.AppName.HelpText,
        defaultValue: Definitions.AppName.DefaultValue,
        resourceNames: ["app.name"],
        hidden: true
    )]
    public string? Name { get; set; }

    [Variable(
        name: Definitions.AppDescription.Name,
        helpText: Definitions.AppDescription.HelpText,
        resourceNames: ["app.description"],
        hidden: true
    )]
    public string? Description { get; set; }

    [Variable(
        name: Definitions.AppCompany.Name,
        helpText: Definitions.AppCompany.HelpText,
        defaultValue: Definitions.AppCompany.DefaultValue,
        resourceNames: ["app.company"],
        hidden: true
    )]
    public string? Company { get; set; }

    [Variable(name: Definitions.AppVersion.Name, helpText: Definitions.AppVersion.HelpText, resourceNames: ["app.version"], hidden: true)]
    public string? AppVersion { get; set; }

    [Variable(
        name: Definitions.AppPrefix.Name,
        helpText: Definitions.AppPrefix.HelpText,
        defaultValue: Definitions.AppPrefix.DefaultValue,
        resourceNames: ["app.prefix"],
        hidden: true
    )]
    public string? Prefix { get; set; }

    public SemVer Version { get; private set; }

    internal static partial class Definitions
    {
        public static class AppName
        {
            public const string Name = nameof(AppName);
            public const string HelpText = "Short name of the application";
            public const string DefaultValue = "xsdk";
        }

        public static class AppDescription
        {
            public const string Name = nameof(AppDescription);
            public const string HelpText = "Description of the application";
        }

        public static class AppCompany
        {
            public const string Name = nameof(AppCompany);
            public const string HelpText = "Company name of the application";
            public const string DefaultValue = "xcom";
        }

        public static class AppPrefix
        {
            public const string Name = nameof(AppPrefix);
            public const string HelpText = "Prefix for the application";
            public const string DefaultValue = "XSDK";
        }

        public static class AppVersion
        {
            public const string Name = nameof(AppVersion);
            public const string HelpText = "Version of the application";
        }
    }
}
