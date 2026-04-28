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

namespace xSdk.Tools;

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
