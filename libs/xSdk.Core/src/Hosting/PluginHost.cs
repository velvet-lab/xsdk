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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public abstract class PluginHost : PluginDescription, IPluginHost
{
    protected ILogger Logger => LogManager.GetCurrentClassLogger();

    protected IEnvironmentSetup Environment => this.LoadSetup<IEnvironmentSetup>();


    public virtual void ConfigureServices(IServiceCollection services) { }

    public virtual void ConfigureServices(HostBuilderContext context, IServiceCollection services) { }
    
    protected TSetup LoadSetup<TSetup>()
        where TSetup : class, ISetup
        => SetupLoader.Load<TSetup>();
}

public abstract class PluginHost<TSetup> : PluginHost, IPluginHost<TSetup>
    where TSetup : class, ISetup
{
    protected TSetup Setup => this.LoadSetup<TSetup>();
}
