using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects;
using My.JDownloader.Api.ApiObjects.Devices;
using System.Threading.Tasks;
using My.JDownloader.Api.ApiObjects.Login;

namespace My.JDownloader.Api.Namespaces
{
    public class DownloadController : NamespaceBase
    {
        public DownloadController(DeviceObject device, LoginObject loginObject) : base(device, loginObject, "downloadcontroller") { }

        /// <summary>
        /// Forces JDownloader to start downloading the given links/packages
        /// </summary>
        /// <param name="linkIds">The ids of the links you want to force download.</param>
        /// <param name="packageIds">The ids of the packages you want to force download.</param>
        /// <returns>True if successfull</returns>
        public async Task<bool> ForceDownload(long[] linkIds, long[] packageIds)
        {
            var param = new[] { linkIds, packageIds };
            var result = await CallAction<DefaultReturnObject>("forceDownload", param);
            return result != null;
        }

        /// <summary>
        /// Gets the current state of the device
        /// </summary>
        /// <returns>The current state of the device.</returns>
        public async Task<string> GetCurrentState()
        {
            var result = await CallAction<string>("getCurrentState", null);
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
            var result = await CallAction<long>("getSpeedInBps", null);
            return result;
        }

        /// <summary>
        /// Starts all downloads.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public async Task<bool> Start()
        {
            var result = await CallAction<bool>("start", null);
            return result;
        }

        /// <summary>
        /// Stops all downloads.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public async Task<bool> Stop()
        {
            var result = await CallAction<bool>("stop", null);
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
            var result = await CallAction<bool>("pause", param);

            return result;
        }


    }
}
