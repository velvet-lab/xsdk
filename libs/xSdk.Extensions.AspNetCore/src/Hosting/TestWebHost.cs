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

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace xSdk.Hosting;

public static partial class TestWebHost
{
    private const string APP_NAME = "xUnitWebTestHost";
    private const string APP_COMPANY = "xUnit";
    private const string APP_PREFIX = "UnitTest";

    public static IHostBuilder CreateBuilder() => CreateBuilder(new string[] { }, APP_NAME, APP_COMPANY, APP_PREFIX);

    public static IHostBuilder CreateBuilder(string[] args) => CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX);

    public static IHostBuilder CreateBuilder(string[] args, string appName) => CreateBuilder(args, appName, APP_COMPANY, APP_PREFIX);

    public static IHostBuilder CreateBuilder(string[] args, string appName, string appPrefix) => CreateBuilder(args, appName, APP_COMPANY, appPrefix);

    public static IHostBuilder CreateBuilder(string[] args, string? appName, string? appCompany, string? appPrefix)
    {
        var builder = TestHostFactory.CreateTestHost(args, appName, appCompany, appPrefix);
        var slimHost = builder.GetSlimHost();

        builder.ConfigureWebHost(webhostBuilder =>
        {
#pragma warning disable EXTEXP0014 // Der Typ dient nur zu Testzwecken und kann in zukünftigen Aktualisierungen geändert oder entfernt werden. Unterdrücken Sie diese Diagnose, um fortzufahren.
            webhostBuilder
                .UseFakeStartup()
                .ListenHttpOnAnyPort();
#pragma warning restore EXTEXP0014 // Der Typ dient nur zu Testzwecken und kann in zukünftigen Aktualisierungen geändert oder entfernt werden. Unterdrücken Sie diese Diagnose, um fortzufahren.

            webhostBuilder
                .ConfigureServices((context, services) =>
                {
                    slimHost.ConfigureWebPluginHost(x => x.ConfigureServices(context, services));
                });
        });

        return builder;
    }
}
