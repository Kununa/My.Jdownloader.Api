namespace My.JDownloader.Api.ApiObjects.LinkgrabberV2
{
    public class CrawledPackage
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "bytesTotal")]
        public long BytesTotal { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "childCount")]
        public int ChildCount { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "downloadPassword")]
        public string DownloadPassword { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "hosts")]
        public string[] Hosts { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "offlineCount")]
        public int OfflineCount { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "onlineCount")]
        public int OnlineCount { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "priority")]
        public PriorityType Priority { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "saveTo")]
        public string SaveTo { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "tempUnknownCount")]
        public int TempUnknownCount { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "unkownCount")]
        public int UnkownCount { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "uuid")]
        public long Uuid { get; set; }
    }
}
