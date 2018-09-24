using System.Collections.Generic;
using Newtonsoft.Json;

namespace My.JDownloader.Api.ApiObjects.LinkgrabberV2
{
    internal class CrawledPackageObject
    {
        public List<CrawledPackage> Data { get; set; }
        [JsonProperty(PropertyName = "rid")]
        public int RequestId { get; set; }
    }
}
