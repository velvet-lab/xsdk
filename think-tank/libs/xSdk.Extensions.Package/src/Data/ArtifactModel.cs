using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace xSdk.Data
{
    public class ArtifactModel
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("createdBy")]
        public string CreatedBy { get; set; }

        [JsonPropertyName("lastModified")]
        public DateTime LastModified { get; set; }

        [JsonPropertyName("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonPropertyName("lastUpdated")]
        public DateTime LastUpdated { get; set; }

        [JsonPropertyName("properties")]
        public IDictionary<string, string[]> Properties { get; set; }

        [JsonPropertyName("downLoadUri")]
        public string DownLoadUri { get; set; }

        [JsonPropertyName("mimeType")]
        public string MimeType { get; set; }

        [JsonPropertyName("size")]
        public string Size { get; set; }

        [JsonPropertyName("checksums")]
        public Dictionary<string, string> Checksums { get; set; }

        [JsonPropertyName("originalChecksums")]
        public Dictionary<string, string> OriginalChecksums { get; set; }

        [JsonPropertyName("uri")]
        public Uri Uri { get; set; }

        private bool IsEqual(ArtifactModel other)
        {
            return Uri == other.Uri;
        }

        /// <summary>
        /// Equality of the artifacts is equality of references
        /// or equality of URIs
        /// </summary>
        public bool Equals(ArtifactModel other)
        {
            return other != null && (ReferenceEquals(this, other) || IsEqual(other));
        }

        public override bool Equals(object obj)
        {
            return obj is ArtifactModel model && (ReferenceEquals(this, model) || IsEqual(model));
        }

        public override int GetHashCode()
        {
            return Uri.GetHashCode();
        }
    }
}
