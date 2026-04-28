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

using Microsoft.Extensions.Logging;
using xSdk.Hosting;

namespace xSdk.Extensions.Plugin;

public class PluginDescription : IPluginDescription
{
    internal static int DefaultOrder => 99999;

    protected ILogger Logger { get; } = LogManager.CreateLogger<PluginDescription>();

    protected internal virtual int Order { get; } = DefaultOrder;

    public string? Name { get; internal set; }

    public Version? Version { get; internal set; }

    public string? Description { get; internal set; }

    public string? ProductVersion { get; internal set; }

    public string? Tag { get; internal set; }

    public List<string> Tags { get; internal set; } = new List<string>();
}
