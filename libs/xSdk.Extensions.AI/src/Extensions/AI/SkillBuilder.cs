using System;
using System.Collections.Generic;
using System.Text;
using xSdk.Extensions.Builder;
using xSdk.Plugins.AI;

namespace xSdk.Extensions.AI;

public sealed class SkillBuilder<TBuilder>(TBuilder builder) : SkillBuilder(builder)
    where TBuilder : AIBuilder
{ }

public class SkillBuilder(AIBuilder builder) : BuilderBase
{
    public Action<SkillBuilder> ConfigureBuilderAction { get; internal set; }

    public string Name { get; internal set; }
}
