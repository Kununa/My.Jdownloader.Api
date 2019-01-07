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
        private readonly DeviceObject _Device;

        internal System(DeviceObject device)
        {
            _Device = device;
        }

        /// <summary>
        /// Closes the JDownloader client.
        /// </summary>
        public async Task ExitJd()
        {
            await JDownloaderApiHandler.CallAction<object>(_Device, "/system/exitJD", null, JDownloaderHandler.LoginObject);
        }

        /// <summary>
        /// Gets storage informations of the given path.
        /// </summary>
        /// <param name="path">The Path you want to check.</param>
        /// <returns>An array with storage informations.</returns>
        public async Task<StorageInfoReturnObject[]> GetStorageInfos(string path)
        {
            var param = new[] { path };
            var tmp = await JDownloaderApiHandler.CallAction<StorageInfoReturnObject[]>(_Device, "/system/getStorageInfos", param, JDownloaderHandler.LoginObject);

            return tmp;
        }

        /// <summary>
        /// Gets information of the system the JDownloader client is running on.
        /// </summary>
        /// <returns></returns>
        public async Task<SystemInfoReturnObject> GetSystemInfos()
        {
            return await JDownloaderApiHandler.CallAction<SystemInfoReturnObject>(_Device, "/system/getSystemInfos", null, JDownloaderHandler.LoginObject);
        }

        /// <summary>
        /// Hibernates the current os the JDownloader client is running on.
        /// </summary>
        public async Task HibernateOS()
        {
            await JDownloaderApiHandler.CallAction<object>(_Device, "/system/hibernateOS", null, JDownloaderHandler.LoginObject);
        }

        /// <summary>
        /// Restarts the JDownloader client.
        /// </summary>
        public async Task RestartJd()
        {
            await JDownloaderApiHandler.CallAction<object>(_Device, "/system/restartJD", null, JDownloaderHandler.LoginObject);
        }

        /// <summary>
        /// Shutsdown the current os the JDownloader client is running on.
        /// </summary>
        /// <param name="force">True if you want to force the shutdown process.</param>
        public async Task ShutdownOS(bool force)
        {
            await JDownloaderApiHandler.CallAction<object>(_Device, "/system/shutdownOS", new[] { force }, JDownloaderHandler.LoginObject);
        }

        /// <summary>
        /// Sets the current os the JDownloader client is running on in standby.
        /// </summary>
        public async Task StandbyOS()
        {
            await JDownloaderApiHandler.CallAction<object>(_Device, "/system/standbyOS", null, JDownloaderHandler.LoginObject);
        }
    }
}
