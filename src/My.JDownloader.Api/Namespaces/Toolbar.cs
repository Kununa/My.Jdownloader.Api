using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Toolbar;
using System.Threading.Tasks;
using My.JDownloader.Api.ApiObjects.Login;

namespace My.JDownloader.Api.Namespaces
{
    public class Toolbar : NamespaceBase
    {
        public Toolbar(DeviceObject device, LoginObject loginObject) : base(device, loginObject, "toolbar") { }

        /// <summary>
        /// Toggles the automatic reconnect function.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ToggleAutomaticReconnect()
        {
            return await CallAction<bool>("toggleAutomaticReconnect");
        }

        /// <summary>
        /// Toggles the bandwidth limit.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ToggleDownloadSpeedLimit()
        {
            return await CallAction<bool>("toggleDownloadSpeedLimit");
        }

        /// <summary>
        /// Gets the current Status of jDowloader
        /// </summary>
        /// <returns></returns>
        public async Task<StatusObject> GetStatus()
        {
            return await CallAction<StatusObject>("getStatus");
        }
    }
}
