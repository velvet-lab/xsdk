using OpenAI;

namespace xSdk.Extensions.AI;

public static class ClientBuilderExtensions
{
    extension(ClientBuilder builder)
    {
        public ClientBuilder WithName(string name)
        {
            builder.Name = name;
            return builder;
        }

        public ClientBuilder WithApiKey(string apiKey)
        {
            builder.ApiKey = apiKey;
            return builder;
        }

        public ClientBuilder WithEndpoint(string endpoint)
        {
            builder.Endpoint = endpoint;
            return builder;
        }

        public ClientBuilder UseOpenAIClient(Action<OpenAIClientOptions> configure)
        {
            builder.OpenAiClientOptionsFactory = configure;
            return builder;
        }
    }
}
