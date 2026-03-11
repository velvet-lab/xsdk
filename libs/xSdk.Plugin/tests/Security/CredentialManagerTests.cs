using xSdk.Security;

namespace xSdk.Plugin.Tests.Security;

public class CredentialManagerTests : IDisposable
{
    private class TestCredentials : Credentials
    {
        public string ApiUrl { get; set; } = string.Empty;
    }

    private readonly string _originalDir;

    public CredentialManagerTests()
    {
        _originalDir = Directory.GetCurrentDirectory();
        var tempDir = Path.Combine(Path.GetTempPath(), "cred-tests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        Directory.SetCurrentDirectory(tempDir);
    }

    [Fact]
    public void SaveAndLoadCredentials_WithCustomContext_RoundTripsSuccessfully()
    {
        const string context = "test-save-load";
        var credentials = new TestCredentials { User = "admin", Token = "secret-token", ApiUrl = "https://api.example.com" };

        CredentialManager.SaveCredentials(credentials, context);
        var loaded = CredentialManager.LoadCredentials<TestCredentials>(context);

        Assert.NotNull(loaded);
        Assert.Equal(credentials.User, loaded.User);
        Assert.Equal(credentials.Token, loaded.Token);
        Assert.Equal(credentials.ApiUrl, loaded.ApiUrl);

        CredentialManager.Reset(context);
    }

    [Fact]
    public void LoadCredentials_WhenNoFileExists_ReturnsNewInstance()
    {
        const string context = "nonexistent-context-xyz";

        var result = CredentialManager.LoadCredentials<TestCredentials>(context);

        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.User);
        Assert.Equal(string.Empty, result.Token);
    }

    [Fact]
    public void Reset_WithExistingFile_DeletesFile()
    {
        const string context = "test-reset";
        var credentials = new TestCredentials { User = "user", Token = "token" };
        CredentialManager.SaveCredentials(credentials, context);

        CredentialManager.Reset(context);

        var loaded = CredentialManager.LoadCredentials<TestCredentials>(context);
        Assert.Equal(string.Empty, loaded.User);
    }

    [Fact]
    public void Reset_WhenNoFileExists_DoesNotThrow()
    {
        const string context = "reset-nonexistent-xyz";

        var ex = Record.Exception(() => CredentialManager.Reset(context));

        Assert.Null(ex);
    }

    public void Dispose()
    {
        Directory.SetCurrentDirectory(_originalDir);
    }
}
