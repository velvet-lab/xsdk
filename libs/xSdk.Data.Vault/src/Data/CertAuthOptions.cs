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

using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Logging;
using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;
using xSdk.Tools;

namespace xSdk.Data;

[VariablePrefix("vault-cert-auth")]
public class CertAuthOptions : VariableSetup
{
    private static ILogger Logger => field ??= LogManager.CreateLogger<CertAuthOptions>();

    [Variable(
        name: Definitions.Certificate.Name,
        template: Definitions.Certificate.Template,
        helpText: Definitions.Certificate.HelpText,
        hidden: true)]
    public string? Certificate
    {
        get => ReadValue<string>(Definitions.Certificate.Name);
        set => SetValue(Definitions.Certificate.Name, value);
    }

    [Variable(
        name: Definitions.Key.Name,
        template: Definitions.Key.Template,
        helpText: Definitions.Key.HelpText,
        hidden: true)]
    public string? Key
    {
        get => ReadValue<string>(Definitions.Key.Name);
        set => SetValue(Definitions.Key.Name, value);
    }

    internal X509Certificate2? CreateCertificate()
    {
        try
        {
            string? cert = Certificate;
            if (Base64Tools.IsBase64(cert))
            {
                Logger.LogInformation("Converting base64 encoded certificate to PEM format.");
                cert = Base64Tools.ConvertFromBase64(cert);
            }

            string? key = Key;
            if (Base64Tools.IsBase64(key))
            {
                Logger.LogInformation("Converting base64 encoded key to PEM format.");
                key = Base64Tools.ConvertFromBase64(key);
            }

            var certificate = X509Certificate2.CreateFromPem(cert, key);
            X509Certificate2 pkcsCertificate = X509CertificateLoader.LoadCertificate(certificate.Export(X509ContentType.Pkcs12));

            return pkcsCertificate;
        }
        catch
        {
            throw new SdkException("Failed to create certificate from PEM data. Please check the provided certificate and key.");
        }
    }

    private static class Definitions
    {
        public static class Certificate
        {
            public const string Name = "client_cert";
            public const string Template = $"--vault-client-cert <pem>";
            public const string HelpText = "Public certificate chain for cert based auth to access vault";
        }

        public static class Key
        {
            public const string Name = "client_key";
            public const string Template = $"--vault-client-key <token>";
            public const string HelpText = "Private certificate key for cert based auth to access vault";
        }
    }
}
