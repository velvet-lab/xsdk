using System.Net.Http.Handlers;
using NLog;
using xSdk.Hosting;
using xSdk.Shared;

namespace xSdk.Extensions.Web;

public static class HttpClientBuilder
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();


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

    private static HttpClient BuildHttpClient(HttpMessageHandler handler, Uri baseUrl)
    {
        var client = new HttpClient(handler, true);
        ConfigureHttpClient(client, baseUrl);
        return client;
    }

    private static void ConfigureHttpClient(HttpClient client, Uri? baseUrl)
    {
        if (baseUrl != null)
            client.BaseAddress = baseUrl;

        string? userAgent = string.Empty;
        string? appPrefix = SlimHost.Instance.AppPrefix;
        string? appVersion = SlimHost.Instance.AppVersion;

        if (!string.IsNullOrEmpty(appPrefix) && !string.IsNullOrEmpty(appVersion))
        {
            userAgent = $"{appPrefix.ToUpper()} {appVersion}";
        }

        if (!string.IsNullOrEmpty(userAgent))
        {
            client.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent);
        }
    }
}
