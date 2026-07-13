using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.AI;

namespace xSdk.Extensions.AI;

public static class ToolBuilderExtensions
{
    extension(ToolBuilder builder)
    {
        public ToolBuilder WithName(string name)
        {
            builder.Name = name;
            return builder;
        }

        public ToolBuilder WithDescription(string description)
        {
            builder.Description = description;
            return builder;
        }

        public ToolBuilder UseInProcess(Delegate method)
            => builder.UseInProcess(method, new AIFunctionFactoryOptions());

        public ToolBuilder UseInProcess(Delegate method, AIFunctionFactoryOptions options)
        {
            builder.InProcessDelegate = method;
            builder.InProcessOptions = options;
            return builder;
        }
    }
}


//// MCP: Über HTTP
//public AgentBuilder UseMcp(string url)
//{
//    _factory = sp => new McpAgentTool(_name, url);
//    RegisterTool();
//    return _parent;
//}

//// A2A: Über A2A Protocol
//public AgentBuilder UseA2a(string url)
//{
//    _factory = sp => new A2aAgentTool(_name, url);
//    RegisterTool();
//    return _parent;
//}

//// OpenAI-kompatibel
//public AgentBuilder UseOpenAi(string url, string model)
//{
//    _factory = sp => new OpenAiAgentTool(_name, url, model);
//    RegisterTool();
//    return _parent;
//}
