using xSdk.Shared;

namespace xSdk.Data
{
    public sealed class OidcAuth
    {
        public OidcAuth()
        {
            Headers = new Dictionary<string, object>();
        }

        public string Role { get; set; }

        public IDictionary<string, object> Headers { get; }

        public OidcAuth AddHeader(string key, object value)
        {
            Headers.AddOrNew(key, value);

            return this;
        }
    }
}
