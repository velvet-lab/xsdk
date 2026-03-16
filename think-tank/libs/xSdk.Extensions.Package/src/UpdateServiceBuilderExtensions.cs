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

using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Package.Providers.GitHub;
using xSdk.Extensions.Package.Providers.Local;
using xSdk.Extensions.Package.Providers.Nuget;
using xSdk.Extensions.Variable;

namespace xSdk.Extensions.Package
{
    public static class UpdateServiceBuilderExtensions
    {
        public static IUpdateServiceBuilder ValidateOnStartup(this IUpdateServiceBuilder builder)
        {
            return builder;
        }

        public static IUpdateServiceBuilder UseLocal(this IUpdateServiceBuilder builder) => builder.UseLocal(setup => { });

        public static IUpdateServiceBuilder UseLocal(this IUpdateServiceBuilder builder, Action<LocalSetup> setup)
        {
            if (builder is UpdateServiceBuilder concreteBuilder)
            {
                concreteBuilder.AddProvider(provider =>
                {
                    var setupImpl = provider.GetRequiredService<IVariableService>().GetSetup<LocalSetup>(true, true);

                    setup?.Invoke(setupImpl);

                    return new LocalProvider(setupImpl);
                });
            }

            return builder;
        }

        public static IUpdateServiceBuilder UseNuget(this IUpdateServiceBuilder builder) => builder.UseLocal(setup => { });

        public static IUpdateServiceBuilder UseNuget(this IUpdateServiceBuilder builder, Action<NugetSetup> setup)
        {
            if (builder is UpdateServiceBuilder concreteBuilder)
            {
                concreteBuilder.AddProvider(provider =>
                {
                    var nugetSetup = new NugetSetup();
                    setup?.Invoke(nugetSetup);

                    return new NugetPackageProvider(nugetSetup);
                });
            }
            return builder;
        }

        public static IUpdateServiceBuilder UseGitHub(this IUpdateServiceBuilder builder, Action<GitHubSetup> setup)
        {
            if (builder is UpdateServiceBuilder concreteBuilder)
            {
                concreteBuilder.AddProvider(provider =>
                {
                    var githubSetup = new GitHubSetup();
                    setup?.Invoke(githubSetup);

                    return new GitHubProvider(githubSetup);
                });
            }
            return builder;
        }
    }
}
