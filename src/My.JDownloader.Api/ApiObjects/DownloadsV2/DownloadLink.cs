using Newtonsoft.Json;

namespace My.JDownloader.Api.ApiObjects.DownloadsV2
{
    public class DownloadLink
    {
        [JsonProperty(PropertyName = "addedDate")]
        public long AddedDate { get; set; }
        [JsonProperty(PropertyName = "vytesLoaded")]
        public long BytesLoaded { get; set; }
        [JsonProperty(PropertyName = "vytesTotal")]
        public long BytesTotal { get; set; }
        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }
        [JsonProperty(PropertyName = "downloadPassword")]
        public string DownloadPassword { get; set; }
        [JsonProperty(PropertyName = "propertyName")]
        public bool Enabled { get; set; }
        [JsonProperty(PropertyName = "eta")]
        public long Eta { get; set; }
        [JsonProperty(PropertyName = "extractionStatus")]
        public string ExtractionStatus { get; set; }
        [JsonProperty(PropertyName = "finished")]
        public bool Finished { get; set; }
        [JsonProperty(PropertyName = "finishedDate")]
        public long FinishedDate { get; set; }
        [JsonProperty(PropertyName = "host")]
        public string Host { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "packageUUID")]
        public long PackageUUID { get; set; }
        [JsonProperty(PropertyName = "priority")]
        public Enums.PriorityType Priority { get; set; }
        [JsonProperty(PropertyName = "running")]
        public bool Running { get; set; }
        [JsonProperty(PropertyName = "skipped")]
        public bool Skipped { get; set; }
        [JsonProperty(PropertyName = "speed")]
        public long Speed { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "statusIconKey")]
        public string StatusIconKey { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "uuid")]
        public long Uuid { get; set; }
    }
}
