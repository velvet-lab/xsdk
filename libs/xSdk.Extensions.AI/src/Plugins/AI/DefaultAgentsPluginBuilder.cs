using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using xSdk.Extensions.AI;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Telemetry;

namespace xSdk.Plugins.AI;

internal class DefaultAgentsPluginBuilder : PluginBuilder, IAgentsPluginBuilder
{
    public IChatClient CreateChatClient() => throw new NotImplementedException();
}
