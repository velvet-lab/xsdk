/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.ComponentModel;
using Microsoft.Agents.AI;

namespace xSdk.Demos.AI.Skills;

#pragma warning disable MAAI001 // Der Typ dient nur zu Testzwecken und kann in zukünftigen Aktualisierungen geändert oder entfernt werden. Unterdrücken Sie diese Diagnose, um fortzufahren.
public sealed class WeatherExpertSkill : AgentClassSkill<WeatherExpertSkill>
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
