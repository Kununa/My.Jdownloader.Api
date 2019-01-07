using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace My.JDownloader.Api.Namespaces
{
    public class System
    {
        private readonly JDownloaderApiHandler _ApiHandler;
        private readonly DeviceObject _Device;

        internal System(JDownloaderApiHandler apiHandler, DeviceObject device)
        {
            _ApiHandler = apiHandler;
            _Device = device;
        }

        /// <summary>
        /// Closes the JDownloader client.
        /// </summary>
        public async Task ExitJd()
        {
            await _ApiHandler.CallAction<object>(_Device, "/system/exitJD", null, JDownloaderHandler.LoginObject, true);
        }

        /// <summary>
        /// Gets storage informations of the given path.
        /// </summary>
        /// <param name="path">The Path you want to check.</param>
        /// <returns>An array with storage informations.</returns>
        public async Task<StorageInfoReturnObject[]> GetStorageInfos(string path)
        {
            var param = new[] { path };
            var tmp = await _ApiHandler.CallAction<StorageInfoReturnObject[]>(_Device, "/system/getStorageInfos", param, JDownloaderHandler.LoginObject, true);

            return tmp;
        }

        /// <summary>
        /// Gets information of the system the JDownloader client is running on.
        /// </summary>
        /// <returns></returns>
        public async Task<SystemInfoReturnObject> GetSystemInfos()
        {
            return await _ApiHandler.CallAction<SystemInfoReturnObject>(_Device, "/system/getSystemInfos", null, JDownloaderHandler.LoginObject, true);
        }

        /// <summary>
        /// Hibernates the current os the JDownloader client is running on.
        /// </summary>
        public async Task HibernateOS()
        {
            await _ApiHandler.CallAction<object>(_Device, "/system/hibernateOS", null, JDownloaderHandler.LoginObject, true);
        }

        /// <summary>
        /// Restarts the JDownloader client.
        /// </summary>
        public async Task RestartJd()
        {
            await _ApiHandler.CallAction<object>(_Device, "/system/restartJD", null, JDownloaderHandler.LoginObject, true);
        }

        /// <summary>
        /// Shutsdown the current os the JDownloader client is running on.
        /// </summary>
        /// <param name="force">True if you want to force the shutdown process.</param>
        public async Task ShutdownOS(bool force)
        {
            await _ApiHandler.CallAction<object>(_Device, "/system/shutdownOS", new[] { force }, JDownloaderHandler.LoginObject, true);
        }

        /// <summary>
        /// Sets the current os the JDownloader client is running on in standby.
        /// </summary>
        public async Task StandbyOS()
        {
            await _ApiHandler.CallAction<object>(_Device, "/system/standbyOS", null, JDownloaderHandler.LoginObject, true);
        }
    }
}
