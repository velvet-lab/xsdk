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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xSdk.Extensions.Options;
using xSdk.Hosting;

namespace xSdk.Plugins.WebSecurity;

[ExcludeFromCodeCoverage(Justification = "ASP.NET Core request pipeline configuration – requires a running web host.")]
internal sealed class WebSecurityPluginHost(IOptions<WebSecurityOptions> websecurityOptions, IOptions<EnvironmentOptions> environmentOptions) : WebPluginHost
{
    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        if (websecurityOptions.Value.IsCorsEnabled)
        {
            Logger.LogInformation("Cors is enabled. Configure further security options");
            services.AddCors(cors =>
                cors.AddDefaultPolicy(policy =>
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        // .AllowAnyOrigin() // Dont activate, otherwise everybody can access the API
                        .WithOrigins(GetOrigins())
                        .WithExposedHeaders("Content-Disposition")
                        .AllowCredentials()
                )
            );
        }

        services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(365);
        });
    }

    public override void Configure(WebHostBuilderContext context, IApplicationBuilder app)
    {
        PreBuild(app);

        var stage = environmentOptions.Value.Stage;
        if (stage == Stage.Development)
        {
            app.UseDeveloperExceptionPage();
        }

        Logger.LogDebug("Enabled Cookie Policy");
        app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax, Secure = CookieSecurePolicy.Always });

        Build(app);

        app.UseStaticFiles();

        if (websecurityOptions.Value.IsCorsEnabled)
            app.UseCors();

        PostBuild(app);
    }

    private void PreBuild(IApplicationBuilder app)
    {
        // Konfiguriert den Forward Header sollte die App hinter einem Proxy laufen
        // z.B. wenn die App hinter einem Loadbalancer oder Reverse Proxy läuft

        // IDEE: Man könnte die KnowProxies und KnownNetworks auch aus der Konfiguration laden
        // dann würde die App nur auf den Forwarded Headers vertrauen, wenn sie von einem
        // vertrauenswürdigen Proxy kommt.

        // KnownNetworks und KnownProxies werden geleert, damit der Forwarded Header
        // unabhängig vom vorgelagerten Proxy akzeptiert wird.

        Logger.LogDebug("Configure Forwarded Headers");
        var fordwardedHeaderOptions = new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All };
        fordwardedHeaderOptions.KnownNetworks.Clear();
        fordwardedHeaderOptions.KnownProxies.Clear();
        app.UseForwardedHeaders(fordwardedHeaderOptions);
    }

    private void Build(IApplicationBuilder app)
    {
        Logger.LogDebug("Configure HSTS");
        app.UseHsts()
            .UseReferrerPolicy(_ =>
            {
                _.NoReferrer();
            });
    }

    private void PostBuild(IApplicationBuilder app)
    {
        var origins = GetOrigins();

        Logger.LogDebug("Configure Security Headers");
        app.UseXXssProtection(options => options.EnabledWithBlockMode());
        app.UseXContentTypeOptions();

        app.UseNoCacheHttpHeaders();
        app.UseXfo(options => options.Deny());
        app.UseCsp(options =>
        {
            options
                .BlockAllMixedContent()
                .BaseUris(_ =>
                {
                    _.Self();
                    _.CustomSources = origins;
                })
                .ObjectSources(_ =>
                {
                    _.None();
                })
                .Sandbox(_ =>
                {
                    _.AllowForms().AllowSameOrigin().AllowScripts().AllowPopups().AllowModals();
                })
                .FrameSources(_ =>
                {
                    _.Self();
                })
                .ConnectSources(_ =>
                {
                    _.Enabled = true;
                    _.Self();
                    _.CustomSources = origins;
                })
                .ImageSources(_ =>
                {
                    _.Enabled = true;
                    _.SelfSrc = true;
                    _.CustomSources = origins.Concat(new List<string> { "data:" });
                })
                .FontSources(_ =>
                {
                    _.Enabled = true;
                    _.SelfSrc = true;
                    _.CustomSources = origins.Concat(new List<string> { "data:" });
                })
                .ScriptSources(_ =>
                {
                    _.Enabled = true;
                    _.SelfSrc = true;
                    _.UnsafeInlineSrc = true;
                    _.UnsafeEvalSrc = true;
                    _.CustomSources = origins;
                })
                .StyleSources(_ =>
                {
                    _.Enabled = true;
                    _.SelfSrc = true;
                    _.UnsafeInlineSrc = true;
                    _.CustomSources = origins;
                })
                .DefaultSources(_ =>
                {
                    _.Enabled = true;
                    _.SelfSrc = true;
                    _.CustomSources = origins;
                });
        });
    }

    private string[] GetOrigins()
    {
        IEnumerable<string> additionalOrigins = new List<string>();
        if (!string.IsNullOrEmpty(websecurityOptions.Value.Origins))
        {
            var splittedOrigins = websecurityOptions.Value.Origins
                .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            additionalOrigins = new List<string>(splittedOrigins);
        }

        IEnumerable<string> origins = new List<string>();
        if (additionalOrigins.Any())
            origins = origins.Concat(additionalOrigins);

        Logger.LogInformation("Cors Origins configured: {0}", string.Join(", ", origins));

        return origins.ToArray();
    }

    private static string CleanDomain(string domain)
    {
        return domain.Replace("http://", "").Replace("https://", "");
    }
}
