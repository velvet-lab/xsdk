/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

//using xSdk.Data;
//using xSdk.Shared;
//using Microsoft.Extensions.Logging;
//using RestSharp;
//using System;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.Json;
//using System.Threading;
//using System.Threading.Tasks;

//namespace xSdk.Extensions.Package.Stores.Artifactory
//{
//    internal partial class ArtifactoryManager
//    {
//        internal Task<ReleaseInfo> GetReleaseInfoFromStoreAsync(PackageModel outputPackage, string repository, CancellationToken token = default)
//        {
//            var metaFilePath = new FileInfo(outputPackage.MetaFileName).Directory.FullName;
//            return GetReleaseInfoFromStoreAsync(outputPackage.NativeName, metaFilePath, repository, token);
//        }

//        internal async Task<ReleaseInfo> GetReleaseInfoFromStoreAsync(string name, string destination, string repository, CancellationToken token = default)
//        {
//            _logger.LogInformation("Load release info");
//            if (string.IsNullOrEmpty(repository))
//            {
//                repository = Globals.DefaultRepository;
//            }

//            var metaFileName = $"{name}.json";
//            var metaFilePath = Path.Combine(destination, metaFileName);

//            if (File.Exists(metaFilePath))
//            {
//                File.Delete(metaFilePath);
//            }

//            _logger.LogTrace("Download release info from artifactory");
//            if (!File.Exists(metaFilePath))
//            {
//                using (var client = CreateRestClient("Start downloading artifact", false))
//                {
//                    var artifactList = await SearchByNameAsync(client, repository, name, token);
//                    if (artifactList != null && artifactList.Artifacts.Any())
//                    {
//                        var downloadUri = artifactList.Artifacts.FirstOrDefault(x => x.Path.EndsWith(".json"))?.DownLoadUri;
//                        if (!string.IsNullOrEmpty(downloadUri))
//                        {
//                            var request = new RestRequest(downloadUri);
//                            var response = await client.GetAsync(request, token);
//                            if (!string.IsNullOrEmpty(response.Content))
//                            {
//                                File.WriteAllText(metaFilePath, response.Content, Encoding.UTF8);
//                            }
//                        }
//                    }
//                    _consoleSvc.StopProgress();
//                }
//            }

//            var result = new ReleaseInfo();
//            if (File.Exists(metaFilePath))
//            {
//                result = JsonSerializer.Deserialize<ReleaseInfo>(File.ReadAllText(metaFilePath), JsonHelper.GetSerializerOptions());
//            }
//            return result;
//        }

//        private async Task<ArtifactModel> DownloadArtifactInfosAsync(IRestClient client, Uri uri, CancellationToken token = default)
//        {
//            var request = new RestRequest(uri);
//            var response = await client.GetAsync(request, token);

//            HttpStatusCode status = response.StatusCode;
//            if (status != HttpStatusCode.OK
//                && status != HttpStatusCode.NoContent
//                && status != HttpStatusCode.Accepted)
//            {
//                return new ArtifactModel { Uri = uri };
//            }

//            var artifact = JsonSerializer.Deserialize<ArtifactModel>(response.Content, JsonHelper.GetSerializerOptions());
//            return await DownloadArtifactPropertiesAsync(client, uri, artifact, token);
//        }

//        private async Task<ArtifactModel> DownloadArtifactPropertiesAsync(IRestClient client, Uri uri, ArtifactModel artifact, CancellationToken token = default)
//        {
//            var urlWithProperties = $"{uri}?properties";
//            var request = new RestRequest(new Uri(urlWithProperties));
//            var response = await client.GetAsync(request, token);

//            HttpStatusCode status = response.StatusCode;
//            if (status != HttpStatusCode.OK
//                && status != HttpStatusCode.NoContent
//                && status != HttpStatusCode.Accepted)
//            {
//                return artifact;
//            }

//            var tmp = JsonSerializer.Deserialize<ArtifactModel>(response.Content, JsonHelper.GetSerializerOptions());
//            if(tmp.Properties != null && tmp.Properties.Any())
//            {
//                artifact.Properties = tmp.Properties;
//            }

//            return artifact;
//        }
//    }
//}
