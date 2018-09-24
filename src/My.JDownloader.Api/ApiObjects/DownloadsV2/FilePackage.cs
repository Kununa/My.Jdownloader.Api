namespace My.JDownloader.Api.ApiObjects.DownloadsV2
{
    public class FilePackage
    {
        public string ActiveTask { get; set; }
        public long BytesLoaded { get; set; }
        public long BytesTotal { get; set; }
        public int ChildCount { get; set; }
        public string Comment { get; set; }
        public string DownloadPassword { get; set; }
        public bool Enabled { get; set; }
        public long Eta { get; set; }
        public bool Finished { get; set; }
        public string[] Hosts { get; set; }
        public string Name { get; set; }
        public PriorityType Priority { get; set; }
        public bool Running { get; set; }
        public string SaveTo { get; set; }
        public long Speed { get; set; }
        public string Status { get; set; }
        public string StatusIconKey { get; set; }
        public long Uuid { get; set; }
}
}
