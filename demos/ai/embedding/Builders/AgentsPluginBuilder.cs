using System.ClientModel;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI;
using xSdk.Extensions.AI;

namespace xSdk.Demos.Builders;

internal class AgentsPluginBuilder(IOptions<AIPluginOptions> pluginOptions) : AIPluginBuilder, IAIPluginBuilder
{
    public override void Initialize()
    {
        
    }

      
}
