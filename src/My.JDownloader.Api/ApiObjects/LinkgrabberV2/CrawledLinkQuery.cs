namespace My.JDownloader.Api.ApiObjects.LinkgrabberV2
{
    public class CrawledLinkQuery
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "availability")]
        public bool Availability { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "bytesTotal")]
        public bool BytesTotal { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "comment")]
        public bool Comment { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "host")]
        public bool Host { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "jobUUIDs")]
        public long[] JobUuiDs { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "maxResults")]
        public int MaxResults { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "packageUUIDs")]
        public long[] PackageUuiDs { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "password")]
        public bool Password { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "priority")]
        public bool Priority { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "startAt")]
        public int StartAt { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "status")]
        public bool Status { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "url")]
        public bool Url { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "variantID")]
        public bool VariantId { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "variantIcon")]
        public bool VariantIcon { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "variantName")]
        public bool VariantName { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "variants")]
        public bool Variants { get; set; }
    }
}
