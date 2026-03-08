namespace xSdk.Security.Claims;

public sealed class ClaimModel
{
    // This class exists only for serialization purposes.
    // Original Claim Class is not serializable, because no public constructor exists.

    public string Issuer { get; set; }

    public string OriginalIssuer { get; set; }

    public string Type { get; set; }

    public string Value { get; set; }

    public string ValueType { get; set; }
}
