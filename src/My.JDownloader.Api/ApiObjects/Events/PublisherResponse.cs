using Newtonsoft.Json;

namespace My.JDownloader.Api.ApiObjects.Events
{
    public class PublisherResponse
    {
        [JsonProperty(PropertyName = "eventids")]
        public string[] EventIds { get; set; }
        [JsonProperty(PropertyName = "publisher")]
        public string Publisher { get; set; }
    }
}
