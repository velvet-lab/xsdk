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

using System.Threading;
using System.Threading.Tasks;

namespace xSdk.Extensions.Consul;

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
