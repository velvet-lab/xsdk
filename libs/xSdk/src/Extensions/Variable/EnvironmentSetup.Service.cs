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

using System.Reflection;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.Variable;

public sealed partial class EnvironmentSetup
{
    [Variable(name: Definitions.ServiceName.Name, template: Definitions.ServiceName.Template, helpText: Definitions.ServiceName.HelpText)]
    public string ServiceName
    {
        get => this.ReadValue<string>(Definitions.ServiceName.Name);
        set => this.SetValue(Definitions.ServiceName.Name, value);
    }

    [Variable(name: Definitions.ServiceNamespace.Name, template: Definitions.ServiceNamespace.Template, helpText: Definitions.ServiceNamespace.HelpText)]
    public string ServiceNamespace
    {
        get => this.ReadValue<string>(Definitions.ServiceNamespace.Name);
        set => this.SetValue(Definitions.ServiceNamespace.Name, value);
    }

    [Variable(name: Definitions.ServiceVersion.Name, template: Definitions.ServiceVersion.Template, helpText: Definitions.ServiceVersion.HelpText)]
    public string ServiceVersion
    {
        get => this.ReadValue<string>(Definitions.ServiceVersion.Name);
        set => this.SetValue(Definitions.ServiceVersion.Name, value);
    }

    [Variable(name: Definitions.ServiceFullName.Name, helpText: Definitions.ServiceFullName.HelpText, protect: true, hidden: true)]
    public string ServiceFullName { get; private set; }

    private void InitializeService()
    {
        var currentServiceName = ServiceName;
        var currentServiceNamespace = ServiceNamespace;
        var currentServiceVersion = ServiceVersion;

        if (string.IsNullOrEmpty(currentServiceName) || string.IsNullOrEmpty(currentServiceNamespace) || string.IsNullOrEmpty(currentServiceVersion))
        {
            var assembly = Assembly.GetEntryAssembly();
            var assemblyName = assembly.GetName();

            if (string.IsNullOrEmpty(currentServiceName))
            {
                currentServiceName = assemblyName.Name;
            }

            if (string.IsNullOrEmpty(currentServiceNamespace))
            {
                currentServiceNamespace = ReadServiceNamespace(assembly);
                if (string.IsNullOrEmpty(currentServiceNamespace))
                {
                    currentServiceNamespace = Definitions.ServiceNamespace.DefaultValue;
                }
            }

            if (string.IsNullOrEmpty(currentServiceVersion))
            {
                currentServiceVersion = assemblyName.Version.ToString();
            }
        }

        if (!string.IsNullOrEmpty(currentServiceName) && !string.IsNullOrEmpty(currentServiceNamespace))
        {
            var seperator = ".";
            if (currentServiceNamespace.EndsWith("."))
            {
                currentServiceNamespace = currentServiceNamespace.Substring(currentServiceNamespace.Length - 1);
            }

            ServiceFullName = $"{currentServiceNamespace}{seperator}{currentServiceName}".Trim();
        }

        ServiceName = currentServiceName;
        ServiceNamespace = currentServiceNamespace;
        ServiceVersion = currentServiceVersion;
    }

    private static string? ReadServiceNamespace(Assembly assembly)
    {
        var types = assembly.GetExportedTypes();
        if (types != null && types.Any())
        {
            return types.Select(x => x.Namespace).Where(x => x != null).OrderBy(x => x.Length).FirstOrDefault();
        }
        else
        {
            return string.Empty;
        }
    }
}
