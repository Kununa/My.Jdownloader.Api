using System.Collections.Generic;
namespace My.JDownloader.Api.ApiObjects.Extraction
{
    public class ArchiveStatus
    {
        /**
   * ID to adress the archive. Used for example for extraction/getArchiveSettings?[,,...]
   */

        [Newtonsoft.Json.JsonProperty(PropertyName = "archiveId")]
        public string ArchiveId { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "archiveName")]
        string ArchiveName { get; set; }
        /**
         * -1 or the controller ID if any controller is active. Used in cancelExtraction? 
         */

        [Newtonsoft.Json.JsonProperty(PropertyName = "controllerId")]
        public long ControllerId { get; set; }
        /**
         * The status of the controller
         */

        [Newtonsoft.Json.JsonProperty(PropertyName = "controllerStatus")]
        public Enums.ControllerStatus ControllerStatus { get; set; }
        /**
         * Map Keys: Filename of the Part-File. Values: ArchiveFileStatus
         * Example: 
         * {
         * archive.part1.rar:COMPLETE,
         * archive.part2.rar:MISSING
         * }
         */

        [Newtonsoft.Json.JsonProperty(PropertyName = "states")]
        public Dictionary<string, Enums.ArchiveFileStatus> States { get; set; }
        /**
         * Type of the archive. e.g. GZIP_SINGLE, RAR_MULTI,RAR_SINGLE,.... 
         */
        [Newtonsoft.Json.JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
