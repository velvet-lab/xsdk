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
