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

namespace xSdk.Extensions.Options;

internal sealed class ServiceDescription
{
    private const char NamespaceSeperator = '.';

    private string _serviceNamespace;
    private string _serviceName;
    private string _serviceVersion;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private ServiceDescription()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
    }

    internal string ServiceFullName => $"{ServiceNamespace}{NamespaceSeperator}{ServiceName}".Trim();

    internal string ServiceName
    {
        get
        {
            if (string.IsNullOrEmpty(_serviceName))
            {
                AssemblyName? assemblyName = Assembly.GetEntryAssembly()?.GetName();

                _serviceName = assemblyName?.Name ?? EnvironmentOptions.Definitions.ServiceName.DefaultValue;
            }

            return _serviceName;
        }

        private set => _serviceName = value;
    }

    internal string ServiceVersion
    {
        get
        {
            if (string.IsNullOrEmpty(_serviceVersion))
            {
                AssemblyName? assemblyName = Assembly.GetEntryAssembly()?.GetName();

                _serviceVersion = assemblyName?.Version?.ToString() ?? EnvironmentOptions.Definitions.ServiceVersion.DefaultValue;
            }

            return _serviceVersion;
        }

        private set => _serviceVersion = value;
    }

    internal string ServiceNamespace
    {
        get
        {
            if (string.IsNullOrEmpty(_serviceNamespace))
            {
                _serviceNamespace = ReadServiceNamespace() ?? EnvironmentOptions.Definitions.ServiceNamespace.DefaultValue;
                if (_serviceNamespace.EndsWith(NamespaceSeperator))
                {
                    _serviceNamespace = _serviceNamespace.Substring(_serviceNamespace.Length - 1);
                }
            }

            return _serviceNamespace;
        }

        private set => _serviceNamespace = value;
    }

    //public static ServiceDescription Create()
    //    => Create(null, null, null);

    //public static ServiceDescription Create(string? serviceName)
    //    => Create(serviceName, null, null);

    //public static ServiceDescription Create(string? serviceName, string? serviceVersion)
    //    => Create(serviceName, null, serviceVersion);

    //public static ServiceDescription Create(string? serviceName, string? serviceNamespace, string? serviceVersion)
    //{
    //    return new ServiceDescription
    //    {
    //        ServiceName = serviceName ?? EnvironmentOptions.Definitions.ServiceName.DefaultValue,
    //        ServiceNamespace = serviceNamespace ?? EnvironmentOptions.Definitions.ServiceNamespace.DefaultValue,
    //        ServiceVersion = serviceVersion ?? EnvironmentOptions.Definitions.ServiceVersion.DefaultValue
    //    };
    //}

    internal static ServiceDescription Create(ApplicationOptions options)
    {
        return new ServiceDescription
        {
            ServiceName = options.Name ?? EnvironmentOptions.Definitions.ServiceName.DefaultValue,
            ServiceNamespace = options.Company ?? EnvironmentOptions.Definitions.ServiceNamespace.DefaultValue,
            ServiceVersion = options.AppVersion ?? EnvironmentOptions.Definitions.ServiceVersion.DefaultValue
        };
    }

    private static string? ReadServiceNamespace()
    {
        var assembly = Assembly.GetEntryAssembly();
        if (assembly != null)
        {
            Type[] types = assembly.GetExportedTypes();
            if (types != null && types.Length != 0)
            {
                return types.Select(x => x.Namespace).Where(x => x != null).OrderBy(x => x?.Length).FirstOrDefault();
            }
        }

        return null;
    }
}
