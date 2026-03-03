using System.Collections.Generic;
using System.Text.Json.Serialization;
using RestSharp;

namespace xSdk.Data
{
    public class ArtifactModelList
    {
        public ArtifactModelList()
        {
            Artifacts = new List<ArtifactModel>();
        }

        [JsonPropertyName("results")]
        public IList<ArtifactModel> Artifacts { get; set; }

        public RestResponse Response { get; set; }
    }
}
