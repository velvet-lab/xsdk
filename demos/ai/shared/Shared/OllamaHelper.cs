using OllamaSharp;

namespace xSdk.Demos;

public static class OllamaHelper
{
    private const string Endpoint = "http://192.168.189.32:11434";    

    public static OllamaApiClient CreateClient()
    {
        return new OllamaApiClient(new OllamaApiClient.Configuration
        {
            Uri = new Uri(Endpoint)
        });
    }
}
