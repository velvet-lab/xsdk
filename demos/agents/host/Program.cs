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

//[assembly: ApiController]
//[assembly: ApiConventionType(typeof(DefaultApiConventions))]

//const string APP_NAME = "webapi";
//const string APP_COMPANY = "xdemos";
//const string APP_PREFIX = "webapi";

//IHost host = xSdk.Hosting.WebHost
//    .CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX)
//    .EnableWebApi()
//    // .EnableDocumentation<DocumentationPluginBuilder>()
//    // .EnableWebSecurity()
//    // .EnableAuthentication<AuthenticationPluginBuilder>()
//    // .EnableCompression()
//    // .EnableDataProtection()    
//    .Build();

//ILogger logger = LogManager.GetCurrentClassLogger();
//logger.LogInformation("Starting {AppName}", APP_NAME);

//await host.RunAsync();



using System.ClientModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Responses;
using xSdk.Demos.Skills;
using xSdk.Demos.Tools;


#pragma warning disable MAAI001 // Der Typ dient nur zu Testzwecken und kann in zukünftigen Aktualisierungen geändert oder entfernt werden. Unterdrücken Sie diese Diagnose, um fortzufahren.

var client = new OpenAIClient(new ApiKeyCredential("OpenApiKey"), new OpenAIClientOptions
{
    Endpoint = new Uri("http://192.168.189.32:11434/v1"),
});

IList<AITool> tools = [
    AIFunctionFactory.Create(WeatherTool.GetWeather)
];


AgentSkillsProvider skillProvider = new(new WeatherExpertSkill());


var chatClient = client.GetChatClient("gemello");
var agent = chatClient
    .AsAIAgent(new ChatClientAgentOptions
    {
        Name = "Assitant",
        ChatOptions = new()
        {
            Instructions = "You are a helpful assistant.",
            // Tools = [AIFunctionFactory.Create(WeatherTool.GetWeather)]
        },
        AIContextProviders = [skillProvider]
    });    

Console.WriteLine(await agent.RunAsync("Was ist heute das Wetter in Frankreich?"));

#pragma warning restore MAAI001 // Der Typ dient nur zu Testzwecken und kann in zukünftigen Aktualisierungen geändert oder entfernt werden. Unterdrücken Sie diese Diagnose, um fortzufahren.
