using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace My.JDownloader.Api.ApiObjects.LinkgrabberV2
{
    public class AddLinksQuery
    {
        [JsonProperty(PropertyName = "assignJobID")]
        public bool? AssignJobId { get; set; }
        [JsonProperty(PropertyName = "autoExtract")]
        public bool? AutoExtract { get; set; }
        [JsonProperty(PropertyName = "autostart")]
        public bool? Autostart { get; set; }
        [JsonProperty(PropertyName = "dataURLs")]
        public string[] DataUrLs { get; set; }
        [JsonProperty(PropertyName = "deepDecrypt")]
        public bool? DeepDecrypt { get; set; }
        [JsonProperty(PropertyName = "destinationFolder")]
        public string DestinationFolder { get; set; }
        [JsonProperty(PropertyName = "downloadPassword")]
        public string DownloadPassword { get; set; }
        [JsonProperty(PropertyName = "extractPassword")]
        public string ExtractPassword { get; set; }
        [JsonProperty(PropertyName = "links")]
        public string Links { get; set; }
        [JsonProperty(PropertyName = "overwritePackagizerRules")]
        public bool? OverwritePackagizerRules { get; set; }
        [JsonProperty(PropertyName = "packageName")]
        public string PackageName { get; set; }
        [JsonProperty(PropertyName = "priority"), JsonConverter(typeof(StringEnumConverter))]
        public Enums.PriorityType Priority { get; set; }
        [JsonProperty(PropertyName = "sourceUrl")]
        public string SourceUrl { get; set; }

        public AddLinksQuery()
        {
            AssignJobId = null;
            AutoExtract = null;
            Autostart = null;
            DeepDecrypt = null;
            OverwritePackagizerRules = null;
        }
    }
}
