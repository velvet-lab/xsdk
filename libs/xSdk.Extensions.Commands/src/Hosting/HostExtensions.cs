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

using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using xSdk.Extensions.Commands;

namespace xSdk.Hosting;

[ExcludeFromCodeCoverage]
public static class HostExtensions
{
    extension(IHost host)
    {
        public int RunConsole(string[] args)
            => host.RunConsoleAsync(args).GetAwaiter().GetResult();

        public async Task<int> RunConsoleAsync(string[] args)
        {
            Guard.IsNotNull(host);

            await host.StartAsync();

            System.Console.Clear();
            AnsiConsole.Clear();

            IApplication app = host.Services.GetRequiredService<IApplication>();
            return await app.RunAsync(args);
        }
    }
}
