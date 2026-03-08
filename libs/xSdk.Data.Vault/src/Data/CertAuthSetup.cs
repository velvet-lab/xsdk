using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using NLog;
using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;
using xSdk.Shared;

namespace xSdk.Data;

[VariablePrefix("CertAuth")]
public class CertAuthSetup : Setup
{
    private readonly Logger logger = LogManager.GetCurrentClassLogger();

    [Variable(
        name: Definitions.Certificate.Name,
        template: Definitions.Certificate.Template,
        helpText: Definitions.Certificate.HelpText,
        hidden: true)]
    [JsonPropertyName(Definitions.Certificate.Name)]
    public string Certificate
    {
        get => ReadValue<string>(Definitions.Certificate.Name);
        set => SetValue(Definitions.Certificate.Name, value);
    }

    [Variable(
        name: Definitions.Key.Name,
        template: Definitions.Key.Template,
        helpText: Definitions.Key.HelpText,
        hidden: true)]
    [JsonPropertyName(Definitions.Key.Name)]
    public string Key
    {
        get => ReadValue<string>(Definitions.Key.Name);
        set => SetValue(Definitions.Key.Name, value);
    }

    protected override void ValidateSetup()
    {
        this.ValidateMember(x => string.IsNullOrEmpty(x.Certificate), "Client certificate for vault certificate auth is missing", Definitions.Certificate.Name);
        this.ValidateMember(x => string.IsNullOrEmpty(x.Key), "Client certificate key for vault certificate auth is missing", Definitions.Key.Name);
    }

    public X509Certificate2? CreateCertificate()
    {
        try
        {
            var cert = this.Certificate;
            if (Base64Helper.IsBase64(cert))
            {
                logger.Info("Converting base64 encoded certificate to PEM format.");
                cert = Base64Helper.ConvertFromBase64(cert);
            }

            var key = this.Key;
            if (Base64Helper.IsBase64(key))
            {
                logger.Info("Converting base64 encoded key to PEM format.");
                key = Base64Helper.ConvertFromBase64(key);
            }

            var certificate = X509Certificate2.CreateFromPem(cert, key);
            var pkcsCertificate = new X509Certificate2(certificate.Export(X509ContentType.Pkcs12));

            return pkcsCertificate;
        }
        catch
        {
            throw new SdkException("Failed to create certificate from PEM data. Please check the provided certificate and key.");
        }
    }

    private class Definitions
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
