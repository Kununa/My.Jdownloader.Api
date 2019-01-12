using System.Collections.Generic;

namespace My.JDownloader.Api.ApiObjects.Extraction
{
    /**
     * Leave Boolean values empty or set them to null if you want JD to use the default value
     */
    public class ArchiveSettings
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "archiveId")]
        public string ArchiveId { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "autoExtract")]
        public bool AutoExtract { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "extractPath")]
        public string ExtractPath { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "finalPassword")]
        public string FinalPassword { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "passwords")]
        public List<string> Passwords { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "removeDownloadLinksAfterExtraction")]
        public bool RemoveDownloadLinksAfterExtraction { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "removeFilesAfterExtraction")]
        public bool RemoveFilesAfterExtraction { get; set; }
    }
}
