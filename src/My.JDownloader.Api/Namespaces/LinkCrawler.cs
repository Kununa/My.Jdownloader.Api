using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects.Devices;
using System.Threading.Tasks;
using My.JDownloader.Api.ApiObjects.Login;

namespace My.JDownloader.Api.Namespaces
{
    public class LinkCrawler : NamespaceBase
    {
        public LinkCrawler(DeviceObject device, LoginObject loginObject) : base(device, loginObject) { }

        /// <summary>
        /// Asks the client if the linkcrawler is still crawling.
        /// </summary>
        /// <returns>Ture if succesfull</returns>
        public async Task<bool> IsCrawling()
        {
            var response =
                await CallAction<bool>("/linkcrawler/isCrawling", null);
            return response;
        }
    }
}
