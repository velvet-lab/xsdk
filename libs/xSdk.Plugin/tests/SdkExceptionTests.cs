namespace xSdk.Plugin.Tests;

public class SdkExceptionTests
{
    [Fact]
    public void DefaultConstructor_CreatesInstance()
    {
        var ex = new SdkException();

        Assert.NotNull(ex);
    }

    [Fact]
    public void Constructor_WithMessage_SetsMessage()
    {
        var message = "Test error message";

        var ex = new SdkException(message);

        Assert.Equal(message, ex.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_SetsBoth()
    {
        var message = "Outer error";
        var inner = new InvalidOperationException("Inner error");

        var ex = new SdkException(message, inner);

        Assert.Equal(message, ex.Message);
        Assert.Same(inner, ex.InnerException);
    }

    [Fact]
    public void SdkException_IsException()
    {
        var ex = new SdkException("Test");

        Assert.IsAssignableFrom<Exception>(ex);
    }

    [Fact]
    public void SdkException_CanBeThrown()
    {
        void Throw() => throw new SdkException("thrown");
        Assert.Throws<SdkException>(Throw);
    }

    [Fact]
    public void SdkException_CanBeCaught_AsException()
    {
        Exception caught = null;
        try
        {
            throw new SdkException("thrown");
        }
        catch (Exception ex)
        {
            caught = ex;
        }

        Assert.NotNull(caught);
        Assert.IsType<SdkException>(caught);
    }
}
