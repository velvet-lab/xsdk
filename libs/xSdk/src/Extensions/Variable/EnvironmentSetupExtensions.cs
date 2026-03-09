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
