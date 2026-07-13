using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.AI;

namespace xSdk.Extensions.AI;

public static class AgentBuilderExtensions
{

    extension(AgentBuilder builder)
    {
        public AgentBuilder UseClient(string name)
        {
            builder.ClientName = name;
            return builder;
        }

        public AgentBuilder WithName(string name)
        {
            builder.Name = name;
            return builder;
        }

        public AgentBuilder WithDescription(string description)
        {
            builder.Description = description;
            return builder;
        }

        public AgentBuilder WithModel(string model)
        {
            builder.Model = model;
            return builder;
        }

        public AgentBuilder WithFile(string filePath)
        {
            builder.FilePath = filePath;
            return builder;
        }

        public AgentBuilder WithPrompt(string instructions)
        {
            builder.Instructions = instructions;
            return builder;
        }

        public AgentBuilder UseTool(string name)
        {
            builder.Tools.Add(name);
            return builder;
        }
    }
}



    //// ── Prompt ──
    //public AgentBuilder WithPrompt(string prompt)
    //{
    //    // Store the prompt for later use
    //    return this;
    //}

    //// ── Model Client ──
    //public AgentBuilder WithModel<TClient>()
    //    where TClient : IChatClient
    //{
    //    return this;
    //}

    //// ── Tools ──
    //public ToolBuilder WithTool(string name)
    //{
    //    return new ToolBuilder(this, name);
    //}

    //// ── Transport: In-Process ──
    //public AgentBuilder WithInProcess()
    //{
    //    return this;
    //}

    //// ── Transport: Verteilt ──
    //public AgentBuilder Distribute(string url)
    //{
    //    return this;
    //}

    //// ── Endpoints ──
    //public AgentBuilder ExposeOpenAiEndpoint(int port)
    //{

    //    return this;
    //}

    //public AgentBuilder WithHealthChecks()
    //{
    //    return this;
    //}
