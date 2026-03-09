using System.Globalization;

namespace xSdk.Shared;

public static class TimeSpanParser
{
    public static bool TryParse(object value, out TimeSpan result)
    {
        if (value == null)
        {
            result = TimeSpan.Zero;
            return false;
        }

        var parseResult = Parse(value);
        if (parseResult != TimeSpan.Zero)
        {
            result = parseResult;
            return true;
        }

        result = TimeSpan.Zero;
        return false;
    }

    public static TimeSpan Parse(object value)
    {
        try
        {
            var stringValue = value.ToString();
            stringValue = ValidateString(stringValue);

            var unit = stringValue.Substring(stringValue.Length - 2);
            if (!IsValidUnit(unit))
            {
                unit = stringValue.Substring(stringValue.Length - 1);
                stringValue = stringValue.Substring(0, stringValue.Length - 1);
            }
            else
                stringValue = stringValue.Substring(0, stringValue.Length - 2);

            // Default Unit is Seconds
            if (!IsValidUnit(unit))
                unit = "s";

            unit = unit.ToLower();
            var doubleValue = double.Parse(stringValue, CultureInfo.InvariantCulture);
            if (unit == "ms")
                return TimeSpan.FromMilliseconds(doubleValue);
            else if (unit == "s")
                return TimeSpan.FromSeconds(doubleValue);
            else if (unit == "m")
                return TimeSpan.FromMinutes(doubleValue);
            else if (unit == "h")
                return TimeSpan.FromHours(doubleValue);
            else if (unit == "d")
                return TimeSpan.FromDays(doubleValue);

            return TimeSpan.Zero;
        }
        catch
        {
            throw;
        }
    }

    private static bool IsValidUnit(string value)
    {
        var units = new string[] { "ms", "s", "m", "h", "d" };

        return units.Contains(value.ToLower());
    }

    private static string ValidateString(string value)
    {
        var existUnit = false;
        new List<string>() { "ms", "s", "m", "h", "d" }
            .ToList()
            .ForEach(x =>
            {
                if (value.ToLower().EndsWith(x))
                    existUnit = true;
            });

        // Default Unit is Seconds
        if (!existUnit)
            return $"{value}s";

        return value;
    }
}
