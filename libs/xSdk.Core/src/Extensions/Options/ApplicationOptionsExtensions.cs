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

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Extensions.Options;

public static class ApplicationOptionsExtensions
{
    public static IServiceCollection RegisterApplicationOptions(this IServiceCollection services, ApplicationOptions options)
    {
        services
            .AddOptions<ApplicationOptions>()
            .Configure(appOptions =>
            {
                appOptions.Name = options.Name ?? ApplicationOptions.Definitions.AppName.DefaultValue;
                appOptions.Company = options.Company ?? ApplicationOptions.Definitions.AppCompany.DefaultValue;
                appOptions.Prefix = options.Prefix ?? ApplicationOptions.Definitions.AppPrefix.DefaultValue;

                appOptions.Description = options.Description;
                appOptions.AppVersion = options.AppVersion;

                var validator = new ApplicationOptionsValidator();
                validator.ValidateAndThrow(appOptions);
            });

        return services;
    }
}
