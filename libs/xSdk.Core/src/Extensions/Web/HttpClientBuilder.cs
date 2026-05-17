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
using System.Net.Http.Handlers;
using Microsoft.Extensions.Logging;
using xSdk.Hosting;
using xSdk.Tools;

namespace xSdk.Extensions.Web;

[ExcludeFromCodeCoverage]
public static class HttpClientBuilder
{
    public static HttpClient CreateHttpClient(Uri? baseUrl)
        => CreateHttpClientInternal(baseUrl, null, null);

    public static HttpClient CreateHttpClient(Uri? baseUrl, IProgress<double>? progress)
        => CreateHttpClientInternal(baseUrl, null, progress);

    public static HttpClient CreateHttpClient(Uri? baseUrl, Action<HttpClientHandler>? configure)
        => CreateHttpClientInternal(baseUrl, configure, null);

    public static HttpClient CreateHttpClient(Uri? baseUrl, Action<HttpClientHandler>? configure, IProgress<double>? progress)
        => CreateHttpClientInternal(baseUrl, configure, progress);

    private static HttpClient CreateHttpClientInternal(Uri? baseUrl, Action<HttpClientHandler>? configure, IProgress<double>? progress)
    {
        HttpMessageHandler usedHandler;

        var defaultHandler = new HttpClientHandler()
        {
            UseProxy = false,
            AllowAutoRedirect = true,
            ServerCertificateCustomValidationCallback = CertificateHelper.ValidateServerCallbacks,
        };
        configure?.Invoke(defaultHandler);
        usedHandler = defaultHandler;

        if (progress != null)
        {
            var progressHandler = new ProgressMessageHandler(defaultHandler);
            progressHandler.HttpSendProgress += (_, args) => progress.Report(args.ProgressPercentage);
            progressHandler.HttpReceiveProgress += (_, args) => progress.Report(args.ProgressPercentage);
            usedHandler = progressHandler;
        }

        return BuildHttpClient(usedHandler, baseUrl);
    }

    private static HttpClient BuildHttpClient(HttpMessageHandler handler, Uri? baseUrl)
    {
        var client = new HttpClient(handler, true);
        ConfigureHttpClient(client, baseUrl);
        return client;
    }

    private static void ConfigureHttpClient(HttpClient client, Uri? baseUrl)
    {
        throw new NotImplementedException();

        //if (baseUrl != null)
        //{
        //    client.BaseAddress = baseUrl;
        //}

        //string? userAgent = string.Empty;

        //string? appPrefix = SlimHost.Instance.AppPrefix;
        //string? appVersion = SlimHost.Instance.AppVersion;

        //if (!string.IsNullOrEmpty(appPrefix) && !string.IsNullOrEmpty(appVersion))
        //{
        //    userAgent = $"{appPrefix.ToUpper()} {appVersion}";
        //}

        //if (!string.IsNullOrEmpty(userAgent))
        //{
        //    client.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent);
        //}
    }
}
