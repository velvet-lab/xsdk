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

using Microsoft.Extensions.Hosting;
using xSdk.Extensions.Options;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public static class SlimHostExtensions
{
    extension(SlimHost slimHost)
    {
        public void ConfigurePluginHost(Action<IPluginHost> factory) => slimHost.ConfigurePluginHost(factory);

        public EnvironmentOptions GetEnvironment()
        {
            EnvironmentOptions? options = slimHost.BuildEnvironmentOptions();
            if (options != null)
            {
                return options;
            }

            throw new SdkException("Failed to build environment options.");
        }

        public IEnumerable<TPluginHost> GetPluginHosts<TPluginHost>()
            where TPluginHost : IPluginHost => slimHost.GetPluginHosts<TPluginHost>();
    }
}
