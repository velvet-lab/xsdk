using System.ClientModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI;
using xSdk.Extensions.AI;
using xSdk.Extensions.Plugin;

namespace xSdk.Demos.Builders;

internal class AgentsPluginBuilder(IOptions<AgentsPluginOptions> pluginOptions) : PluginBuilder, IAgentsPluginBuilder
{
    public IChatClient CreateChatClient() 
    {
        AgentsPluginOptions options = pluginOptions.Value;
        if (options is not null)
        {
            var openaiClient = new OpenAIClient(new ApiKeyCredential(options.ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(options.Endpoint),
            });

            return openaiClient.GetChatClient("gemello").AsIChatClient();
        }

        throw new InvalidOperationException("Invalid plugin options");
    }
}
