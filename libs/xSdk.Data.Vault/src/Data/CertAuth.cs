using System.Security.Cryptography.X509Certificates;

namespace xSdk.Data
{
    public sealed class CertAuth
    {
        public X509Certificate2? Certificate { get; set; }

        public string? RoleName { get; set; }
    }
}
