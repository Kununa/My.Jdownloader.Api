namespace My.JDownloader.Api.ApiObjects.LinkgrabberV2
{
    public class CrawledPackageQuery
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "availableOfflineCount")]
        public bool AvailableOfflineCount { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "availableOnlineCount")]
        public bool AvailableOnlineCount { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "availableTempUnknownCount")]
        public bool AvailableTempUnknownCount { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "availableUnkownCount")]
        public bool AvailableUnkownCount { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "bytesTotal")]
        public bool BytesTotal { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "childCount")]
        public bool ChildCount { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "comment")]
        public bool Comment { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "hosts")]
        public bool Hosts { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "maxResults")]
        public int MaxResults { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "packageUuids")]
        public long[] PackageUuids { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "priority")]
        public bool Priority { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "saveTo")]
        public bool SaveTo { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "startAt")]
        public int StartAt { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "status")]
        public bool Status { get; set; }
    }
}
