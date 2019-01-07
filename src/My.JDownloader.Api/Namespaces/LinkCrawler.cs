using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects;
using My.JDownloader.Api.ApiObjects.Devices;
using System.Threading.Tasks;

namespace My.JDownloader.Api.Namespaces
{
    public class LinkCrawler
    {
        private readonly DeviceObject _Device;

        internal LinkCrawler(DeviceObject device)
        {
            _Device = device;
        }

        /// <summary>
        /// Asks the client if the linkcrawler is still crawling.
        /// </summary>
        /// <returns>Ture if succesfull</returns>
        public async Task<bool> IsCrawling()
        {
            var response =
                await JDownloaderApiHandler.CallAction<bool>(_Device, "/linkcrawler/isCrawling", null, JDownloaderHandler.LoginObject);
            return response;
        }
    }
}
