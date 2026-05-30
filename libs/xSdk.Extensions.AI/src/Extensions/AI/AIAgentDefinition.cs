using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Hosting;
using Microsoft.Extensions.AI;

namespace xSdk.Extensions.AI;

public abstract class AIAgentDefinition
{
    public abstract string Name { get; }

    public abstract string Description { get; }

    public virtual string? Instructions { get; }

    internal IChatClient? ChatClient { get; set; }
}
