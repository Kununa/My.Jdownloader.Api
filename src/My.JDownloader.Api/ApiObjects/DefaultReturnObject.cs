using Newtonsoft.Json;

namespace My.JDownloader.Api.ApiObjects
{
    public class DefaultReturnObject
    {
        [JsonProperty(PropertyName = "data")]
        public dynamic Data { get; set; }
        [JsonProperty(PropertyName = "diffType")]
        public string DiffType { get; set; }
        [JsonProperty(PropertyName = "rid")]
        public int Rid { get; set; }
        [JsonProperty(PropertyName = "diffID")]
        public string DiffID { get; set; }

    }
}
