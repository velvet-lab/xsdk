using Microsoft.Extensions.AI;

namespace xSdk.Extensions.AI;

public static class ChatClientBuilderExtensions
{
    extension(ChatClientBuilder builder)
    {
        public ChatClientBuilder EnableTelemetry(bool enableSensitiveData)
        {
            builder
                .UseOpenTelemetry(sourceName: Diagnostics.SourceName, configure: cfg => cfg.EnableSensitiveData = enableSensitiveData);
            //.UseLogging();

            return builder;
        }
    }
}
