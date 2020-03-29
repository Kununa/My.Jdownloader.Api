using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Toolbar;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using My.JDownloader.Api.ApiObjects.Login;

namespace My.JDownloader.Api.Namespaces
{
    public class Toolbar : NamespaceBase
    {
        public Toolbar(DeviceObject device, LoginObject loginObject) : base(device, loginObject) { }

        /// <summary>
        /// Toggles the automatic reconnect function.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ToggleAutomaticReconnect()
        {
            var tmp = await CallAction<bool>("/toolbar/toggleAutomaticReconnect", null);

            return tmp;
        }

        /// <summary>
        /// Toggles the bandwidth limit.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ToggleDownloadSpeedLimit()
        {
            var tmp = await CallAction<bool>("/toolbar/toggleDownloadSpeedLimit", null);

            return tmp;
        }

        /// <summary>
        /// Gets the current Status of jDowloader
        /// </summary>
        /// <returns></returns>
        public async Task<StatusObject> GetStatus()
        {
            var tmp = await CallAction<DefaultReturnObject>("/toolbar/getStatus", null);

            var data = (JObject)tmp?.Data;
            return data?.ToObject<StatusObject>();
        }
    }
}
