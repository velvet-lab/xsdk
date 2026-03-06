using NLog;
using System.Diagnostics;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace xSdk.Shared
{
    public static class CertificateHelper
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
                        logger.Error(status.StatusInformation);
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
}
