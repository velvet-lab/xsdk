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

public sealed partial class EnvironmentOptions
{
    [Variable(name: Definitions.ServiceName.Name, template: Definitions.ServiceName.Template, helpText: Definitions.ServiceName.HelpText)]
    public string ServiceName
    {
        get => ReadValue<string>(Definitions.ServiceName.Name);
        set => SetValue(Definitions.ServiceName.Name, value);
    }

    [Variable(name: Definitions.ServiceNamespace.Name, template: Definitions.ServiceNamespace.Template, helpText: Definitions.ServiceNamespace.HelpText)]
    public string ServiceNamespace
    {
        get => ReadValue<string>(Definitions.ServiceNamespace.Name);
        set => SetValue(Definitions.ServiceNamespace.Name, value);
    }

    [Variable(name: Definitions.ServiceVersion.Name, template: Definitions.ServiceVersion.Template, helpText: Definitions.ServiceVersion.HelpText)]
    public string ServiceVersion
    {
        get => ReadValue<string>(Definitions.ServiceVersion.Name);
        set => SetValue(Definitions.ServiceVersion.Name, value);
    }

    public string ServiceFullName { get; private set; }

    internal void InitializeService(ServiceDescription serviceDescription)
    {
        ServiceName = serviceDescription.ServiceName;
        ServiceNamespace = serviceDescription.ServiceNamespace;
        ServiceVersion = serviceDescription.ServiceVersion;
        ServiceFullName = serviceDescription.ServiceFullName;
    }

    public static partial class Definitions
    {
        internal static class ServiceName
        {
            public const string Name = nameof(ServiceName);
            public const string Template = "--service-name <name>";
            public const string HelpText = "Service name to identify the application in OpenTelemetry environments";
            public const string DefaultValue = "DefaultService";
        }

        internal static class ServiceNamespace
        {
            public const string Name = nameof(ServiceNamespace);
            public const string Template = "--service-namespace <namespace>";
            public const string HelpText = "Service namespace to identify the application in OpenTelemetry environments";
            public const string DefaultValue = "xSdk";
        }

        internal static class ServiceVersion
        {
            public const string Name = nameof(ServiceVersion);
            public const string Template = "--service-version <version>";
            public const string HelpText = "Service version to identify the application in OpenTelemetry environments";
            public const string DefaultValue = "stable";
        }
    }
}
