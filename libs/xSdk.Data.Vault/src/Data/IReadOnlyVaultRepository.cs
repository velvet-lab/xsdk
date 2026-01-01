namespace xSdk.Data
{
    public interface IReadOnlyVaultRepository : IRepository
    {
        private const string DefaultPath = "default";

        Task<IDictionary<string, string>> GetSecretsAsync(CancellationToken token = default)
            => GetSecretsAsync(null, DefaultPath, token);

        Task<IDictionary<string, string>> GetSecretsAsync(string path, CancellationToken token = default)
            => GetSecretsAsync(null, path, token);

        Task<IDictionary<string, string>> GetSecretsAsync(string? mountPoint, string path, CancellationToken token = default);
    }
}
