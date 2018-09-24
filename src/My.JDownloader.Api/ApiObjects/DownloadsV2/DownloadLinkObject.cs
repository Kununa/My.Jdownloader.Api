using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.JDownloader.Api.ApiObjects.DownloadsV2
{
    public class DownloadLinkObject
    {
        public List<DownloadLink> Data { get; set; }
        public object DiffType { get; set; }
        public int RequestId { get; set; }
        public object DiffId { get; set; }
    }
}
