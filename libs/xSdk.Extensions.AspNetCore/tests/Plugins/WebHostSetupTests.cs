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

namespace xSdk.Hosting;

public class WebHostSetupTests
{
    [Fact]
    public void WebHostSetup_DefaultBind_HasDefaultValue()
    {
        var setup = new WebHostSetup();

        Assert.Equal(WebHostSetup.Definitions.Bind.DefaultValue, setup.Bind);
    }

    [Fact]
    public void WebHostSetup_DefaultHttp_HasDefaultValue()
    {
        var setup = new WebHostSetup();

        Assert.Equal(WebHostSetup.Definitions.Http.DefaultValue, setup.Http);
    }

    [Fact]
    public void WebHostSetup_DefaultHttps_HasDefaultValue()
    {
        var setup = new WebHostSetup();

        Assert.Equal(WebHostSetup.Definitions.Https.DefaultValue, setup.Https);
    }

    [Fact]
    public void WebHostSetup_DefaultGrpc_IsZero()
    {
        var setup = new WebHostSetup();

        Assert.Equal(0, setup.Grpc);
    }

    [Fact]
    public void WebHostSetup_DefaultTlsCertFile_IsEmpty()
    {
        var setup = new WebHostSetup();

        Assert.True(string.IsNullOrEmpty(setup.TlsCertFile));
    }

    [Fact]
    public void WebHostSetup_DefaultTlsKeyFile_IsEmpty()
    {
        var setup = new WebHostSetup();

        Assert.True(string.IsNullOrEmpty(setup.TlsKeyFile));
    }

    [Fact]
    public void WebHostSetup_SetBind_StoresValue()
    {
        var setup = new WebHostSetup();

        setup.Bind = "0.0.0.0";

        Assert.Equal("0.0.0.0", setup.Bind);
    }

    [Fact]
    public void WebHostSetup_SetHttp_StoresValue()
    {
        var setup = new WebHostSetup();

        setup.Http = 9090;

        Assert.Equal(9090, setup.Http);
    }

    [Fact]
    public void WebHostSetup_SetHttps_StoresValue()
    {
        var setup = new WebHostSetup();

        setup.Https = 9443;

        Assert.Equal(9443, setup.Https);
    }

    [Fact]
    public void WebHostSetup_SetGrpc_StoresValue()
    {
        var setup = new WebHostSetup();

        setup.Grpc = 5000;

        Assert.Equal(5000, setup.Grpc);
    }

    [Fact]
    public void WebHostSetup_SetTlsCertFile_StoresValue()
    {
        var setup = new WebHostSetup();

        setup.TlsCertFile = "/path/to/cert.pem";

        Assert.Equal("/path/to/cert.pem", setup.TlsCertFile);
    }

    [Fact]
    public void WebHostSetup_SetTlsKeyFile_StoresValue()
    {
        var setup = new WebHostSetup();

        setup.TlsKeyFile = "/path/to/key.pem";

        Assert.Equal("/path/to/key.pem", setup.TlsKeyFile);
    }

    [Fact]
    public void WebHostSetup_SetAllowSystemPorts_StoresValue()
    {
        var setup = new WebHostSetup();

        setup.AllowSystemPorts = true;

        Assert.True(setup.AllowSystemPorts);
    }

    [Fact]
    public void WebHostSetup_Definitions_BindName_IsCorrect()
    {
        Assert.Equal("bind", WebHostSetup.Definitions.Bind.Name);
    }

    [Fact]
    public void WebHostSetup_Definitions_HttpName_IsCorrect()
    {
        Assert.Equal("http", WebHostSetup.Definitions.Http.Name);
    }

    [Fact]
    public void WebHostSetup_Definitions_HttpsName_IsCorrect()
    {
        Assert.Equal("https", WebHostSetup.Definitions.Https.Name);
    }

    [Fact]
    public void WebHostSetup_Definitions_GrpcName_IsCorrect()
    {
        Assert.Equal("grpc", WebHostSetup.Definitions.Grpc.Name);
    }

    [Fact]
    public void WebHostSetup_Definitions_TlsCertFileName_IsCorrect()
    {
        Assert.Equal("tls-cert-file", WebHostSetup.Definitions.TlsCertFile.Name);
    }

    [Fact]
    public void WebHostSetup_Definitions_TlsKeyFileName_IsCorrect()
    {
        Assert.Equal("tls-key-file", WebHostSetup.Definitions.TlsKeyFile.Name);
    }
}
