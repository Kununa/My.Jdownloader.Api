using System.Collections.Generic;
using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.DownloadsV2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace My.JDownloader.Api.Namespaces
{
    public class DownloadsV2
    {
        private readonly JDownloaderApiHandler _ApiHandler;
        private readonly DeviceObject _Device;

        internal DownloadsV2(JDownloaderApiHandler apiHandler, DeviceObject device)
        {
            _ApiHandler = apiHandler;
            _Device = device;
        }

        /// <summary>
        /// Gets the stop mark as long.
        /// </summary>
        /// <returns>The stop mark as long.</returns>
        public async Task<long> GetStopMark()
        {
            var response = await _ApiHandler.CallAction<long>(_Device, "/downloadsV2/getStopMark", null, JDownloaderHandler.LoginObject);

            return response;
        }

        /// <summary>
        /// Gets informations about a stop marked link.
        /// </summary>
        /// <returns>Returns informations about a stop marked link.</returns>
        public async Task<DownloadLink> GetStopMarkedLink()
        {
            var response = await _ApiHandler.CallAction<DownloadLink>(_Device, "/downloadsV2/getStopMarkedLink", null, JDownloaderHandler.LoginObject);

            return response;
        }

        /// <summary>
        /// Gets a list of available packages that are currently in the download list.
        /// </summary>
        /// <param name="requestObject">The request object which contains properties to define the return properties.</param>
        /// <returns>Returns a list of all available packages.</returns>
        public async Task<List<FilePackage>> QueryPackages(PackageQuery requestObject)
        {
            string json = JsonConvert.SerializeObject(requestObject);
            var param = new[] { json };

            var response =
                await _ApiHandler.CallAction<List<FilePackage>>(_Device, "/downloadsV2/queryPackages", param,
                    JDownloaderHandler.LoginObject, true);
            return response;
        }

        /// <summary>
        /// Moves one or multiple packages after antoher package.
        /// </summary>
        /// <param name="packageIds">The ids of the packages you want to move.</param>
        /// <param name="afterDestPackageId">The id of the package you want to move the others to.</param>
        /// <returns>True if successfull.</returns>
        public async Task<bool> MovePackages(long[] packageIds, long afterDestPackageId)
        {
            var param = new object[] { packageIds, afterDestPackageId };

            var response =
                await _ApiHandler.CallAction<object>(_Device, "/downloadsV2/movePackages", param,
                    JDownloaderHandler.LoginObject);
            if (response == null)
                return false;
            return true;
        }

        /// <summary>
        /// Removes one or multiple links or packages.
        /// </summary>
        /// <param name="linkIds">The ids of the links you want to remove.</param>
        /// <param name="packageIds">The ids of the packages you want to remove.</param>
        /// <returns>True if successfull.</returns>
        public async Task<bool> RemoveLinks(long[] linkIds, long[] packageIds)
        {
            var param = new object[] { linkIds, packageIds };

            var response =
                await _ApiHandler.CallAction<object>(_Device, "/downloadsV2/removeLinks", param,
                    JDownloaderHandler.LoginObject, true);
            if (response == null)
                return false;
            return true;
        }

        /// <summary>
        /// Gets all links that are currently in the download list.
        /// </summary>
        /// <param name="queryLink">The request object which contains properties to define the return properties.</param>
        /// <returns>Returns a list of all links that are currently in the download list.</returns>
        public async Task<List<DownloadLink>> QueryLinks(LinkQuery queryLink)
        {
            string json = JsonConvert.SerializeObject(queryLink);
            var param = new[] { json };

            var response =
                await _ApiHandler.CallAction<List<DownloadLink>>(_Device, "/downloadsV2/queryLinks", param,
                    JDownloaderHandler.LoginObject, true).ConfigureAwait(false);
            return response;
        }

    }
}
