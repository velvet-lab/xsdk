using xSdk.Security;

namespace xSdk.Plugin.Tests.Security;

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

        CryptoTool.Encrypt(_tempFile, original);
        var result = CryptoTool.Decrypt<TestData>(_tempFile);

        Assert.NotNull(result);
        Assert.Equal(original.Name, result.Name);
        Assert.Equal(original.Age, result.Age);
    }

    [Fact]
    public void Encrypt_ThenDecrypt_WithCustomContext_RoundTripsSuccessfully()
    {
        const string context = "test-context";
        var original = new TestData("Bob", 25);

        CryptoTool.Encrypt(_tempFile, original, context);
        var result = CryptoTool.Decrypt<TestData>(_tempFile, context);

        Assert.NotNull(result);
        Assert.Equal(original.Name, result.Name);
        Assert.Equal(original.Age, result.Age);
    }

    [Fact]
    public void Encrypt_CreatesFile()
    {
        var data = new TestData("Test", 1);

        CryptoTool.Encrypt(_tempFile, data);

        Assert.True(File.Exists(_tempFile));
    }

    [Fact]
    public void Encrypt_ThenDecrypt_WithStringData_RoundTripsSuccessfully()
    {
        var original = "Hello, World!";
        var file = Path.Combine(_tempDirectory, "string.enc");

        CryptoTool.Encrypt(file, original);
        var result = CryptoTool.Decrypt<string>(file);

        Assert.Equal(original, result);
    }

    [Fact]
    public void Decrypt_WithNonExistentFile_ThrowsException()
    {
        var nonExistentFile = Path.Combine(_tempDirectory, "nonexistent.enc");

        Assert.Throws<FileNotFoundException>(() => CryptoTool.Decrypt<TestData>(nonExistentFile));
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDirectory))
            Directory.Delete(_tempDirectory, recursive: true);
    }
}
