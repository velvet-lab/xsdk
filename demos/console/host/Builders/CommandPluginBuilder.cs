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

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Spectre.Console.Cli;
using xSdk.Extensions.Commands;
using xSdk.Extensions.Options;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Demos.Builders;

internal class CommandPluginBuilder(IOptions<ApplicationOptions> options) : PluginBuilder, ICommandsPluginBuilder
{
    public Func<string> PromptFactory => throw new NotImplementedException();

    public void Configure(IConfigurator setup)
    {
        setup
            .SetApplicationName(options.Value.Name)
            // Tells the command line application to validate all examples before running the application.
            .ValidateExamples();
    }

    public ICommandApp CreateApplication(IServiceCollection? services = null) => throw new NotImplementedException();
}
