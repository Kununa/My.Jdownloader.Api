using Newtonsoft.Json;

namespace My.JDownloader.Api.ApiObjects.Events
{
    public class PublisherResponse
    {
        [JsonProperty(PropertyName = "eventids")]
        string[] EventIds { get; set; }
        [JsonProperty(PropertyName = "publisher")]
        string Publisher { get; set; }
    }
}
