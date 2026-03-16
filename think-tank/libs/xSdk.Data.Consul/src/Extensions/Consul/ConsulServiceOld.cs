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

//using Consul;
//using xSdk.Configuration;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Net;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace xSdk.Services
//{
//    internal sealed class ConsulService : IConsulService
//    {
//        private readonly IConfiguration _config;
//        private readonly ILogger<ConsulService> _logger;

//        public ConsulService(IConfiguration config, ILogger<ConsulService> logger)
//        {
//            this._config = config ?? throw new ArgumentNullException(nameof(config));
//            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
//        }

//        public void RegisterService(string accesstoken, string name)
//            => RegisterServiceAsync(accesstoken, name, null, -1, null).GetAwaiter().GetResult();

//        public void RegisterService(string accesstoken, string name, string address, int port)
//            => RegisterServiceAsync(accesstoken, name, address, port, null).GetAwaiter().GetResult();

//        public void RegisterService(string accesstoken, string name, string address, int port, string[] tags)
//            => RegisterServiceAsync(accesstoken, name, address, port, tags).GetAwaiter().GetResult();

//        public Task RegisterServiceAsync(string accesstoken, string name, CancellationToken token = default)
//            => RegisterServiceAsync(accesstoken, name, null, -1, null, token);

//        public Task RegisterServiceAsync(string accesstoken, string name, string address, int port, CancellationToken token = default)
//            => RegisterServiceAsync(accesstoken, name, address, port, null, token);

//        public async Task RegisterServiceAsync(string accesstoken, string name, string address, int port, string[] tags, CancellationToken token = default)
//        {
//            var server = _config.GetConsulServer();
//            using (var client = new ConsulClient(setup =>
//            {
//                setup.Address = new Uri($"https://{server}");
//                setup.Token = accesstoken;
//            }))
//            {
//                try
//                {
//                    if (string.IsNullOrEmpty(address))
//                        address = GetCurrentIPAddess();

//                    if (port == -1)
//                        port = 443;

//                    var portAsString = $":{port}";
//                    if (port == 443)
//                        portAsString = "";

//                    _logger.LogInformation("Register new Service");
//                    var registration = new AgentServiceRegistration
//                    {
//                        ID = $"id-{name}",
//                        Name = name,
//                        Address = address,
//                        Port = port,
//                        Tags = tags,
//                        Checks = new AgentServiceCheck[]
//                        {
//                            new AgentServiceCheck{
//                                HTTP = $"https://{address}{portAsString}/health",
//                                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(10),
//                                Interval = TimeSpan.FromSeconds(30),
//                                Timeout = TimeSpan.FromSeconds(20),
//                                Status = HealthStatus.Passing,
//                                TLSSkipVerify = true
//                            }
//                        }
//                    };

//                    var response = await client.Agent.ServiceRegister(registration, token);
//                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
//                        throw new AminOOException($"Service '{name}' could not registered");
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogCritical(ex, "A Error occured while Service will registered");
//                    throw;
//                }
//            }
//        }

//        public string GetKeyValue(string accesstoken, string key)
//            => GetKeyValueAsync(accesstoken, key).GetAwaiter().GetResult();

//        public async Task<string> GetKeyValueAsync(string accesstoken, string key, CancellationToken token = default)
//        {
//            var server = _config.GetConsulServer();
//            using (var client = new ConsulClient(setup =>
//            {
//                setup.Address = new Uri($"https://{server}");
//                setup.Token = accesstoken;
//            }))
//            {
//                try
//                {
//                    _logger.LogInformation("Try to read Key '{0}' from Store", key);
//                    var queryResult = await client.KV.Get(key, token);

//                    if (queryResult != null)
//                        return Encoding.UTF8.GetString(queryResult.Response.Value, 0, queryResult.Response.Value.Length);

//                    return null;
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogCritical(ex, $"A Error occured while Key '{key}' will readed'");
//                    throw;
//                }
//            }
//        }

//        private static string GetCurrentIPAddess()
//        {
//            var hostName = Dns.GetHostName();

//            IPHostEntry ipHostEntry = Dns.GetHostEntry(hostName);
//            IPAddress[] address = ipHostEntry.AddressList;

//            return address[4].ToString();
//        }
//    }
//}
