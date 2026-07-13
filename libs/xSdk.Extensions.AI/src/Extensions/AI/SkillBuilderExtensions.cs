using Microsoft.Agents.AI;

namespace xSdk.Extensions.AI;

public static class SkillBuilderExtensions
{
    extension(SkillBuilder builder)
    {
        public SkillBuilder WithName(string name)
        {
            builder.Name = name;
            return builder;
        }

        public SkillBuilder WithInlineSkill(Action<AgentInlineSkill> configure)
        {
            return builder;
        }
    }
}
