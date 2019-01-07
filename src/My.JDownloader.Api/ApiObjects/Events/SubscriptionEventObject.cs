using Newtonsoft.Json;

namespace My.JDownloader.Api.ApiObjects.Events
{
    public class SubscriptionEventObject
    {
        [JsonProperty(PropertyName = "eventid")]
        public string EventId { get; set; }
        [JsonProperty(PropertyName = "publisher")]
        public string Publisher { get; set; }
        [JsonProperty(PropertyName = "eventdata")]
        public string EventData { get; set; }
    }
}
