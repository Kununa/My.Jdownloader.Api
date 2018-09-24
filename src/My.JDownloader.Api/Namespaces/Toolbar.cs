using System;
using System.Collections.Generic;
using System.Linq;
using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Toolbar;
using Newtonsoft.Json.Linq;

namespace My.JDownloader.Api.Namespaces
{
    public class Toolbar
    {
        private readonly JDownloaderApiHandler _ApiHandler;
        private readonly DeviceObject _Device;

        internal Toolbar(JDownloaderApiHandler apiHandler, DeviceObject device)
        {
            _ApiHandler = apiHandler;
            _Device = device;
        }

        /// <summary>
        /// Toggles the automatic reconnect function.
        /// </summary>
        /// <returns></returns>
        public bool ToggleAutomaticReconnect()
        {
            var tmp = _ApiHandler.CallAction<bool>(_Device, "/toolbar/toggleAutomaticReconnect", null, JDownloaderHandler.LoginObject, true);

            return tmp;
        }

        /// <summary>
        /// Toggles the bandwidth limit.
        /// </summary>
        /// <returns></returns>
        public bool ToggleDownloadSpeedLimit()
        {
            var tmp = _ApiHandler.CallAction<bool>(_Device, "/toolbar/toggleDownloadSpeedLimit", null, JDownloaderHandler.LoginObject, true);

            return tmp;
        }

        /// <summary>
        /// Gets the current Status of jDowloader
        /// </summary>
        /// <returns></returns>
        public StatusObject GetStatus()
        {
            var tmp = _ApiHandler.CallAction<DefaultReturnObject>(_Device, "/toolbar/getStatus", null, JDownloaderHandler.LoginObject, true);

            var data = (JObject)tmp?.Data;
            return data?.ToObject<StatusObject>();
        }
    }
}
