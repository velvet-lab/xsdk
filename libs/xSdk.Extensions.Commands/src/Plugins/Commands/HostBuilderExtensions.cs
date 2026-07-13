// * Copyright 2026 Roland Breitschaft
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Commands;
using xSdk.Hosting;

namespace xSdk.Plugins.Commands;

public static class HostBuilderExtensions
{
    extension(IHostBuilder builder)
    {
        public IHostBuilder EnableReplConsole()
                => builder.EnableReplConsole<ReplConsoleBuilder>(_ => { }, _ => _.DisableDefaultHelp = true);

        public IHostBuilder EnableReplConsole(Action<ReplConsoleBuilder> configure)
            => builder.EnableReplConsole<ReplConsoleBuilder>(configure, _ => _.DisableDefaultHelp = true);

        public IHostBuilder EnableReplConsole(Action<ReplConsoleBuilder> configure, Action<ConsoleOptions> optionsConfigure)
            => builder.EnableReplConsole<ReplConsoleBuilder>(configure, optionsConfigure);

        public IHostBuilder EnableReplConsole<TBuilder>()
            where TBuilder : ReplConsoleBuilder
            => builder
                .EnableReplConsole<TBuilder>(_ => { }, _ => _.DisableDefaultHelp = true);

        public IHostBuilder EnableReplConsole<TBuilder>(Action<TBuilder> configure, Action<ConsoleOptions> optionsConfigure)
            where TBuilder : ReplConsoleBuilder
            => builder
                .EnableConsole<TBuilder>(configure, optionsConfigure);

        public IHostBuilder EnableDefaultConsole()
                => builder.EnableDefaultConsole<ConsoleBuilder>(_ => { }, _ => { });

        public IHostBuilder EnableDefaultConsole(Action<ConsoleBuilder> configure)
            => builder.EnableDefaultConsole<ConsoleBuilder>(configure, _ => { });

        public IHostBuilder EnableDefaultConsole(Action<ConsoleBuilder> configure, Action<ConsoleOptions> optionsConfigure)
            => builder.EnableDefaultConsole<ConsoleBuilder>(configure, optionsConfigure);

        public IHostBuilder EnableDefaultConsole<TBuilder>()
            where TBuilder : ConsoleBuilder
            => builder.EnableDefaultConsole<TBuilder>(_ => { }, _ => { });

        public IHostBuilder EnableDefaultConsole<TBuilder>(Action<TBuilder> configure, Action<ConsoleOptions> optionsConfigure)
            where TBuilder : ConsoleBuilder
            => builder
                .EnableConsole<TBuilder>(configure, optionsConfigure);

        public IHostBuilder EnableConsole<TBuilder>()
            where TBuilder : ConsoleBuilder
            => builder.EnableConsole<TBuilder>(_ => { }, _ => { });

        public IHostBuilder EnableConsole<TBuilder>(Action<ConsoleOptions> configure)
            where TBuilder : ConsoleBuilder
            => builder.EnableConsole<TBuilder>(_ => { }, configure);

        private IHostBuilder EnableConsole<TBuilder>(Action<TBuilder> configure, Action<ConsoleOptions> optionsConfigure)
            where TBuilder : ConsoleBuilder
            => builder
                .RegisterPluginHost<PluginHost<TBuilder>>()
                .RegisterPluginHostOptions<ConsoleOptions>(optionsConfigure)
                .RegisterBuilder<TBuilder>(configure);
    }
}
