using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace xSdk.Data
{
    public class AutomationRuntime
    {
        [JsonPropertyName("constraints")]
        public Dictionary<string, string> Constraints { get; set; } = new Dictionary<string, string>();
    }
}
