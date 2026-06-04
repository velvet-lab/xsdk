using System;
using System.Collections.Generic;
using System.Text;
using xSdk.Extensions.AI.Agents;

namespace xSdk.Demos.AI.Agents;

internal class HelpfulAssistant : AgentDefinition
{
    public override string Name => "helpful-assistant";

    public override string Description => "A helpful assistant that provides guidance and support.";

    public override string? Instructions => """
You are a helpful assistant. Your goal is to provide guidance and support to users in a friendly
and informative manner. Always be polite and try to assist with any questions or problems they may have.
""";

}
