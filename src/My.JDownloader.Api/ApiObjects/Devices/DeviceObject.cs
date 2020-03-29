using Newtonsoft.Json;

namespace My.JDownloader.Api.ApiObjects.Devices
{
    public class DeviceObject
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        
        public override string ToString()
        {
            return Name;
        }
    }
}
