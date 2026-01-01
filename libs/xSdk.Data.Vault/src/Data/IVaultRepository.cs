namespace xSdk.Data
{
    public interface IVaultRepository : IReadOnlyVaultRepository
    {

        private const string DefaultPath = "default";

        Task<bool> AddSecretAsync(string key, object data, CancellationToken token = default)
            => AddSecretAsync(null, DefaultPath, key, data, token);

        Task<bool> AddSecretAsync(string path, string key, object data, CancellationToken token = default)
            => AddSecretAsync(null, path, key, data, token);

        Task<bool> AddSecretAsync(string? mountpoint, string path, string key, object data, CancellationToken token = default)
            => AddSecretAsync(mountpoint, path, new Dictionary<string, object> { { key, data } }, token);



        Task<bool> AddSecretAsync(Dictionary<string, object> data, CancellationToken token = default)
            => AddSecretAsync(null, DefaultPath, data, token);

        Task<bool> AddSecretAsync(string path, Dictionary<string, object> data, CancellationToken token = default)
            => AddSecretAsync(null, path, data, token);

        Task<bool> AddSecretAsync(string? mountpoint, string path, Dictionary<string, object> data, CancellationToken token = default);
    }
}
