using System.Collections.Generic;
using Newtonsoft.Json;

namespace My.JDownloader.Api.ApiObjects.Devices
{
    public class DeviceConnectionInfo
    {
        [JsonProperty(PropertyName = "infos")]
        public List<DirectConnectionInfo> Infos { get; set; }
        [JsonProperty(PropertyName = "rebindProtectionDetected")]
        public bool RebindProtectionDetected { get; set; }
        [JsonProperty(PropertyName = "mode")]
        public string Mode { get; set; }
    }
}
