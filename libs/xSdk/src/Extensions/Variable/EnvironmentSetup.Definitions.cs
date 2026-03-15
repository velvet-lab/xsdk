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

namespace xSdk.Extensions.Variable;

public sealed partial class EnvironmentSetup
{
    public static class Definitions
    {
        public const string CLI40 = "4.0";

        public const string CLI41 = "4.1";

        public const string CLI50 = "5.0";

        public static class AppName
        {
            public const string Name = "app-name";
            public const string Template = "--app-name <name>";
            public const string HelpText = "Short name of the application";
            public const string DefaultValue = "xsdk";
        }

        public static class AppDescription
        {
            public const string Name = "app-description";
            public const string Template = "--app-description <description>";
            public const string HelpText = "Description of the application";
        }

        public static class AppCompany
        {
            public const string Name = "app-company";
            public const string Template = "--app-company <company>";
            public const string HelpText = "Company name of the application";
            public const string DefaultValue = "xcom";
        }

        public static class AppPrefix
        {
            public const string Name = "app-prefix";
            public const string Template = "--app-prefix <prefix>";
            public const string HelpText = "Prefix for the application";
            public const string DefaultValue = "XSDK";
        }

        public static class AppVersion
        {
            public const string Name = "app-version";
            public const string Template = "--app-version <version>";
            public const string HelpText = "Version of the application";
        }

        public static class ServiceName
        {
            public const string Name = "service-name";
            public const string Template = "--service-name <name>";
            public const string HelpText = "Service name to identify the application in MaaS environments";
        }

        public static class ServiceNamespace
        {
            public const string Name = "service-namespace";
            public const string Template = "--service-namespace <namespace>";
            public const string HelpText = "Service namespace to identify the application in MaaS environments";
            public const string DefaultValue = "xSdk";
        }

        public static class ServiceVersion
        {
            public const string Name = "service-version";
            public const string Template = "--service-version <version>";
            public const string HelpText = "Service version to identify the application in MaaS environments";
        }

        public static class ServiceFullName
        {
            public const string Name = "service-fullname";
            public const string HelpText = "Fullname for the service identify the application in MaaS environments";
        }

        public static class MachineName
        {
            public const string Name = "machinename";
            public const string HelpText = "Machine name for the host";
        }

        public static class Arch
        {
            public const string Name = "arch";
            public const string HelpText = "Architecture of the host";
        }

        public static class Mac
        {
            public const string Name = "mac";
            public const string HelpText = "MAC Address of the host";
        }

        public static class IPv4
        {
            public const string Name = "ipv4";
            public const string HelpText = "IPv4 Address of the host";
        }

        public static class OsDescription
        {
            public const string Name = "osdescription";
            public const string HelpText = "Description of the Operating System";
        }

        public static class OsName
        {
            public const string Name = "osname";
            public const string HelpText = "Name of the Operating System";
        }

        public static class OsType
        {
            public const string Name = "ostype";
            public const string HelpText = "Type of the Operating System";
        }

        public static class OsVersion
        {
            public const string Name = "osversion";
            public const string HelpText = "Version of the Operating System";
        }

        public static class FrameworkName
        {
            public const string Name = "frameworkname";
            public const string HelpText = "Name of the .Net Framework";
        }

        public static class FrameworkVersion
        {
            public const string Name = "frameworkversion";
            public const string HelpText = "Version of the .Net Framework";
        }

        public static class FrameworkDescription
        {
            public const string Name = "frameworkdescription";
            public const string HelpText = "Description of the .Net Framework";
        }

        public static class Owner
        {
            public const string Name = "owner";
            public const string HelpText = "Process owner of the current running process";
        }

        public static class Commandline
        {
            public const string Name = "commandline";
            public const string HelpText = "Currently used commandline";
        }

        public static class Pid
        {
            public const string Name = "pid";
            public const string HelpText = "Process PrimaryKey of the current running process";
        }
    }
}
