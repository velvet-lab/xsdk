using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace xSdk.Data
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4004:Collection properties should be readonly", Justification = "<Pending>")]
    public class Dependencies
    {
        [JsonPropertyName("module")]
        public Dictionary<string, string> Module { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("nuget")]
        public Dictionary<string, string> NuGet { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("package")]
        public Dictionary<string, string> Package { get; set; } = new Dictionary<string, string>();
    }
}
