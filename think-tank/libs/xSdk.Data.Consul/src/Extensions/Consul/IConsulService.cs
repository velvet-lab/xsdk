using System.Threading;
using System.Threading.Tasks;

namespace xSdk.Extensions.Consul
{
    public interface IConsulService
    {
        string GetKeyValue(string accesstoken, string key);
        Task<string> GetKeyValueAsync(string accesstoken, string key, CancellationToken token = default);

        void RegisterService(string accesstoken, string name);
        void RegisterService(string accesstoken, string name, string address, int port);
        void RegisterService(string accesstoken, string name, string address, int port, string[] tags);

        Task RegisterServiceAsync(string accesstoken, string name, CancellationToken token = default);
        Task RegisterServiceAsync(string accesstoken, string name, string address, int port, CancellationToken token = default);
        Task RegisterServiceAsync(string accesstoken, string name, string address, int port, string[] tags, CancellationToken token = default);
    }
}
