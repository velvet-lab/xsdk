using Microsoft.Extensions.Options;
using xSdk.Extensions.AI;

namespace xSdk.Demos;

internal class AgentsPluginBuilder() : AIPluginBuilder, IAIPluginBuilder
{
    public override void Initialize()
    {
        // this.RegisterAgentFile("AI\\Agents\\FileInspector.yaml");
    }
}
