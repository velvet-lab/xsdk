using AspNetCore.Authentication.ApiKey;
using xSdk.Data;
using xSdk.Security.Claims;

namespace xSdk.Extensions.Authentication
{
    public interface IApiKeyModel : IModel
    {
        string Key { get; set; }

        string User { get; set; }

        IEnumerable<ClaimModel> Claims { get; set; }

        string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ValidUntil { get; set; }
    }
}
