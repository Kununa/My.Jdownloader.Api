using Newtonsoft.Json;

namespace My.JDownloader.Api.ApiObjects.Devices
{
    public class DirectConnectionInfo
    {
        [JsonProperty(PropertyName = "port")]
        public int Port { get; set; }
        [JsonProperty(PropertyName = "ip")]
        public string Ip { get; set; }
    }
}
