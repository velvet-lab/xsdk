namespace xSdk.Data;

public class FlatFileDatabaseSetupTests
{
    [Fact]
    public void FlatFileDatabaseSetup_DefaultConstructor_SetsUseLowerCamelCaseToTrue()
    {
        var setup = new FlatFileDatabaseSetup();

        Assert.True(setup.UseLowerCamelCase);
    }

    [Fact]
    public void FlatFileDatabaseSetup_DefaultConstructor_SetsReloadBeforeGetCollectionToFalse()
    {
        var setup = new FlatFileDatabaseSetup();

        Assert.False(setup.ReloadBeforeGetCollection);
    }

    [Fact]
    public void FlatFileDatabaseSetup_FilePath_CanBeSet()
    {
        var setup = new FlatFileDatabaseSetup
        {
            FilePath = "/tmp/test.json"
        };

        Assert.Equal("/tmp/test.json", setup.FilePath);
    }

    [Fact]
    public void FlatFileDatabaseSetup_KeyProperty_CanBeSet()
    {
        var setup = new FlatFileDatabaseSetup
        {
            KeyProperty = "id"
        };

        Assert.Equal("id", setup.KeyProperty);
    }

    [Fact]
    public void FlatFileDatabaseSetup_EncryptionKey_CanBeSet()
    {
        var setup = new FlatFileDatabaseSetup
        {
            EncryptionKey = "secret-key"
        };

        Assert.Equal("secret-key", setup.EncryptionKey);
    }

    [Fact]
    public void FlatFileDatabaseSetup_UseLowerCamelCase_CanBeOverridden()
    {
        var setup = new FlatFileDatabaseSetup
        {
            UseLowerCamelCase = false
        };

        Assert.False(setup.UseLowerCamelCase);
    }

    [Fact]
    public void FlatFileDatabaseSetup_ReloadBeforeGetCollection_CanBeOverridden()
    {
        var setup = new FlatFileDatabaseSetup
        {
            ReloadBeforeGetCollection = true
        };

        Assert.True(setup.ReloadBeforeGetCollection);
    }

    [Fact]
    public void Validate_WithEmptyFilePath_AddsValidationError()
    {
        var setup = new FlatFileDatabaseSetup { FilePath = string.Empty };

        setup.Validate(false);

        Assert.NotEmpty(setup.Results);
    }

    [Fact]
    public void Validate_WithValidFilePath_NoValidationErrors()
    {
        var setup = new FlatFileDatabaseSetup { FilePath = "mydata.json" };

        setup.Validate(false);

        Assert.Empty(setup.Results);
    }

    [Fact]
    public void Validate_FilePathWithoutExtension_AppendsJsonExtension()
    {
        var setup = new FlatFileDatabaseSetup { FilePath = "mydata" };

        setup.Validate(false);

        Assert.Equal("mydata.json", setup.FilePath);
    }

    [Fact]
    public void Validate_FilePathAlreadyWithJsonExtension_DoesNotDoubleAppend()
    {
        var setup = new FlatFileDatabaseSetup { FilePath = "mydata.json" };

        setup.Validate(false);

        Assert.Equal("mydata.json", setup.FilePath);
    }

    [Fact]
    public void Validate_WithEmptyFilePath_ThrowsWhenRequired()
    {
        var setup = new FlatFileDatabaseSetup { FilePath = string.Empty };

        Assert.Throws<SdkException>(() => setup.Validate(true));
    }
}
