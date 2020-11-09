using Newtonsoft.Json;

namespace My.JDownloader.Api.ApiObjects.Extensions
{
    public class ExtensionRequestObject
    {
        [JsonProperty(PropertyName = "configInterface")]
        public bool ConfigInterface { get; set; }
        [JsonProperty(PropertyName = "description")]
        public bool Description { get; set; }
        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
        [JsonProperty(PropertyName = "iconKey")]
        public bool IconKey { get; set; }
        [JsonProperty(PropertyName = "installed")]
        public bool Installed { get; set; }
        [JsonProperty(PropertyName = "name")]
        public bool Name { get; set; }
        [JsonProperty(PropertyName = "pattern")]
        public string Pattern { get; set; }
    }
}
