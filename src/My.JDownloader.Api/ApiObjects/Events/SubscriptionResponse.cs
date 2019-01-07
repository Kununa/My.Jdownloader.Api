using Newtonsoft.Json;

namespace My.JDownloader.Api.ApiObjects.Events
{
    public class SubscriptionResponse
    {
        [JsonProperty(PropertyName = "exclusions")]
        public string[] Exclusions { get; set; }
        [JsonProperty(PropertyName = "maxKeepalive")]
        public long MaxKeepalive { get; set; }
        [JsonProperty(PropertyName = "maxPolltimeout")]
        public long MaxPolltimeout { get; set; }
        [JsonProperty(PropertyName = "subscribed")]
        public bool Subscribed { get; set; }
        [JsonProperty(PropertyName = "subscriptionid")]
        public long SubscriptionId { get; set; }
        [JsonProperty(PropertyName = "subscriptions")]
        public string[] Subscriptions { get; set; }
    }
}
