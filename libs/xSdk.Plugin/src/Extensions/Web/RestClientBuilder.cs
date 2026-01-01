using xSdk.Data;
using xSdk.Shared;
using NLog;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;

namespace xSdk.Extensions.Web
{
    public static class RestClientBuilder
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

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

        public static IRestClient CreateRestClient(Uri baseUrl, IAuthenticator authenticator, HttpClient httpClient, Action<RestClientOptions>? options)
            => CreateRestClientWithHttpClient(baseUrl, authenticator, options, httpClient, default);



        public static IRestClient CreateRestClient(Uri baseUrl, IAuthenticator? authenticator, Action<RestClientOptions>? options, Action<HttpClientHandler>? handler, IProgress<double>? progress)
            => CreateRestClientWithHandler(baseUrl, authenticator, options, handler, progress);

        public static IRestClient CreateRestClient(Uri baseUrl, IAuthenticator? authenticator, Action<RestClientOptions>? options, HttpClient httpClient, IProgress<double>? progress)
            => CreateRestClientWithHttpClient(baseUrl, authenticator, options, httpClient, progress);


        private static IRestClient CreateRestClientWithHandler(Uri baseUrl, IAuthenticator? authenticator, Action<RestClientOptions>? options, Action<HttpClientHandler>? handler, IProgress<double>? progress)
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

            return CreateRestClientWithHttpClient(baseUrl, authenticator, options, httpClient, progress);
        }

        private static IRestClient CreateRestClientWithHttpClient(Uri baseUrl, IAuthenticator? authenticator, Action<RestClientOptions>? options, HttpClient httpClient, IProgress<double>? progress)
        {
            logger.Trace("Create rest api client");

            var restOptions = new RestClientOptions(baseUrl);

            // Server Cert Validation
            restOptions.RemoteCertificateValidationCallback = CertificateHelper.ValidateServerCallbacks;
            if (authenticator != null)
            {
                restOptions.Authenticator = authenticator;
            }

            options?.Invoke(restOptions);

            return new RestClient(httpClient, restOptions, configureSerialization: s => s.UseSystemTextJson(JsonHelper.GetSerializerOptions()));
        }
    }
}
