using System.Collections.Generic;
using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.System;
using System.Threading.Tasks;
using My.JDownloader.Api.ApiObjects.Login;

namespace My.JDownloader.Api.Namespaces
{
    public class System : NamespaceBase
    {
        public System(DeviceObject device, LoginObject loginObject) : base(device, loginObject, "system") { }

        /// <summary>
        /// Closes the JDownloader client.
        /// </summary>
        public async Task ExitJd()
        {
            await CallAction<object>("exitJD", null);
        }

        /// <summary>
        /// Gets storage informations of the given path.
        /// </summary>
        /// <param name="path">The Path you want to check.</param>
        /// <returns>An array with storage informations.</returns>
        public async Task<IReadOnlyList<StorageInfoReturnObject>> GetStorageInfos(string path)
        {
            var param = new[] { path };
            var tmp = await CallAction<IReadOnlyList<StorageInfoReturnObject >> ("getStorageInfos", param);

            return tmp;
        }

        /// <summary>
        /// Gets information of the system the JDownloader client is running on.
        /// </summary>
        /// <returns></returns>
        public async Task<SystemInfoReturnObject> GetSystemInfos()
        {
            return await CallAction<SystemInfoReturnObject>("getSystemInfos", null);
        }

        /// <summary>
        /// Hibernates the current os the JDownloader client is running on.
        /// </summary>
        public async Task HibernateOs()
        {
            await CallAction<object>("hibernateOS", null);
        }

        /// <summary>
        /// Restarts the JDownloader client.
        /// </summary>
        public async Task RestartJd()
        {
            await CallAction<object>("restartJD", null);
        }

        /// <summary>
        /// Shutsdown the current os the JDownloader client is running on.
        /// </summary>
        /// <param name="force">True if you want to force the shutdown process.</param>
        public async Task ShutdownOs(bool force)
        {
            await CallAction<object>("shutdownOS", new[] { force });
        }

        /// <summary>
        /// Sets the current os the JDownloader client is running on in standby.
        /// </summary>
        public async Task StandbyOs()
        {
            await CallAction<object>("standbyOS", null);
        }
    }
}
