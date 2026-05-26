using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace xSdk.Demos.Tools;

internal static class WeatherTool
{
    [Description("Get the weather for a given location.")]
    public static string GetWeather([Description("The location to get the weather for.")] string location)
    {
        // In a real implementation, this method would call a weather API to get the current weather for the specified location.
        // For demonstration purposes, we'll return a hardcoded weather report.
        return $"The current weather in {location} is sunny with a temperature of 25°C.";
    }
}
