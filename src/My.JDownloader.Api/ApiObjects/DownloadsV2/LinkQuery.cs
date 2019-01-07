﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.JDownloader.Api.ApiObjects.DownloadsV2
{
    public class LinkQuery
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "addedDate")]
        public bool AddedDate { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "bytesLoaded")]
        public bool BytesLoaded { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "bytesTotal")]
        public bool BytesTotal { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "comment")]
        public bool Comment { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "eta")]
        public bool Eta { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "extractionStatus")]
        public bool ExtractionStatus { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "finished")]
        public bool Finished { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "finishedDate")]
        public bool FinishedDate { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "host")]
        public bool Host { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "jobUUIDs")]
        public long[] JobUUIDs { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "maxResults")]
        public int MaxResults { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "packageUUIDs")]
        public long[] PackageUUIDs { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "password")]
        public bool Password { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "priority")]
        public bool Priority { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "running")]
        public bool Running { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "skipped")]
        public bool Skipped { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "speed")]
        public bool Speed { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "startAt")]
        public int StartAt { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "status")]
        public bool Status { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "url")]
        public bool Url { get; set; }
    }
}