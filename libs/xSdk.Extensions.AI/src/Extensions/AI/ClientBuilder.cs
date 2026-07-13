using System.ClientModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using OpenAI;
using xSdk.Extensions.Builder;
using xSdk.Plugins.AI;
using xSdk.Tools;

namespace xSdk.Extensions.AI;

public sealed class ClientBuilder<TBuilder>(TBuilder builder) : ClientBuilder(builder)
    where TBuilder : AIBuilder
{
}

public class ClientBuilder(AIBuilder builder) : BuilderBase    
{
    private readonly Dictionary<string, IChatClient> _chatClients = new();

    public Action<ClientBuilder> ConfigureBuilderAction { get; internal set; }
    internal string? Name { get; set; }    

    internal string? ApiKey { get; set; }

    internal string? Endpoint { get;  set; }    

    internal Action<OpenAIClientOptions>? OpenAiClientOptionsFactory { get; set; }

    internal IChatClient? Build(string? model)
    {
        ConfigureBuilderAction?.Invoke(this);

        if (IsValid<ClientBuilder, ClientBuilderValidator>())
        {
            var key = $"{Name}:{model}".ToLowerInvariant();
            if (_chatClients.TryGetValue(key, out IChatClient? existingClient))
            {
                return existingClient;
            }

            if (OpenAiClientOptionsFactory is not null)
            {
                var chatClientBuilder = BuildOpenAIChatClient(model);
                var chatClient = ConfigureChatClient(chatClientBuilder);
                _chatClients.AddOrNew(key, chatClient);

                return chatClient;
            }            
        }
        return default;
    }

    private ChatClientBuilder BuildOpenAIChatClient(string? model)
    {
        var options = new OpenAIClientOptions
        {
            Endpoint = new Uri(Endpoint)
        };

        OpenAiClientOptionsFactory?.Invoke(options);

        var client = new OpenAIClient(new ApiKeyCredential(ApiKey!), options);

        return client
            .GetChatClient(model)
            .AsIChatClient()
            .AsBuilder();
    }

    private IChatClient ConfigureChatClient(ChatClientBuilder chatClientBuilder)
    {
        if (builder.Options.IsTelemetryEnabled)
        {
            chatClientBuilder
                .UseOpenTelemetry(sourceName: builder.DiagnosticsSourceName, configure: cfg => cfg.EnableSensitiveData = builder.Options.IsSensitiveDataEnabled);

            if (builder.EnableLogging)
            {
                chatClientBuilder
                    .UseLogging(builder.LoggerFactory);
            }
        }

        return chatClientBuilder            
            .Build();
    }
}

