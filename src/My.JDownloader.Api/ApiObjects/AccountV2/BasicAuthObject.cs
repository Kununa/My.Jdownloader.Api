using Newtonsoft.Json;

namespace My.JDownloader.Api.ApiObjects.AccountV2
{
    public class BasicAuthObject
    {
        [JsonProperty(PropertyName = "created")]
        long Created { get; set; }
        [JsonProperty(PropertyName = "enabled")]
        bool Enabled { get; set; }
        [JsonProperty(PropertyName = "hostmask")]
        string Hostmask { get; set; }
        [JsonProperty(PropertyName = "id")]
        long Id { get; set; }
        [JsonProperty(PropertyName = "lastValidated")]
        long LastValidated { get; set; }
        [JsonProperty(PropertyName = "password")]
        string Password { get; set; }
        [JsonProperty(PropertyName = "type")]
        HostType Type { get; set; }
        [JsonProperty(PropertyName = "username")]
        string Username { get; set; }
    }
}
