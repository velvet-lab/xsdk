//using xSdk.Data;
//using xSdk.Extensions.Web;
//using xSdk.Shared;
//using RestSharp;
//using System.Collections.Generic;
//using System.Net;
//using System.Text.Json;
//using System.Threading;
//using System.Threading.Tasks;

//namespace xSdk.Extensions.Package.Stores.Artifactory
//{
//    internal partial class ArtifactoryManager
//    {
//        public async Task<ArtifactModelList> SearchByNameAsync(string repository, string name, CancellationToken token)
//        {
//            if (string.IsNullOrEmpty(repository))
//            {
//                repository = Globals.DefaultRepository;
//            }

//            using (var client = RestClientBuilder.CreateRestClient(_setup.BaseUrl))
//            {
//                return await SearchByNameAsync(client, repository, name, token);
//            }
//        }

//        public async Task<ArtifactModelList> SearchByPropertiesAsync(string repository, IDictionary<string, string> properties, CancellationToken token = default)
//        {
//            if (string.IsNullOrEmpty(repository))
//            {
//                repository = Globals.DefaultRepository;
//            }

//            using (var client = RestClientBuilder.CreateRestClient(_setup.BaseUrl))
//            {
//                return await SearchByPropertiesAsync(client, repository, properties, token);
//            }
//        }

//        private Task<ArtifactModelList> SearchByNameAsync(IRestClient client, string repository, string name, CancellationToken token)
//        {
//            var url = Globals.ApiBase + $"/search/artifact?name={name}";
//            return SearchInternalAsync(client, url, repository, token);
//        }

//        private Task<ArtifactModelList> SearchByPropertiesAsync(IRestClient client, string repository, IDictionary<string, string> properties, CancellationToken token)
//        {
//            var query = "";
//            foreach(var property in properties)
//            {
//                query += $"&{property.Key}={property.Value}";
//            }
//            query = query.Substring(1);

//            var url = Globals.ApiBase + $"/search/prop?{query}";
//            return SearchInternalAsync(client, url, repository, token);
//        }

//        private async Task<ArtifactModelList> SearchInternalAsync(IRestClient client, string url, string repositoryName, CancellationToken token)
//        {
//            if (string.IsNullOrEmpty(repositoryName))
//            {
//                repositoryName = Globals.DefaultRepository;
//            }

//            url += $"&repos={repositoryName}";

//            RestRequest request = new RestRequest(url);
//            RestResponse response = await client.GetAsync(request, token);

//            HttpStatusCode status = response.StatusCode;
//            if (status != HttpStatusCode.OK
//                && status != HttpStatusCode.NoContent
//                && status != HttpStatusCode.Accepted)
//            {
//                return new ArtifactModelList { Response = response };
//            }

//            var artifacts = new ArtifactModelList();
//            artifacts.Response = response;

//            var items = JsonSerializer.Deserialize<ArtifactModelList>(response.Content, JsonHelper.GetSerializerOptions());
//            foreach (var item in items.Artifacts)
//            {
//                var artifact = await DownloadArtifactInfosAsync(client, item.Uri, token);
//                artifacts.Artifacts.Add(artifact);
//            }

//            return artifacts;
//        }
//    }
//}
