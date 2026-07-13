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

namespace xSdk.Demos.AI.Tools;

public static class WeatherTool
{
    [Description("Get the weather for a given location.")]
    public static string GetWeather(
        [Description("The location to get the weather for.")] string location,
        [Description("The unit of the weather")] string unit)
    {
        // In a real implementation, this method would call a weather API to get the current weather for the specified location.
        // For demonstration purposes, we'll return a hardcoded weather report.
        return $"The current weather in {location} is sunny with a temperature of 25°C.";
    }
}
