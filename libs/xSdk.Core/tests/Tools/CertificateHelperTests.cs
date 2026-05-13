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

using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace xSdk.Tools;

public class CertificateHelperTests
{
    [Fact]
    public void ValidateServerCallbacks_WithNoError_ReturnsTrue()
    {
        var result = CertificateHelper.ValidateServerCallbacks(
            this,
            null,
            null,
            SslPolicyErrors.None);

        Assert.True(result);
    }

    [Fact]
    public void ValidateServerCallbacks_WithCertificateNotAvailable_ReturnsFalse()
    {
        var result = CertificateHelper.ValidateServerCallbacks(
            this,
            null,
            null,
            SslPolicyErrors.RemoteCertificateNotAvailable);

        Assert.False(result);
    }

    [Fact]
    public void ValidateServerCallbacks_WithChainErrors_AndNullChain_ReturnsFalse()
    {
        var result = CertificateHelper.ValidateServerCallbacks(
            this,
            null,
            null,
            SslPolicyErrors.RemoteCertificateChainErrors);

        Assert.False(result);
    }

    [Fact]
    public void ValidateServerCallbacks_WithChainErrors_AndEmptyChain_ReturnsFalse()
    {
        using var chain = new X509Chain();

        var result = CertificateHelper.ValidateServerCallbacks(
            this,
            null,
            chain,
            SslPolicyErrors.RemoteCertificateChainErrors);

        Assert.False(result);
    }
}
