using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Agents.AI;

namespace xSdk.Extensions.AI.Agents;

public interface IAgentService
{
    Task<AIAgent> CreateAgentAsync(CancellationToken token = default);
}
