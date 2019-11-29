using Newtonsoft.Json;

namespace My.JDownloader.Api.ApiObjects.DownloadsV2
{
    class CleanupQuery
    {
        [JsonProperty(PropertyName = "linkIds")]
        public long[] LinkIds { get; set; }
        [JsonProperty(PropertyName = "packageIds")]
        public long[] PackageIds { get; set; }
        [JsonProperty(PropertyName = "action")]
        public string Action { get; set; }
        [JsonProperty(PropertyName = "mode")]
        public string Mode { get; set; }
        [JsonProperty(PropertyName = "selectionType")]
        public string SelectionType { get; set; }
    }
}
