namespace xSdk.Extensions.Commands;

public interface IApplication
{
    Task<int> RunAsync(string[] args);
}
