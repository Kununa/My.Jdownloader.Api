using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects;
using My.JDownloader.Api.ApiObjects.Devices;
using System.Threading.Tasks;

namespace My.JDownloader.Api.Namespaces
{
    public class DownloadController
    {
        private readonly JDownloaderApiHandler _ApiHandler;
        private readonly DeviceObject _Device;

        internal DownloadController(JDownloaderApiHandler apiHandler, DeviceObject device)
        {
            _ApiHandler = apiHandler;
            _Device = device;
        }

        /// <summary>
        /// Forces JDownloader to start downloading the given links/packages
        /// </summary>
        /// <param name="linkIds">The ids of the links you want to force download.</param>
        /// <param name="packageIds">The ids of the packages you want to force download.</param>
        /// <returns>True if successfull</returns>
        public bool ForceDownload(long[] linkIds, long[] packageIds)
        {
            var param = new[] { linkIds, packageIds };
            var result = _ApiHandler.CallAction<DefaultReturnObject>(_Device, "/downloadcontroller/forceDownload", param, JDownloaderHandler.LoginObject, true);
            return result != null;
        }

        /// <summary>
        /// Gets the current state of the device
        /// </summary>
        /// <returns>The current state of the device.</returns>
        public async Task<string> GetCurrentState()
        {
            var result = await _ApiHandler.CallAction<string>(_Device, "/downloadcontroller/getCurrentState", null, JDownloaderHandler.LoginObject, true);
            if (result != null)
                return result;
            return "UNKOWN_STATE";
        }

        /// <summary>
        /// Gets the actual download speed of the client.
        /// </summary>
        /// <returns>The actual download speed.</returns>
        public async Task<long> GetSpeedInBps()
        {
            var result = await _ApiHandler.CallAction<long>(_Device, "/downloadcontroller/getSpeedInBps", null, JDownloaderHandler.LoginObject, true);
            return result;
        }

        /// <summary>
        /// Starts all downloads.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public async Task<bool> Start()
        {
            var result = await _ApiHandler.CallAction<bool>(_Device, "/downloadcontroller/start", null, JDownloaderHandler.LoginObject, true);
            return result;
        }

        /// <summary>
        /// Stops all downloads.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public async Task<bool> Stop()
        {
            var result = await _ApiHandler.CallAction<bool>(_Device, "/downloadcontroller/stop", null, JDownloaderHandler.LoginObject, true);
            return result;
        }

        /// <summary>
        /// Pauses all downloads.
        /// </summary>
        /// <param name="pause">True if you want to pause the download</param>
        /// <returns>True if successfull.</returns>
        public async Task<bool> Pause(bool pause)
        {
            var param = new[] { pause };
            var result = await _ApiHandler.CallAction<bool>(_Device, "/downloadcontroller/pause", param, JDownloaderHandler.LoginObject, true);

            return result;
        }


    }
}
