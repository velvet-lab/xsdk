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

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace xSdk.Tools;

public static class NetworkTools
{
    public static int GetFreePort(int min = 1024, int max = 65535)
    {
        int port = -1;

        bool found = false;
        while (!found)
        {
            port = RandomNumberGenerator.GetInt32(min, max);

            bool result = IsPortFree(port);
            if (result)
            {
                found = true;
            }
        }

        return port;
    }

    public static bool IsPortFree(int port)
    {
        var props = IPGlobalProperties.GetIPGlobalProperties();
        IPEndPoint[] tcpCons = props.GetActiveTcpListeners();
        IPEndPoint[] udpCons = props.GetActiveUdpListeners();

        bool result = tcpCons.Any(x => x.Port == port);
        if (!result)
        {
            result = udpCons.Any(x => x.Port == port);
        }

        return !result;
    }

    public static bool IsLocalIpAddress(string ip) => IsLocalhost(ip);

    public static bool IsLocalhost(string host)
    {
        string[] possibleHosts =
        [
            Environment.MachineName,
            $"{Environment.MachineName}.{Environment.UserDomainName}",
            "localhost", // DevSkim: ignore DS162092
            "127.0.0.1", // DevSkim: ignore DS162092
            "::1", // DevSkim: ignore DS162092
        ];

        return possibleHosts.Any(x => string.Equals(x, host, StringComparison.OrdinalIgnoreCase));
    }

    public static string GetLocalIPAddress()
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new SdkException("No network adapters with an IPv4 address in the system!");
    }

    public static string? GetMacAddress()
    {
        return NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(x => x.OperationalStatus == OperationalStatus.Up && x.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .Select(x => x.GetPhysicalAddress().ToString())
            .FirstOrDefault();
    }
}
