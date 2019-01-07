using System.Collections.Generic;

namespace My.JDownloader.Api.ApiObjects.Devices
{
    public class DeviceJsonReturnObject
    {
        [Newtonsoft.Json.JsonProperty(PropertyName ="list")]
        public List<DeviceObject> Devices { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "rid")]
        public long RequestId { get; set; }
    }
}
