using Newtonsoft.Json;

namespace My.JDownloader.Api.ApiObjects.LinkgrabberV2
{
    public class QueryLinksResponseObject
    {
        [JsonProperty(PropertyName = "availableLinkState")]
        public AvailableLinkStateType AvailableLinkState { get; set; }
        [JsonProperty(PropertyName = "bytesTotal")]
        public long BytesTotal { get; set; }
        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }
        [JsonProperty(PropertyName = "downloadPassword")]
        public string DownloadPassword { get; set; }
        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
        [JsonProperty(PropertyName = "host")]
        public string Host { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "packageUUID")]
        public long PackageUuid { get; set; }
        [JsonProperty(PropertyName = "priority")]
        public Enums.PriorityType Priority { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "uuid")]
        public long Uuid { get; set; }
    }
}
