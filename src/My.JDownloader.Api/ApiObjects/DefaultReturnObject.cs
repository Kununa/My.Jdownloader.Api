﻿using Newtonsoft.Json;

namespace My.JDownloader.Api.ApiObjects
{
    public class DefaultReturnObject
    {
        [JsonProperty(PropertyName = "data")]
        public dynamic Data { get; set; }
        [JsonProperty(PropertyName = "diffType")]
        public string DiffType { get; set; }
        [JsonProperty(PropertyName = "rid")]
        public long Rid { get; set; }
        [JsonProperty(PropertyName = "diffID")]
        public string DiffId { get; set; }

    }
}
