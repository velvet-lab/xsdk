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

using System.Text;

namespace xSdk.Shared;

public static class Base64Helper
{
    public static string ConvertFromBase64(string encoded)
    {
        var result = encoded;
        try
        {
            if (!string.IsNullOrEmpty(encoded) && IsBase64(encoded))
            {
                var encodings = new Encoding[] { Encoding.UTF8, Encoding.Unicode };

                var stringAsBytes = Convert.FromBase64String(encoded);
                foreach (var encoding in encodings)
                {
                    var value = Decode(stringAsBytes, encoding);
                    if (!string.IsNullOrEmpty(value))
                    {
                        result = value;
                        break;
                    }
                }
            }
        }
        catch { }

        return result;
    }

    public static string ConvertToBase64(string value) => ConvertToBase64(value, Encoding.UTF8);

    public static string ConvertToBase64(string value, Encoding encoding)
    {
        if (string.IsNullOrEmpty(value))
        {
            value = "";
        }

        return Convert.ToBase64String(encoding.GetBytes(value));
    }

    public static bool IsBase64(string encoded)
    {
        var result = false;

        if (!string.IsNullOrEmpty(encoded))
        {
            var buffer = new Span<byte>(new byte[encoded.Length]);

            if (Convert.TryFromBase64String(encoded, buffer, out int bytesParsed))
            {
                try
                {
                    var encodings = new Encoding[] { Encoding.UTF8, Encoding.Unicode };

                    var stringAsBytes = Convert.FromBase64String(encoded);
                    foreach (var encoding in encodings)
                    {
                        var value = Decode(stringAsBytes, encoding);
                        if (!string.IsNullOrEmpty(value))
                        {
                            result = true;
                        }
                    }
                }
                catch
                {
                    // Only catch, nothing to tell
                }
            }
        }

        return result;
    }

    private static string Decode(byte[] encoded, Encoding encoding)
    {
        if (encoded.Any())
        {
            try
            {
                var value = encoding.GetString(encoded);
                if (value.IndexOf('\0') > -1)
                {
                    return null;
                }

                if (!IsUnicode(value))
                {
                    return value;
                }
            }
            catch { }
        }
        return null;
    }

    private static bool IsUnicode(string value)
    {
        if (value.Any(x => x > 255))
        {
            return true;
        }

        return false;
    }
}
