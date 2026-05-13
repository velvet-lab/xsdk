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

using System.Globalization;

namespace xSdk.Tools;

public static class TimeSpanParser
{
    public static bool TryParse(object? value, out TimeSpan result)
    {
        if (value == null)
        {
            result = TimeSpan.Zero;
            return false;
        }

        TimeSpan parseResult = Parse(value);
        if (parseResult != TimeSpan.Zero)
        {
            result = parseResult;
            return true;
        }

        result = TimeSpan.Zero;
        return false;
    }

    public static TimeSpan Parse(object? value)
    {        
        string? stringValue = value?.ToString();
        stringValue = ValidateString(stringValue);

        if (stringValue != null)
        {
            string unit = stringValue.Substring(stringValue.Length - 2);
            if (!IsValidUnit(unit))
            {
                unit = stringValue.Substring(stringValue.Length - 1);
                stringValue = stringValue.Substring(0, stringValue.Length - 1);
            }
            else
            {
                stringValue = stringValue.Substring(0, stringValue.Length - 2);
            }

            // Default Unit is Seconds
            if (!IsValidUnit(unit))
            {
                unit = "s";
            }

            unit = unit.ToLower();
            double doubleValue = double.Parse(stringValue, CultureInfo.InvariantCulture);
            if (unit == "ms")
            {
                return TimeSpan.FromMilliseconds(doubleValue);
            }
            else if (unit == "s")
            {
                return TimeSpan.FromSeconds(doubleValue);
            }
            else if (unit == "m")
            {
                return TimeSpan.FromMinutes(doubleValue);
            }
            else if (unit == "h")
            {
                return TimeSpan.FromHours(doubleValue);
            }
            else if (unit == "d")
            {
                return TimeSpan.FromDays(doubleValue);
            }
        }

        return TimeSpan.Zero;        
    }

    private static bool IsValidUnit(string value)
    {
        string[] units = ["ms", "s", "m", "h", "d"];

        return units.Contains(value.ToLower());
    }

    private static string? ValidateString(string? value)
    {
        if(value == null)
        {
            return null;
        }

        bool existUnit = false;
        new List<string>() { "ms", "s", "m", "h", "d" }
            .ToList()
            .ForEach(x =>
            {
                if (value.ToLower().EndsWith(x))
                {
                    existUnit = true;
                }
            });

        // Default Unit is Seconds
        if (!existUnit)
        {
            return $"{value}s";
        }

        return value;
    }
}
