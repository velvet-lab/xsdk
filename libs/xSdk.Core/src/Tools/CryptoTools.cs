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

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using xSdk.Hosting;

namespace xSdk.Tools;

public static class CryptoTools
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    private static readonly object _lockObject = new object();

    public static void Encrypt<TData>(string file, TData data, string context = "xsdk")
    {
        try
        {
            _logger.LogDebug("Encrypt data");

            var dataAsJson = JsonSerializer.Serialize(data, JsonTools.GetSerializerOptions());
            var dataAsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(dataAsJson));

            lock (_lockObject)
            {
                using (FileStream fileStream = new(file, FileMode.OpenOrCreate))
                {
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = CreateKey(context);

                        byte[] iv = aes.IV;
                        fileStream.Write(iv, 0, iv.Length);

                        using (CryptoStream cryptoStream = new(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            // By default, the StreamWriter uses UTF-8 encoding.
                            // Convert change the text encoding, pass the desired encoding as the second parameter.
                            // For example, new StreamWriter(cryptoStream, Encoding.Unicode).
                            using (StreamWriter encryptWriter = new(cryptoStream))
                            {
                                encryptWriter.Write(dataAsBase64);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("The encryption failed. {0}", ex);
            throw;
        }
    }

    public static TData Decrypt<TData>(string file, string context = "xsdk")
    {
        TData result = default;

        try
        {
            _logger.LogDebug("Decrypt data");

            lock (_lockObject)
            {
                using (FileStream fileStream = new(file, FileMode.Open))
                {
                    using (Aes aes = Aes.Create())
                    {
                        byte[] iv = new byte[aes.IV.Length];
                        int numBytesToRead = aes.IV.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }

                        using (CryptoStream cryptoStream = new(fileStream, aes.CreateDecryptor(CreateKey(context), iv), CryptoStreamMode.Read))
                        {
                            // By default, the StreamReader uses UTF-8 encoding.
                            // Convert change the text encoding, pass the desired encoding as the second parameter.
                            // For example, new StreamReader(cryptoStream, Encoding.Unicode).
                            using (StreamReader decryptReader = new(cryptoStream))
                            {
                                string decryptedMessage = decryptReader.ReadToEnd();
                                var dataAsJson = Encoding.UTF8.GetString(Convert.FromBase64String(decryptedMessage));
                                result = JsonSerializer.Deserialize<TData>(dataAsJson, JsonTools.GetSerializerOptions());
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("The decryption failed. {0}", ex);
            throw;
        }

        return result;
    }

    private static byte[] CreateKey(string context)
    {
        var keyAsString = $"#3,{context};1!{Environment.MachineName}#1,{context};2!{Environment.UserName}#2,{context};3!";
        var keyAsBytes = Encoding.UTF8.GetBytes(keyAsString);

        var hashedKey = SHA256.HashData(keyAsBytes);

        return hashedKey.Take(16).ToArray();
    }
}
