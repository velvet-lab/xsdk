using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace xSdk.Data
{
    public class AutomationHub
    {
        [JsonPropertyName("dependencies")]
        public Dependencies Dependencies { get; set; } = new Dependencies();

        [JsonPropertyName("runtime")]
        public AutomationRuntime Runtime { get; set; } = new AutomationRuntime();
    }
}
