using Newtonsoft.Json;

namespace My.JDownloader.Api.ApiObjects.LinkgrabberV2
{
    public class LinkCollectingJob 
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

       
    }
}
