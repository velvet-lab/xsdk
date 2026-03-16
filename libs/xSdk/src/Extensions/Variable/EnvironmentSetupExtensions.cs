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

using System.Diagnostics;
using System.Runtime.InteropServices;
using xSdk.Extensions.Commands;
using xSdk.Extensions.IO;
using xSdk.Shared;

namespace xSdk.Extensions.Variable;

public static class EnvironmentSetupExtensions
{
    internal static SemVer GetVersion(Type requestedType) => new SemVer(requestedType.Assembly.GetName().Version.ToString());

    internal static string DetectProcessArchitecture(this EnvironmentSetup setup)
    {
        if (
            RuntimeInformation.ProcessArchitecture == Architecture.Arm
            || RuntimeInformation.ProcessArchitecture == Architecture.Armv6
            || RuntimeInformation.ProcessArchitecture == Architecture.Arm64
        )
        {
            if (Environment.Is64BitProcess)
            {
                return "arm64";
            }
            return "arm32";
        }
        else if (RuntimeInformation.ProcessArchitecture == Architecture.Ppc64le)
        {
            if (Environment.Is64BitProcess)
            {
                return "ppc64";
            }
            return "ppc32";
        }
        else if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
        {
            return "amd64";
        }
        else if (RuntimeInformation.ProcessArchitecture == Architecture.X86)
        {
            return "x86";
        }
        else if (RuntimeInformation.ProcessArchitecture == Architecture.S390x)
        {
            return "s390x";
        }

        throw new SdkException("Could not determine current host architecture");
    }

    internal static (string, string) DetectOsType(this EnvironmentSetup setup)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
        {
            return ("freebsd", "Unix");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return ("linux", "Unix");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return ("darwin", "iOS");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return ("windows", "Windows");
        }

        throw new SdkException("Could not determine current os platform");
    }

    internal static TValue? ReadCommandlineValue<TValue>(this EnvironmentSetup setup, string pattern)
    {
        var result = string.Empty;

        var parser = CommandlineParser.Parse();
        if (parser.ContainsPattern(pattern))
        {
            result = parser.ReadPattern(pattern);
        }

        if (!string.IsNullOrEmpty(result))
        {
            return TypeConverter.ConvertTo<TValue>(result);
        }

        return default(TValue);
    }

    internal static string DetermineContentRoot(this EnvironmentSetup setup)
    {
        var contentRoot = setup.ReadCommandlineValue<string>(DefaultCommandSettings.Definitions.ContentRoot.Name);
        if (string.IsNullOrEmpty(contentRoot))
        {
            contentRoot = Environment.CurrentDirectory;

            if (Debugger.IsAttached)
            {
                contentRoot = FileSystemHelper.SearchGitRoot(contentRoot);
            }
        }

        return contentRoot;
    }
}
