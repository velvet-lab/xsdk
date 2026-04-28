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

public class CryptoToolTests : IDisposable
{
    private readonly string _tempDirectory;
    private readonly string _tempFile;

    public CryptoToolTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempDirectory);
        _tempFile = Path.Combine(_tempDirectory, "test.enc");
    }

    private record TestData(string Name, int Age);

    [Fact]
    public void Encrypt_ThenDecrypt_WithDefaultContext_RoundTripsSuccessfully()
    {
        var original = new TestData("Alice", 30);

        CryptoTools.Encrypt(_tempFile, original);
        var result = CryptoTools.Decrypt<TestData>(_tempFile);

        Assert.NotNull(result);
        Assert.Equal(original.Name, result.Name);
        Assert.Equal(original.Age, result.Age);
    }

    [Fact]
    public void Encrypt_ThenDecrypt_WithCustomContext_RoundTripsSuccessfully()
    {
        const string context = "test-context";
        var original = new TestData("Bob", 25);

        CryptoTools.Encrypt(_tempFile, original, context);
        var result = CryptoTools.Decrypt<TestData>(_tempFile, context);

        Assert.NotNull(result);
        Assert.Equal(original.Name, result.Name);
        Assert.Equal(original.Age, result.Age);
    }

    [Fact]
    public void Encrypt_CreatesFile()
    {
        var data = new TestData("Test", 1);

        CryptoTools.Encrypt(_tempFile, data);

        Assert.True(File.Exists(_tempFile));
    }

    [Fact]
    public void Encrypt_ThenDecrypt_WithStringData_RoundTripsSuccessfully()
    {
        var original = "Hello, World!";
        var file = Path.Combine(_tempDirectory, "string.enc");

        CryptoTools.Encrypt(file, original);
        var result = CryptoTools.Decrypt<string>(file);

        Assert.Equal(original, result);
    }

    [Fact]
    public void Decrypt_WithNonExistentFile_ThrowsException()
    {
        var nonExistentFile = Path.Combine(_tempDirectory, "nonexistent.enc");

        Assert.Throws<FileNotFoundException>(() => CryptoTools.Decrypt<TestData>(nonExistentFile));
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDirectory))
            Directory.Delete(_tempDirectory, recursive: true);
    }
}
