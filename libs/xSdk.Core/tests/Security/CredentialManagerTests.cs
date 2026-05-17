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

namespace xSdk.Security;

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
        string tempDir = Path.Combine(Path.GetTempPath(), "cred-tests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        Directory.SetCurrentDirectory(tempDir);
    }

    ~CredentialManagerTests()
    {
        Dispose(false);   
    }

    [Fact]
    public void SaveAndLoadCredentials_WithCustomContext_RoundTripsSuccessfully()
    {
        const string context = "test-save-load";
        var credentials = new TestCredentials { User = "admin", Token = "secret-token", ApiUrl = "https://api.example.com" };

        CredentialManager.SaveCredentials(credentials, context);
        TestCredentials? loaded = CredentialManager.LoadCredentials<TestCredentials>(context);

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

        TestCredentials? result = CredentialManager.LoadCredentials<TestCredentials>(context);

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

        TestCredentials? loaded = CredentialManager.LoadCredentials<TestCredentials>(context);
        Assert.Equal(string.Empty, loaded?.User);
    }

    [Fact]
    public void Reset_WhenNoFileExists_DoesNotThrow()
    {
        const string context = "reset-nonexistent-xyz";

        Exception? ex = Record.Exception(() => CredentialManager.Reset(context));

        Assert.Null(ex);
    }

    protected virtual void Dispose(bool disposing)
    {
        Directory.SetCurrentDirectory(_originalDir);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
