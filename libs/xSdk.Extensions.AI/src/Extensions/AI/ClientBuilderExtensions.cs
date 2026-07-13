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

using OpenAI;

namespace xSdk.Extensions.AI;

public static class ClientBuilderExtensions
{
    extension(ClientBuilder builder)
    {
        public ClientBuilder WithName(string name)
        {
            builder.Name = name;
            return builder;
        }

        public ClientBuilder WithApiKey(string apiKey)
        {
            builder.ApiKey = apiKey;
            return builder;
        }

        public ClientBuilder WithEndpoint(string endpoint)
        {
            builder.Endpoint = endpoint;
            return builder;
        }

        public ClientBuilder UseOpenAIClient(Action<OpenAIClientOptions> configure)
        {
            builder.OpenAiClientOptionsFactory = configure;
            return builder;
        }
    }
}
