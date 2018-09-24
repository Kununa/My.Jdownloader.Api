namespace My.JDownloader.Api.ApiObjects.Toolbar
{
    public class StatusObject
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "running")]
        public bool Running { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "reconnect")]
        public bool Reconnect { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "premium")]
        public bool Premium { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "download_complete")]
        public long Download_complete { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "stopafter")]
        public bool Stopafter { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "limit")]
        public bool Limit { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "limitspeed")]
        public long Limitspeed { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "download_current")]
        public long Download_current { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "clipboard")]
        public bool Clipboard { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "speed")]
        public long Speed { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "pause")]
        public bool Pause { get; set; }
    }
}
