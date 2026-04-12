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

namespace xSdk.Tools;

public class NetworkToolsTests
{
    [Theory]
    [InlineData("localhost")]
    [InlineData("127.0.0.1")]
    [InlineData("::1")]
    public void IsLocalhost_WithLocalhostAddress_ReturnsTrue(string host)
    {
        var result = NetworkTools.IsLocalhost(host);

        Assert.True(result);
    }

    [Theory]
    [InlineData("example.com")]
    [InlineData("192.168.1.100")]
    [InlineData("8.8.8.8")]
    public void IsLocalhost_WithRemoteAddress_ReturnsFalse(string host)
    {
        var result = NetworkTools.IsLocalhost(host);

        Assert.False(result);
    }

    [Fact]
    public void IsLocalhost_WithMachineName_ReturnsTrue()
    {
        var machineName = Environment.MachineName;

        var result = NetworkTools.IsLocalhost(machineName);

        Assert.True(result);
    }

    [Theory]
    [InlineData("localhost")]
    [InlineData("127.0.0.1")]
    public void IsLocalIpAddress_WithLocalhostAddress_ReturnsTrue(string ip)
    {
        var result = NetworkTools.IsLocalIpAddress(ip);

        Assert.True(result);
    }

    [Fact]
    public void IsPortFree_WithNonOccupiedPort_ReturnsBoolean()
    {
        var result = NetworkTools.IsPortFree(65000);

        Assert.IsType<bool>(result);
    }

    [Fact]
    public void GetFreePort_WithDefaultRange_ReturnsPortInRange()
    {
        var port = NetworkTools.GetFreePort();

        Assert.InRange(port, 1024, 65535);
    }

    [Fact]
    public void GetFreePort_WithCustomRange_ReturnsPortInRange()
    {
        const int min = 50000;
        const int max = 55000;

        var port = NetworkTools.GetFreePort(min, max);

        Assert.InRange(port, min, max);
    }

    [Fact]
    public void GetLocalIPAddress_ReturnsValidIpAddress()
    {
        var ip = NetworkTools.GetLocalIPAddress();

        Assert.NotNull(ip);
        Assert.NotEmpty(ip);
    }
}
