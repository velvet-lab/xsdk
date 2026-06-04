using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.AI;
using xSdk.Hosting;

namespace xSdk.Extensions.AI;

public static class ChatClientBuilderExtensions
{
    extension(ChatClientBuilder builder)
    {
        public ChatClientBuilder EnableTelemetry(bool enableSensitiveData)
        {
            builder
                .UseOpenTelemetry(sourceName: Diagnostics.SourceName, configure: cfg => cfg.EnableSensitiveData = enableSensitiveData)
                .UseLogging(LogManager.Factory);

            return builder;
        }
    }
}
