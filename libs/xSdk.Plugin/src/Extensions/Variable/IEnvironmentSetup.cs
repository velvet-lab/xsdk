namespace xSdk.Extensions.Variable
{
    public interface IEnvironmentSetup : ISetup
    {
        string AppCompany { get; set; }
        string AppDescription { get; set; }
        string AppName { get; set; }
        string AppPrefix { get; set; }
        string AppVersion { get; set; }
        string Arch { get; }
        string Commandline { get; }
        string ContentRoot { get; set; }
        string FrameworkDescription { get; }
        string FrameworkName { get; }
        Version FrameworkVersion { get; }
        string IPv4 { get; }
        bool IsDemo { get; }
        bool IsDotNetRunningInContainer { get; }
        string LogLevel { get; set; }
        string Mac { get; }
        string MachineName { get; }
        string OsDescription { get; }
        string OsName { get; }
        string OsType { get; }
        string OsVersion { get; }
        string Owner { get; }
        int Pid { get; }
        string ServiceFullName { get; }
        string ServiceName { get; set; }
        string ServiceNamespace { get; set; }
        string ServiceVersion { get; set; }
        Stage Stage { get; set; }
        SemVer Version { get; }
    }
}
