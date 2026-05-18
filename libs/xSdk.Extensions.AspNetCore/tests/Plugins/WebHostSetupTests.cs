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
    public void WebHostSetup_DefaultGrpc_IsZero()
    {
        var setup = new WebHostOptions();

        Assert.Equal(0, setup.Grpc);
    }

    [Fact]
    public void WebHostSetup_DefaultTlsCertFile_IsEmpty()
    {
        var setup = new WebHostOptions();

        Assert.True(string.IsNullOrEmpty(setup.TlsCertFile));
    }

    [Fact]
    public void WebHostSetup_DefaultTlsKeyFile_IsEmpty()
    {
        var setup = new WebHostOptions();

        Assert.True(string.IsNullOrEmpty(setup.TlsKeyFile));
    }

    [Fact]
    public void WebHostSetup_SetBind_StoresValue()
    {
        var setup = new WebHostOptions();

        setup.Bind = "0.0.0.0";

        Assert.Equal("0.0.0.0", setup.Bind);
    }

    [Fact]
    public void WebHostSetup_SetHttp_StoresValue()
    {
        var setup = new WebHostOptions();

        setup.Http = 9090;

        Assert.Equal(9090, setup.Http);
    }

    [Fact]
    public void WebHostSetup_SetHttps_StoresValue()
    {
        var setup = new WebHostOptions();

        setup.Https = 9443;

        Assert.Equal(9443, setup.Https);
    }

    [Fact]
    public void WebHostSetup_SetGrpc_StoresValue()
    {
        var setup = new WebHostOptions();

        setup.Grpc = 5000;

        Assert.Equal(5000, setup.Grpc);
    }

    [Fact]
    public void WebHostSetup_SetTlsCertFile_StoresValue()
    {
        var setup = new WebHostOptions();

        setup.TlsCertFile = "/path/to/cert.pem";

        Assert.Equal("/path/to/cert.pem", setup.TlsCertFile);
    }

    [Fact]
    public void WebHostSetup_SetTlsKeyFile_StoresValue()
    {
        var setup = new WebHostOptions();

        setup.TlsKeyFile = "/path/to/key.pem";

        Assert.Equal("/path/to/key.pem", setup.TlsKeyFile);
    }

    [Fact]
    public void WebHostSetup_SetAllowSystemPorts_StoresValue()
    {
        var setup = new WebHostOptions();

        setup.AllowSystemPorts = true;

        Assert.True(setup.AllowSystemPorts);
    }

    [Fact]
    public void WebHostSetup_Definitions_BindName_IsCorrect()
    {
        Assert.Equal("bind", WebHostOptions.Definitions.Bind.Name);
    }

    [Fact]
    public void WebHostSetup_Definitions_HttpName_IsCorrect()
    {
        Assert.Equal("http", WebHostOptions.Definitions.Http.Name);
    }

    [Fact]
    public void WebHostSetup_Definitions_HttpsName_IsCorrect()
    {
        Assert.Equal("https", WebHostOptions.Definitions.Https.Name);
    }

    [Fact]
    public void WebHostSetup_Definitions_GrpcName_IsCorrect()
    {
        Assert.Equal("grpc", WebHostOptions.Definitions.Grpc.Name);
    }

    [Fact]
    public void WebHostSetup_Definitions_TlsCertFileName_IsCorrect()
    {
        Assert.Equal("tls-cert-file", WebHostOptions.Definitions.TlsCertFile.Name);
    }

    [Fact]
    public void WebHostSetup_Definitions_TlsKeyFileName_IsCorrect()
    {
        Assert.Equal("tls-key-file", WebHostOptions.Definitions.TlsKeyFile.Name);
    }
}
