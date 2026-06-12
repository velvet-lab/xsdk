using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using xSdk.Extensions.Logging;

namespace xSdk.Extensions.AI;

public static class AIAgentBuilderExtension
{
    extension(AIAgentBuilder builder)
    {
        public AIAgentBuilder EnableTelemetry(bool enableSensitiveData)
        {
            builder
                .UseOpenTelemetry(sourceName: Diagnostics.SourceName, configure: cfg => cfg.EnableSensitiveData = enableSensitiveData);

            return builder;
        }
    }
}
