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

//using RestSharp;
//using System.Dynamic;
//using System.Net;
//using System.Text.Json;
//using xSdk.Data;

//namespace xSdk.Extensions.Package.Stores.Artifactory
//{
//    internal partial class ArtifactoryManager
//    {
//        internal Task UploadAsync(string repository, string location, string artifact, CancellationToken token = default)
//            => UploadAsync(repository, location, artifact, null, token);

//        internal async Task UploadAsync(string repository, string location, string artifact, Dictionary<string, string> properties, CancellationToken token = default)
//        {
//            if (string.IsNullOrEmpty(repository))
//            {
//                repository = Globals.DefaultRepository;
//            }

//            if (string.IsNullOrEmpty(location))
//            {
//                location = Globals.DefaultLocation;
//            }

//            using (var client = CreateRestClient("Start uploading artifact"))
//            {
//                var name = artifact.Split(@"\").Last();
//                var path = location + "/" + name;
//                var baseUrl = repository + "/" + path;

//                var canUpload = false;
//                RestRequest request = new RestRequest(baseUrl, Method.Put);
//                if (artifact.EndsWith(".json"))
//                {
//                    var data = File.ReadAllText(artifact);
//                    request.AddStringBody(data, ContentType.Json);
//                    canUpload = true;
//                }
//                else if (artifact.EndsWith(".zip"))
//                {
//                    var data = File.ReadAllBytes(artifact);
//                    request.AddBody(data, ContentType.GZip);
//                    canUpload = true;
//                }
//                else
//                {
//                    throw new SdkException("Unknown content type for upload artifact");
//                }

//                if (canUpload)
//                {
//                    var response = await client.ExecuteAsync(request, token);
//                    var statusCode = response.StatusCode;
//                    if (statusCode != HttpStatusCode.Created)
//                    {
//                        throw new SdkException("Artifact could not uploaded/updated");
//                    }

//                    await UpdateProperties(client, baseUrl, properties, token);
//                }
//            }
//        }

//        private async Task UpdateProperties(IRestClient client, string baseUrl, Dictionary<string, string> properties, CancellationToken token)
//        {
//            var content = SerializeProperties(properties);
//            if (!string.IsNullOrEmpty(content))
//            {
//                // Dieses Schnipsel funktioniert nicht wirklich. In Insomia geht alles, hier im Code schafft er es nicht die Properties
//                // zu setzen

//                //var propertyUrl = Globals4Package.ApiBase + $"/metadata/{baseUrl}?&recursiveProperties=0&atomicProperties=0";
//                //var request = new RestRequest(propertyUrl, Method.Patch);
//                //request.AddStringBody(content, ContentType.Json);

//                // see https://jfrog.com/help/r/jfrog-rest-apis/set-item-properties
//                var propertyUrl = Globals.ApiBase + $"/storage/{baseUrl}?properties=";
//                foreach(var property in properties)
//                {
//                    var value = property.Value;
//                    value = value.Replace(",", $"%5C,")
//                        .Replace(@"\", @$"%5C\")
//                        .Replace("|", $"%5C|")
//                        .Replace("=", $"%5C=")
//                        .Replace(";", $"%5C;");

//                    propertyUrl += $"{property.Key}={value};";
//                }

//                propertyUrl = propertyUrl.Substring(0, propertyUrl.Length - 1);
//                propertyUrl += "&recursive=0";
//                var request = new RestRequest(propertyUrl, Method.Put);

//                var response = await client.ExecuteAsync(request, token);
//                if (response.StatusCode != HttpStatusCode.NoContent)
//                {
//                    throw new SdkException(response.Content);
//                }
//            }
//        }

//        private string SerializeProperties(Dictionary<string, string> properties)
//        {
//            if (properties != null && properties.Any())
//            {
//                dynamic expando = new ExpandoObject();
//                var data = (IDictionary<string, object>)expando;

//                foreach (var property in properties.Where(x => !string.IsNullOrEmpty(x.Value)))
//                {
//                    data.Add(property.Key, property.Value);
//                }

//                var options = JsonHelper.GetSerializerOptions();
//                return JsonSerializer.Serialize(new
//                {
//                    props = data
//                }, options);
//            }
//            return null;
//        }
//    }
//}
