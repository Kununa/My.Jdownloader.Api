using System.Collections.Generic;

namespace My.JDownloader.Api.ApiObjects.AccountV2
{
    public class ListAccountRequestObject
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "error")]
        public bool Error { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "maxResults")]
        public int MaxResults { get; set; } = 20;
        [Newtonsoft.Json.JsonProperty(PropertyName = "startAt")]
        public int StartAt { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "trafficLeft")]
        public bool TrafficLeft { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "trafficMax")]
        public bool TrafficMax { get; set; }
		[Newtonsoft.Json.JsonProperty(PropertyName = "userName")]
        public bool Username { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "uuidlist")]
        public List<long> Uuidlist { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "valid")]
        public bool Valid { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "validUntil")]
        public bool ValidUntil { get; set; }
    }
}
