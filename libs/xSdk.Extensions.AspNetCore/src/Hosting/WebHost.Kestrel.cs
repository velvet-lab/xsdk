using System.Diagnostics;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.IO;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public static partial class WebHost
{
    private static void ConfigureKestrel(KestrelServerOptions options)
    {
        var webSetup = options.ApplicationServices.GetRequiredService<IVariableService>().GetSetup<WebHostSetup>();
        var certAvailable = false;

        var httpPort = webSetup.Http;
        var grpcPort = webSetup.Grpc;

        // Remove Kestrel Header for security reasons
        options.AddServerHeader = false;

        if (TryLoadCertificateIfHttpsIsEnabled(webSetup, out X509Certificate2 cert))
        {
            certAvailable = true;
            httpPort = webSetup.Https;

            options.ConfigureHttpsDefaults(_ =>
            {
                _.ClientCertificateMode = ClientCertificateMode.NoCertificate;
                if (Debugger.IsAttached)
                    _.CheckCertificateRevocation = false;

                _.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13; // DevSkim: ignore DS440001,DS440020,DS112836
                _.ServerCertificate = cert;
            });

            options.ConfigureEndpointDefaults(_ =>
            {
                _.UseHttps();
            });
        }

        if (httpPort <= 1024 && !webSetup.AllowSystemPorts)
            throw new SdkException($"Port '{httpPort}' is not allowed");

        if (httpPort == grpcPort)
        {
            throw new SdkException($"Http Port must be different to gRpc Port");
        }

        var protocols = HttpProtocols.Http1;
        if (certAvailable)
        {
            protocols = HttpProtocols.Http1AndHttp2;
        }

        if (httpPort > 1024 || webSetup.AllowSystemPorts)
        {
            if (httpPort > 0)
            {
                if (string.Compare(webSetup.Bind, "localhost", true) == 0) // DevSkim: ignore DS162092
                    options.ListenLocalhost(httpPort, setup => setup.Protocols = protocols);
                else
                    options.ListenAnyIP(httpPort, setup => setup.Protocols = protocols);
            }
            else
            {
                _logger.Debug("Http Port is not set");
            }
        }

        if (grpcPort > 1024 || webSetup.AllowSystemPorts)
        {
            if (grpcPort > 0)
            {
                if (certAvailable)
                {
                    if (string.Compare(webSetup.Bind, "localhost", true) == 0) // DevSkim: ignore DS162092
                        options.ListenLocalhost(grpcPort, setup => setup.Protocols = HttpProtocols.Http2);
                    else
                        options.ListenAnyIP(grpcPort, setup => setup.Protocols = HttpProtocols.Http2);
                }
                else
                {
                    _logger.Error("Https configuration is needed for gRpc");
                }
            }
            else
            {
                _logger.Debug("gRpc Port is not set");
            }
        }
    }

    private static bool TryLoadCertificateIfHttpsIsEnabled(WebHostSetup webSetup, out X509Certificate2? cert)
    {
        if (webSetup.IsHttpsEnabled)
        {
            var certLocation = SlimHost.Instance.FileSystem.Machine.Data.GetFullPath("/certs");
            if (Debugger.IsAttached)
                certLocation = Environment.CurrentDirectory;

            var certFilePath = Path.Combine(certLocation, webSetup.TlsCertFile);
            if (File.Exists(certFilePath))
            {
                var keyFilePath = Path.Combine(certLocation, webSetup.TlsKeyFile);
                if (File.Exists(keyFilePath))
                {
                    try
                    {
                        _logger.Info("Load Certificate from certificate and key File");
                        var innerCert = X509Certificate2.CreateFromPemFile(certFilePath, keyFilePath);
                        // cert = X509CertificateLoader.LoadCertificate(innerCert.Export(X509ContentType.Pkcs12));
                        cert = new X509Certificate2(innerCert.Export(X509ContentType.Pkcs12));

                        return true;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Certificate could not created from certificate- and key File. (Reason: {0})", ex.Message);
                    }
                }
                else
                    _logger.Warn("Https is enabled, but no key file '{0}' could not found", keyFilePath);
            }
            else
                _logger.Warn("Https is enabled, but no certificate file '{0}' could not found", certFilePath);
        }

        cert = null;
        return false;
    }
}
