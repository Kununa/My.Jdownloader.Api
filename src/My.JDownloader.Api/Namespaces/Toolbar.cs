using System;
using System.Collections.Generic;
using System.Linq;
using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Toolbar;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace My.JDownloader.Api.Namespaces
{
    public class Toolbar
    {
        private readonly DeviceObject _Device;

        internal Toolbar(DeviceObject device)
        {
            _Device = device;
        }

        /// <summary>
        /// Toggles the automatic reconnect function.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ToggleAutomaticReconnect()
        {
            var tmp = await JDownloaderApiHandler.CallAction<bool>(_Device, "/toolbar/toggleAutomaticReconnect", null, JDownloaderHandler.LoginObject);

            return tmp;
        }

        /// <summary>
        /// Toggles the bandwidth limit.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ToggleDownloadSpeedLimit()
        {
            var tmp = await JDownloaderApiHandler.CallAction<bool>(_Device, "/toolbar/toggleDownloadSpeedLimit", null, JDownloaderHandler.LoginObject);

            return tmp;
        }

        /// <summary>
        /// Gets the current Status of jDowloader
        /// </summary>
        /// <returns></returns>
        public async Task<StatusObject> GetStatus()
        {
            var tmp = await JDownloaderApiHandler.CallAction<DefaultReturnObject>(_Device, "/toolbar/getStatus", null, JDownloaderHandler.LoginObject);

            var data = (JObject)tmp?.Data;
            return data?.ToObject<StatusObject>();
        }
    }
}
