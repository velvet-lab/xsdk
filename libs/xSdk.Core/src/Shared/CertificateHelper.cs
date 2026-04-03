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

using System.Diagnostics;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Extensions.Logging;
using xSdk;
using xSdk.Hosting;

namespace xSdk.Shared;

public static class CertificateHelper
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public static IEnumerable<X509Certificate> ImportFromString(string certificates)
    {
        var certs = new List<X509Certificate>();

        var splittedCertStrings = certificates.Split("-----END CERTIFICATE-----", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        foreach (var certString in splittedCertStrings)
        {
            var item = certString.Replace("-----BEGIN CERTIFICATE-----", null).Replace("-----END CERTIFICATE-----", null).Trim();

            var cert = new X509Certificate2(Encoding.ASCII.GetBytes(item));
            certs.Add(cert);
        }

        return certs;
    }

    public static bool ValidateServerCallbacks(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
    {
        if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors)
        {
            if (chain != null)
            {
                foreach (var status in chain.ChainStatus)
                {
                    //validation errors here
                    _logger.LogError("{StatusInformation}", status.StatusInformation);
                }
            }
        }

        if (Debugger.IsAttached)
        {
            if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateNameMismatch)
            {
                // Server is running on localhost so external Certificate Server Names does not match
                return true;
            }
        }
        return sslPolicyErrors == SslPolicyErrors.None;
    }
}
