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
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xSdk.Extensions.IO;

namespace xSdk.Hosting;

public static partial class WebHost
{
    private static void ConfigureKestrel(KestrelServerOptions options, ILogger logger)
    {
        WebHostOptions? webSetup = options.ApplicationServices.GetService<IOptions<WebHostOptions>>()?.Value;
        IFileSystemService? fileService = options.ApplicationServices.GetService<IFileSystemService>();
        bool certAvailable = false;

        if (webSetup == null)
        {
            logger.LogDebug("No WebHostOptions found, using default Kestrel configuration");
        }
        else
        {
            int httpPort = webSetup.Http;
            int grpcPort = webSetup.Grpc;

            // Remove Kestrel Header for security reasons
            options.AddServerHeader = false;

            if (TryLoadCertificateIfHttpsIsEnabled(fileService, webSetup, logger, out X509Certificate2? cert))
            {
                certAvailable = true;
                httpPort = webSetup.Https;

                options.ConfigureHttpsDefaults(_ =>
                {
                    _.ClientCertificateMode = ClientCertificateMode.NoCertificate;
                    if (Debugger.IsAttached)
                    {
                        _.CheckCertificateRevocation = false;
                    }

                    _.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13; // DevSkim: ignore DS440001,DS440020,DS112836
                    _.ServerCertificate = cert;
                });

                options.ConfigureEndpointDefaults(_ => _.UseHttps());
            }

            if (httpPort <= 1024 && !webSetup.AllowSystemPorts)
            {
                throw new SdkException(string.Format("Port '{0}' is not allowed", httpPort));
            }

            if (httpPort == grpcPort)
            {
                throw new SdkException("Http Port must be different to gRpc Port");
            }

            HttpProtocols protocols = HttpProtocols.Http1;
            if (certAvailable)
            {
                protocols = HttpProtocols.Http1AndHttp2;
            }

            if (httpPort > 1024 || webSetup.AllowSystemPorts)
            {
                if (httpPort > 0)
                {
                    if (string.Compare(webSetup.Bind, "localhost", true) == 0) // DevSkim: ignore DS162092
                    {
                        options.ListenLocalhost(httpPort, setup => setup.Protocols = protocols);
                    }
                    else
                    {
                        options.ListenAnyIP(httpPort, setup => setup.Protocols = protocols);
                    }
                }
                else
                {
                    logger.LogDebug("Http Port is not set");
                }
            }

            if (grpcPort > 1024 || webSetup.AllowSystemPorts)
            {
                if (grpcPort > 0)
                {
                    if (certAvailable)
                    {
                        if (string.Compare(webSetup.Bind, "localhost", true) == 0) // DevSkim: ignore DS162092
                        {
                            options.ListenLocalhost(grpcPort, setup => setup.Protocols = HttpProtocols.Http2);
                        }
                        else
                        {
                            options.ListenAnyIP(grpcPort, setup => setup.Protocols = HttpProtocols.Http2);
                        }
                    }
                    else
                    {
                        logger.LogError("Https configuration is needed for gRpc");
                    }
                }
                else
                {
                    logger.LogDebug("gRpc Port is not set");
                }
            }
        }
    }

    private static bool TryLoadCertificateIfHttpsIsEnabled(IFileSystemService? fileService, WebHostOptions? webSetup, ILogger logger, out X509Certificate2? cert)
    {
        if (webSetup != null && webSetup.IsHttpsEnabled)
        {
            if (fileService == null)
            {
                cert = null;
                return false;
            }

            string certLocation = fileService.Machine.Data.GetFullPath("/certs");
            if (Debugger.IsAttached)
            {
                certLocation = Environment.CurrentDirectory;
            }

            string? tlsCertFile = webSetup.TlsCertFile;
            string? tlsKeyFile = webSetup.TlsKeyFile;

            if (!string.IsNullOrEmpty(tlsCertFile) && !string.IsNullOrEmpty(tlsKeyFile))
            {
                string certFilePath = Path.Combine(certLocation, tlsCertFile);
                if (File.Exists(certFilePath))
                {
                    string keyFilePath = Path.Combine(certLocation, tlsKeyFile);
                    if (File.Exists(keyFilePath))
                    {
                        try
                        {
                            logger.LogInformation("Load Certificate from certificate and key File");
                            var innerCert = X509Certificate2.CreateFromPemFile(certFilePath, keyFilePath);
                            cert = X509CertificateLoader.LoadCertificate(innerCert.Export(X509ContentType.Pkcs12));

                            return true;
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Certificate could not created from certificate- and key File. (Reason: {message})", ex.Message);
                        }
                    }
                    else
                    {
                        logger.LogWarning("Https is enabled, but no key file '{file}' could not found", keyFilePath);
                    }
                }
                else
                {
                    logger.LogWarning("Https is enabled, but no certificate file '{file}' could not found", certFilePath);
                }
            }
            else
            {
                logger.LogWarning("Https is enabled, but no certificate and key file specified");
            }
        }

        cert = null;
        return false;
    }
}
