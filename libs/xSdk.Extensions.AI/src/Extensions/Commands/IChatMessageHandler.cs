namespace xSdk.Extensions.Commands;

public interface IChatMessageHandler
{
    int HandleMessage(string? message)
        => HandleMessageAsync(message).GetAwaiter().GetResult();

    Task<int> HandleMessageAsync(string? message, CancellationToken token = default);
}
