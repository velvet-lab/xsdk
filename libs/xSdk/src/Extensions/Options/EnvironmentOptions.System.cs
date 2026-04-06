using System.Diagnostics;
using System.Runtime.InteropServices;
using xSdk.Extensions.Variable.Attributes;
using xSdk.Shared;

namespace xSdk.Extensions.Options;

public sealed partial class EnvironmentOptions
{
    [Variable(
        name: Definitions.MachineName.Name,
        helpText: Definitions.MachineName.HelpText,
        resourceNames: new[] { "host.name" },
        protect: true,
        hidden: true
    )]
    public string MachineName { get; private set; }

    [Variable(name: Definitions.Arch.Name, helpText: Definitions.Arch.HelpText, resourceNames: new[] { "host.arch" }, protect: true, hidden: true)]
    public string Arch { get; private set; }

    [Variable(name: Definitions.IPv4.Name, helpText: Definitions.IPv4.HelpText, resourceNames: new[] { "host.ip" }, protect: true, hidden: true)]
    public string IPv4 { get; private set; }

    [Variable(name: Definitions.Mac.Name, helpText: Definitions.Mac.HelpText, resourceNames: new[] { "host.mac" }, protect: true, hidden: true)]
    public string Mac { get; private set; }

    [Variable(
        name: Definitions.OsDescription.Name,
        helpText: Definitions.OsDescription.HelpText,
        resourceNames: new[] { "os.description" },
        protect: true,
        hidden: true
    )]
    public string OsDescription { get; private set; }

    [Variable(name: Definitions.OsName.Name, helpText: Definitions.OsName.HelpText, resourceNames: new[] { "os.name" }, protect: true, hidden: true)]
    public string OsName { get; private set; }

    [Variable(name: Definitions.OsType.Name, helpText: Definitions.OsType.HelpText, resourceNames: new[] { "os.type" }, protect: true, hidden: true)]
    public string OsType { get; private set; }

    [Variable(
        name: Definitions.OsVersion.Name,
        helpText: Definitions.OsVersion.HelpText,
        resourceNames: new[] { "os.version" },
        protect: true,
        hidden: true
    )]
    public string OsVersion { get; private set; }

    [Variable(
        name: Definitions.FrameworkName.Name,
        helpText: Definitions.FrameworkName.HelpText,
        resourceNames: new[] { "process.runtime.name" },
        protect: true,
        hidden: true
    )]
    public string FrameworkName { get; private set; }

    [Variable(
        name: Definitions.FrameworkVersion.Name,
        helpText: Definitions.FrameworkVersion.HelpText,
        resourceNames: new[] { "process.runtime.version" },
        protect: true,
        hidden: true
    )]
    public Version FrameworkVersion { get; private set; }

    [Variable(
        name: Definitions.FrameworkDescription.Name,
        helpText: Definitions.FrameworkDescription.HelpText,
        resourceNames: new[] { "process.runtime.description" },
        protect: true,
        hidden: true
    )]
    public string FrameworkDescription { get; private set; }

    [Variable(
        name: Definitions.Commandline.Name,
        helpText: Definitions.Commandline.HelpText,
        resourceNames: new[] { "process.command_line" },
        protect: true,
        hidden: true
    )]
    public string Commandline { get; private set; }

    [Variable(name: Definitions.Owner.Name, helpText: Definitions.Owner.HelpText, resourceNames: new[] { "process.owner" }, protect: true, hidden: true)]
    public string Owner { get; private set; }

    [Variable(name: Definitions.Pid.Name, helpText: Definitions.Pid.HelpText, resourceNames: new[] { "process.pid" }, protect: true, hidden: true)]
    public int Pid { get; private set; }

    public bool IsDotNetRunningInContainer { get; private set; }

    private void InitializeSystem()
    {
        // see https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-environment-variables
        if (!EnvironmentTools.TryReadEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER", out string isRunningInContainer))
        {
            if (EnvironmentTools.TryReadEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINERS", out isRunningInContainer))
            {
                IsDotNetRunningInContainer = Convert.ToBoolean(isRunningInContainer);
            }
        }
        else
        {
            IsDotNetRunningInContainer = Convert.ToBoolean(isRunningInContainer);
        }

        MachineName = Environment.MachineName;
        Arch = DetectProcessArchitecture();
        Mac = NetworkTools.GetMacAddress();
        IPv4 = NetworkTools.GetLocalIPAddress();

        var (osType, osName) = DetectOsType();
        OsDescription = RuntimeInformation.OSDescription;
        OsName = osName;
        OsType = osType;
        OsVersion = Environment.OSVersion.Version.ToString();

        FrameworkName = ".NET";
        FrameworkVersion = Environment.Version;
        FrameworkDescription = RuntimeInformation.FrameworkDescription;

        Commandline = Environment.CommandLine;
        Owner = Environment.UserName;
        Pid = Process.GetCurrentProcess().Id;
    }

    private static string DetectProcessArchitecture()
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

    private static (string, string) DetectOsType()
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

    private static partial class Definitions
    {
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
