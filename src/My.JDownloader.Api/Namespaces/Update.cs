using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects;
using My.JDownloader.Api.ApiObjects.Devices;
using System.Threading.Tasks;

namespace My.JDownloader.Api.Namespaces
{
    public class Update
    {
        private readonly JDownloaderApiHandler _ApiHandler;
        private readonly DeviceObject _Device;

        internal Update(JDownloaderApiHandler apiHandler, DeviceObject device)
        {
            _ApiHandler = apiHandler;
            _Device = device;
        }

        /// <summary>
        /// Checks if the client has an update available.
        /// </summary>
        /// <returns>True if an update is available.</returns>
        public async Task<bool> IsUpdateAvailable()
        {
            var response = await _ApiHandler.CallAction<bool>(_Device, "/update/isUpdateAvailable",
                null, JDownloaderHandler.LoginObject, true);

            return response;
        }

        /// <summary>
        /// Restarts the client and starts the update.
        /// </summary>
        public async Task RestartAndUpdate()
        {
            await _ApiHandler.CallAction<object>(_Device, "/update/restartAndUpdate",
                null, JDownloaderHandler.LoginObject, true);
        }

        /// <summary>
        /// Start the update check on the client.
        /// </summary>
        public async Task RunUpdateCheck()
        {
            await _ApiHandler.CallAction<object>(_Device, "/update/runUpdateCheck",
                null, JDownloaderHandler.LoginObject, true);
        }
    }
}
