namespace xSdk.Extensions.Commands;

public interface ICommandHandler
{
    int Execute();

    Task<int> ExecuteAsync(CancellationToken cancellationToken);
}
