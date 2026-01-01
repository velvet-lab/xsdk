namespace xSdk.Shared
{
    public static class EnvironmentTools
    {
        public static string? ReadEnvironmentVariable(string key) => ReadEnvironmentVariable(key, null);

        public static string? ReadEnvironmentVariable(string key, string? defaultValue)
        {
            var result = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(result))
            {
                result = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User);
            }

            if (string.IsNullOrEmpty(result))
            {
                result = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);
            }

            if (string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(defaultValue))
            {
                result = defaultValue;
            }

            return result;
        }

        public static bool TryReadEnvironmentVariable(string key, out string value) => TryReadEnvironmentVariable(key, out value, null);

        public static bool TryReadEnvironmentVariable(string key, out string value, string? defaultValue)
        {
            value = string.Empty;

            var result = ReadEnvironmentVariable(key);
            if (!string.IsNullOrEmpty(result))
            {
                value = result;
                return true;
            }

            if (!string.IsNullOrEmpty(defaultValue))
            {
                value = defaultValue;
                return true;
            }

            return false;
        }
    }
}
