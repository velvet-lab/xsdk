using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using NLog;
using xSdk.Data;

namespace xSdk.Security;

public static class CryptoTool
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    private static readonly object LockObject = new object();

    public static void Encrypt<TData>(string file, TData data, string context = "xsdk")
    {
        try
        {
            _logger.Debug("Encrypt data");

            var dataAsJson = JsonSerializer.Serialize(data, JsonHelper.GetSerializerOptions());
            var dataAsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(dataAsJson));

            lock (LockObject)
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
            _logger.Error("The encryption failed. {0}", ex);
            throw;
        }
    }

    public static TData Decrypt<TData>(string file, string context = "xsdk")
    {
        TData result = default;

        try
        {
            _logger.Debug("Decrypt data");

            lock (LockObject)
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
                                result = JsonSerializer.Deserialize<TData>(dataAsJson, JsonHelper.GetSerializerOptions());
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error("The decryption failed. {0}", ex);
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
