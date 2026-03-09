namespace xSdk.Security.Claims;

public sealed class ClaimModel
{
    // This class exists only for serialization purposes.
    // Original Claim Class is not serializable, because no public constructor exists.

    public string Issuer { get; set; } = string.Empty;

    public string OriginalIssuer { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string ValueType { get; set; } = string.Empty;
}
