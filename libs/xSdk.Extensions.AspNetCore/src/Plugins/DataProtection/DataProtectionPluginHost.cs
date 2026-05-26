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
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xSdk.Extensions.DataProtection;
using xSdk.Extensions.Options;
using xSdk.Hosting;
using xSdk.Tools;

namespace xSdk.Plugins.DataProtection;

[ExcludeFromCodeCoverage(Justification = "ASP.NET Core data-protection pipeline – requires a running host with filesystem/key-ring.")]
public sealed class DataProtectionPluginHost(IOptions<ApplicationOptions> applicationOptions, IOptions<DataProtectionPluginOptions> pluginOptions) : PluginHost
{
    public override void ConfigureServices(IServiceCollection services)
    {
        Logger.LogInformation("Configure DataProtection");

        var dataprotectionOptions = pluginOptions.Value;
        var appOptions = applicationOptions.Value;

        IDataProtectionBuilder? builder = null;
        if (!string.IsNullOrEmpty(dataprotectionOptions.Discriminator))
            builder = services.AddDataProtection(_ => _.ApplicationDiscriminator = dataprotectionOptions.Discriminator);
        else
            builder = services.AddDataProtection();

        if (!string.IsNullOrEmpty(appOptions.Name))
            builder.SetApplicationName(appOptions.Name);

        if (!string.IsNullOrEmpty(dataprotectionOptions.KeyLifetime))
        {
            if (TimeSpanParser.TryParse(dataprotectionOptions.KeyLifetime, out TimeSpan result))
            {
                builder.SetDefaultKeyLifetime(result);
            }
        }

        InvokeBuilder<IDataProtectionPluginBuilder>(x => x.ConfigureDataProtection(builder));
    }
}
