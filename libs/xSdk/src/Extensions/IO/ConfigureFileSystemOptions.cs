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

using Microsoft.Extensions.Options;
using xSdk.Extensions.Options;

namespace xSdk.Extensions.IO;

internal sealed class ConfigureFileSystemOptions(IOptions<ApplicationOptions> appOptions, IOptions<EnvironmentOptions> envOptions) : IConfigureOptions<FileSystemOptions>
{
    private readonly ApplicationOptions _appOptions = appOptions.Value;
    private readonly EnvironmentOptions _envOptions = envOptions.Value;

    public void Configure(FileSystemOptions options)
    {
        options.ApplicationName = _appOptions?.Name;
        options.Company = _appOptions?.Company;
        options.ContentRoot = _envOptions?.ContentRoot;
    }
}
