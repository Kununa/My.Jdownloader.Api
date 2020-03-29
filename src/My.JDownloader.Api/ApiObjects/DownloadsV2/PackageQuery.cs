namespace My.JDownloader.Api.ApiObjects.DownloadsV2
{
    public class PackageQuery
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "bytesLoaded")]
        public bool BytesLoaded { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "bytesTotal")]
        public bool BytesTotal { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "childCount")]
        public bool ChildCount { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "comment")]
        public bool Comment { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "eta")]
        public bool Eta { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "finished")]
        public bool Finished { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "hosts")]
        public bool Hosts { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "maxResults")]
        private uint MaxResults { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "packageUUIDs")]
        public long[] PackageUuiDs { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "priority")]
        public bool Priority { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "running")]
        public bool Running { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "saveTo")]
        public bool SaveTo { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "speed")]
        public bool Speed { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "startAt")]
        public int StartAt { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "status")]
        public bool Status { get; set; }

        public PackageQuery(uint maxResults = 100)
        {
            MaxResults = maxResults;
        }
    }
}
