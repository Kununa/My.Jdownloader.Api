using System.Collections.Generic;
using Newtonsoft.Json;


namespace My.JDownloader.Api.ApiObjects.DownloadsV2
{
    class PackageQueryObject
    {
        public List<FilePackage> Data { get; set; }
        [JsonProperty(PropertyName = "rid")]
        public int RequestId { get; set; }
    }
}
