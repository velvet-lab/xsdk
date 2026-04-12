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

namespace xSdk.Tools;

public static class NetworkTools
{
    public static int GetFreePort(int min = 1024, int max = 65535)
    {
        var random = new Random();

        int port = -1;

        var found = false;
        while (!found)
        {
            port = random.Next(min, max);

            var result = IsPortFree(port);
            if (result)
                found = true;
        }

        return port;
    }

    public static bool IsPortFree(int port)
    {
        var props = IPGlobalProperties.GetIPGlobalProperties();
        var tcpCons = props.GetActiveTcpListeners();
        var udpCons = props.GetActiveUdpListeners();

        var result = tcpCons.Any(x => x.Port == port);
        if (!result)
            result = udpCons.Any(x => x.Port == port);

        return !result;
    }

    public static bool IsLocalIpAddress(string ip) => IsLocalhost(ip);

    public static bool IsLocalhost(string host)
    {
        var possibleHosts = new string[]
        {
            Environment.MachineName,
            $"{Environment.MachineName}.{Environment.UserDomainName}",
            "localhost", // DevSkim: ignore DS162092
            "127.0.0.1", // DevSkim: ignore DS162092
            "::1", // DevSkim: ignore DS162092
        };

        foreach (var possibleHost in possibleHosts)
        {
            if (string.Compare(host, possibleHost, true) == 0)
                return true;
        }

        return false;
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new SdkException("No network adapters with an IPv4 address in the system!");
    }

    public static string GetMacAddress()
    {
        return NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(x => x.OperationalStatus == OperationalStatus.Up && x.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .Select(x => x.GetPhysicalAddress().ToString())
            .FirstOrDefault();
    }
}
