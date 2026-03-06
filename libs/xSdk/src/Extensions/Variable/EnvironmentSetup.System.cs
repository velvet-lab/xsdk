using xSdk.Extensions.Variable.Attributes;
using xSdk.Shared;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace xSdk.Extensions.Variable
{
    public sealed partial class EnvironmentSetup
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
            Arch = this.DetectProcessArchitecture();
            Mac = NetworkTools.GetMacAddress();
            IPv4 = NetworkTools.GetLocalIPAddress();

            var (osType, osName) = this.DetectOsType();
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
    }
}
