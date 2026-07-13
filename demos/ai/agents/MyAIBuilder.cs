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

using System.ClientModel.Primitives;
using Microsoft.Extensions.AI;
using xSdk.Demos.AI.Tools;
using xSdk.Extensions.AI;
using xSdk.Extensions.Logging;
using xSdk.Plugins.AI;

namespace xSdk.Demos;

internal class MyAIBuilder() : AIBuilder
{
    public override void ConfigureBuilder()
    {
        var openAiClient = OpenAIHelper.CreateClient();
        var endpoint = "http://192.168.189.32:8000/v1";
        var apiKey = "sk-none";
        var model = "Qwen/Qwen2.5-VL-7B-Instruct-AWQ";

        CreateBuilder()
            // Generic Configs
            .WithLogging(LogManager.Factory)
            // Create a client
            .AddClient("openai", builder =>
            {
                builder
                    .UseOpenAIClient(options =>
                    {
                        options.EnableDistributedTracing = true;
                        options.ClientLoggingOptions = new System.ClientModel.Primitives.ClientLoggingOptions
                        {
                            EnableLogging = true,
                            EnableMessageContentLogging = true,
                            EnableMessageLogging = true
                        };
                    })
                    .WithApiKey(apiKey)
                    .WithEndpoint(endpoint);
            })
            // Add a Tool for getting weather information
            .AddTool("get_weather", builder =>
            {
                builder
                    .UseInProcess(WeatherTool.GetWeather);
            })
            // Add Agents
            .AddAgent("SimpleAgent", builder =>
            {
                builder
                    .UseClient("openai")
                    .WithFile("AI\\Agents\\Assistant.yaml");
            })
            .AddAgent("WeatherAgent", builder =>
            {
                builder
                    .UseClient("openai")
                    .UseTool("get_weather")
                    .WithFile("AI\\Agents\\GetWeather.yaml");
            });
    }
}
