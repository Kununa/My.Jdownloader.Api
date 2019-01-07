using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects;
using My.JDownloader.Api.ApiObjects.Devices;
using System.Threading.Tasks;

namespace My.JDownloader.Api.Namespaces
{
    public class Extraction
    {
        private readonly DeviceObject _Device;

        internal Extraction(DeviceObject device)
        {
            _Device = device;
        }

        /// <summary>
        /// Adds an archive password to the client.
        /// </summary>
        /// <param name="password">The password to add.</param>
        /// <returns>True if successfull.</returns>
        public async Task<bool> AddArchivePassword(string password)
        {
            var param = new[] {password};
            var response = await JDownloaderApiHandler.CallAction<bool>(_Device, "/extraction/addArchivePassword",
                param, JDownloaderHandler.LoginObject);

            return response;
        }

        /// <summary>
        /// Cancels an extraction process.
        /// </summary>
        /// <param name="controllerId">The id of the controll you want to cancel.</param>
        /// <returns>True if successfull.</returns>
        public async Task<bool> CancelExtraction(string controllerId)
        {
            var param = new[] { controllerId };
            var response = await JDownloaderApiHandler.CallAction<bool>(_Device, "/extraction/cancelExtraction",
                param, JDownloaderHandler.LoginObject);

            return response;
        }
    }
}
