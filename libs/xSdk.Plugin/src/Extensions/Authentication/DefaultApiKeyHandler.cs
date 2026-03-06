//using AspNetCore.Authentication.ApiKeyName;

//namespace xSdk.Extensions.Authentication
//{
//    public sealed class DefaultApiKeyHandler : IApiKeyHandler
//    {
//        public Task<IApiKey> GetApiKeyAsync(string key)
//        {
//            // Default dynamic ApiKeyName
//            return Task.FromResult<IApiKey>(new ApiKeyName { Key = Guid.NewGuid().ToString(), OwnerName = "Dynamic API Key User" });
//        }
//    }
//}
