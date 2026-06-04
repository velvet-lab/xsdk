using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using xSdk.Demos.AI;
using xSdk.Demos.AI.Tools;
using xSdk.Extensions.AI;

namespace xSdk.Demos.Builders;

internal class AgentsPluginBuilder(IOptions<AIPluginOptions> pluginOptions) : AIPluginBuilder, IAIPluginBuilder
{
    public override void Initialize()
    {
        // Simple Agent
        this.RegisterAgentFile("AI\\Agents\\Assistant.yaml");

        // A Weather Agent with Tools
        this.RegisterAgentFile("AI\\Agents\\GetWeather.yaml", [AIFunctionFactory.Create(WeatherTool.GetWeather)]);
    }

    public override IChatClient? CreateChatClient()
        => OpenAIHelper.CreateChatClient(pluginOptions.Value);
}
