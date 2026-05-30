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

namespace xSdk.Extensions.AI;

public class AgentsPluginOptionsTests
{
    [Fact]
    public void Endpoint_DefaultValue_IsOpenAiUrl()
    {
        var options = new AIPluginOptions();

        Assert.Equal(AIPluginOptions.Definitions.Endpoint.DefaultValue, options.Endpoint);
    }

    [Fact]
    public void Endpoint_SetValue_StoresValue()
    {
        var options = new AIPluginOptions();

        options.Endpoint = "https://custom.openai.example.com/v1";

        Assert.Equal("https://custom.openai.example.com/v1", options.Endpoint);
    }

    [Fact]
    public void ApiKey_DefaultValue_IsNull()
    {
        var options = new AIPluginOptions();

        Assert.Null(options.ApiKey);
    }

    [Fact]
    public void ApiKey_SetValue_StoresValue()
    {
        var options = new AIPluginOptions();

        options.ApiKey = "sk-test-key-12345";

        Assert.Equal("sk-test-key-12345", options.ApiKey);
    }

    [Fact]
    public void Definitions_Endpoint_Name_IsCorrect()
    {
        Assert.Equal("endpoint", AIPluginOptions.Definitions.Endpoint.Name);
    }

    [Fact]
    public void Definitions_Endpoint_DefaultValue_IsOpenAiBaseUrl()
    {
        Assert.Equal("https://api.openai.com/v1", AIPluginOptions.Definitions.Endpoint.DefaultValue);
    }

    [Fact]
    public void Definitions_Endpoint_Template_ContainsEndpoint()
    {
        Assert.Contains("endpoint", AIPluginOptions.Definitions.Endpoint.Template);
    }

    [Fact]
    public void Definitions_ApiKey_Name_IsCorrect()
    {
        Assert.Equal("apikey", AIPluginOptions.Definitions.ApiKey.Name);
    }

    [Fact]
    public void Definitions_ApiKey_Template_ContainsApikey()
    {
        Assert.Contains("apikey", AIPluginOptions.Definitions.ApiKey.Template);
    }

    [Fact]
    public void Definitions_Endpoint_HelpText_IsNotEmpty()
    {
        Assert.False(string.IsNullOrEmpty(AIPluginOptions.Definitions.Endpoint.HelpText));
    }

    [Fact]
    public void Definitions_ApiKey_HelpText_IsNotEmpty()
    {
        Assert.False(string.IsNullOrEmpty(AIPluginOptions.Definitions.ApiKey.HelpText));
    }
}

