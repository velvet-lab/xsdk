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

using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;
using xSdk;
using xSdk.Data;
using xSdk.Hosting;
using xSdk.Shared;

namespace xSdk.Extensions.Web;

public static class RestClientBuilder
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public static IRestClient CreateRestClient(Uri baseUrl)
        => CreateRestClientWithHandler(baseUrl, default, default, default, default);

    public static IRestClient CreateRestClient(Uri baseUrl, Action<RestClientOptions> options)
        => CreateRestClientWithHandler(baseUrl, default, options, default, default);

    public static IRestClient CreateRestClient(Uri baseUrl, Action<HttpClientHandler> handler)
        => CreateRestClientWithHandler(baseUrl, default, default, handler, default);




    public static IRestClient CreateRestClient(Uri baseUrl, HttpClient httpClient)
        => CreateRestClientWithHttpClient(baseUrl, default, default, httpClient, default);

    public static IRestClient CreateRestClient(Uri baseUrl, HttpClient httpClient, Action<RestClientOptions> options)
        => CreateRestClientWithHttpClient(baseUrl, default, options, httpClient, default);


    public static IRestClient CreateRestClient(Uri baseUrl, IAuthenticator authenticator)
        => CreateRestClientWithHandler(baseUrl, authenticator, default, default, default);

    public static IRestClient CreateRestClient(Uri baseUrl, IAuthenticator authenticator, Action<RestClientOptions>? options)
        => CreateRestClientWithHandler(baseUrl, authenticator, options, default, default);

    public static IRestClient CreateRestClient(Uri baseUrl, IAuthenticator authenticator, Action<HttpClientHandler>? handler)
        => CreateRestClientWithHandler(baseUrl, authenticator, default, handler, default);


    public static IRestClient CreateRestClient(Uri baseUrl, IAuthenticator authenticator, HttpClient httpClient)
        => CreateRestClientWithHttpClient(baseUrl, authenticator, default, httpClient, default);

    public static IRestClient CreateRestClient(
        Uri baseUrl,
        IAuthenticator authenticator,
        HttpClient httpClient,
        Action<RestClientOptions>? options
    )
        => CreateRestClientWithHttpClient(baseUrl, authenticator, options, httpClient, default);



    public static IRestClient CreateRestClient(
        Uri baseUrl,
        IAuthenticator? authenticator,
        Action<RestClientOptions>? options,
        Action<HttpClientHandler>? handler,
        IProgress<double>? progress
    )
        => CreateRestClientWithHandler(baseUrl, authenticator, options, handler, progress);

    public static IRestClient CreateRestClient(
        Uri baseUrl,
        IAuthenticator? authenticator,
        Action<RestClientOptions>? options,
        HttpClient httpClient,
        IProgress<double>? progress
    )
        => CreateRestClientWithHttpClient(baseUrl, authenticator, options, httpClient, progress);


    private static IRestClient CreateRestClientWithHandler(
        Uri baseUrl,
        IAuthenticator? authenticator,
        Action<RestClientOptions>? options,
        Action<HttpClientHandler>? handler,
        IProgress<double>? progress
    )
    {
        HttpClient? httpClient = null;
        if (handler != null)
        {
            httpClient = HttpClientBuilder.CreateHttpClient(baseUrl, handler, progress);
        }
        else
        {
            httpClient = HttpClientBuilder.CreateHttpClient(baseUrl, progress);
        }

        return CreateRestClientWithHttpClient(baseUrl, authenticator, options, httpClient);
    }

    private static IRestClient CreateRestClientWithHttpClient(
        Uri baseUrl,
        IAuthenticator? authenticator,
        Action<RestClientOptions>? options,
        HttpClient httpClient,
        IProgress<double>? progress = default
    )
    {
        _logger.LogTrace("Create rest api client");

        var restOptions = new RestClientOptions(baseUrl);

        // Server Cert Validation
        restOptions.RemoteCertificateValidationCallback = CertificateHelper.ValidateServerCallbacks;
        if (authenticator != null)
        {
            restOptions.Authenticator = authenticator;
        }

        options?.Invoke(restOptions);

        return new RestClient(
            httpClient,
            restOptions,
            configureSerialization: serializer => serializer.UseSystemTextJson(JsonHelper.GetSerializerOptions())
        );
    }
}
