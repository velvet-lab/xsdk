using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Agents.AI;

namespace xSdk.Demos.AI.Skills;

#pragma warning disable MAAI001 // Der Typ dient nur zu Testzwecken und kann in zukünftigen Aktualisierungen geändert oder entfernt werden. Unterdrücken Sie diese Diagnose, um fortzufahren.
internal sealed class WeatherExpertSkill : AgentClassSkill<WeatherExpertSkill>
{
    public override AgentSkillFrontmatter Frontmatter { get; } = new(
        name: "weather-skill",
        description: "A skill that provides weather information for a given location."
    );

    protected override string Instructions => """
Use this skill to get the current weather for a specific location.

1. Use the get-weather script to load the weather for a location.
2. Present the weather information in a clear and concise manner.
""";

    [AgentSkillScript("get-weather")]
    [Description("Get the weather for a given location.")]
    public string GetWeather([Description("The location for which to get the weather.")] string location)
    {
        // In a real implementation, this method would call a weather API to get the current weather for the specified location.
        // For demonstration purposes, we'll return a hardcoded weather report.
        return $"The current weather in {location} is sunny with a temperature of 25°C.";
    }
}
#pragma warning restore MAAI001 // Der Typ dient nur zu Testzwecken und kann in zukünftigen Aktualisierungen geändert oder entfernt werden. Unterdrücken Sie diese Diagnose, um fortzufahren.
