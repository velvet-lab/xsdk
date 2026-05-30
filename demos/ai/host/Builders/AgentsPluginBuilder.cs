using System.ClientModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI;
using xSdk.Demos.AI.Agents;
using xSdk.Extensions.AI;
using xSdk.Extensions.Plugin;

namespace xSdk.Demos.Builders;

internal class AgentsPluginBuilder(IOptions<AIPluginOptions> pluginOptions) : PluginBuilder, IAIPluginBuilder
{
    public IChatClient CreateDefaultChatClient() 
    {
        AIPluginOptions options = pluginOptions.Value;
        if (options is not null)
        {
            var openaiClient = new OpenAIClient(new ApiKeyCredential(options.ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(options.Endpoint),
            });

            return openaiClient.GetChatClient(options.Model).AsIChatClient();
        }

        throw new InvalidOperationException("Invalid plugin options");
    }

    public void Initialize()
    {
        this.RegisterAgent<HelpfulAssistant>();
    }
}
