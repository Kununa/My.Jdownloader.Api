using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.DownloadsV2;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using My.JDownloader.Api.ApiObjects.Login;

namespace My.JDownloader.Api.Namespaces
{
    public class DownloadsV2 : NamespaceBase
    {
        public DownloadsV2(DeviceObject device, LoginObject loginObject) : base(device, loginObject) { }

        public async Task<bool> Cleanup(long[] linkIds, long[] packageIds, Enums.Action action, Enums.Mode mode, Enums.SelectionType selectionType)
        {
            var param = new object[] { linkIds, packageIds, action.ToString(),  mode.ToString(), selectionType.ToString() };
            var response =
                await CallAction<string>("/downloadsV2/cleanup", param);
            return response != null;
        }

        public async Task<bool> ForceDownload(long[] linkIds, long[] packageIds)
        {
            var param = new object[] { linkIds, packageIds };
            var response =
                await CallAction<bool>("/downloadsV2/queryPackages", param);
            return response;
        }

        /*public async Task<bool> GetDownloadUrls(long[] linkIds, long[] packageIds, UrlDisplayTypeStorable[] urlDisplayTypeStorable)
        {
            var param = new object[] { linkIds, packageIds };

            var response =
                await JDownloaderApiHandler.CallAction<bool>(_Device, "/downloadsV2/queryPackages", param,
                    JDownloaderHandler.LoginObject);
            return response;
        }*/

        /// <summary>
        /// Gets the stop mark as long.
        /// </summary>
        /// <returns>The stop mark as long.</returns>
        public async Task<long> GetStopMark()
        {
            var response = await CallAction<long>("/downloadsV2/getStopMark", null);
            return response;
        }

        /// <summary>
        /// Gets informations about a stop marked link.
        /// </summary>
        /// <returns>Returns informations about a stop marked link.</returns>
        public async Task<DownloadLink> GetStopMarkedLink()
        {
            var response = await CallAction<DownloadLink>("/downloadsV2/getStopMarkedLink", null);
            return response;
        }

        /// <summary>
        /// Gets a list of available packages that are currently in the download list.
        /// </summary>
        /// <param name="requestObject">The request object which contains properties to define the return properties.</param>
        /// <returns>Returns a list of all available packages.</returns>
        public async Task<IReadOnlyList<FilePackage>> QueryPackages(PackageQuery requestObject)
        {
            if (requestObject == null)
                requestObject = new PackageQuery();
            var json = JsonConvert.SerializeObject(requestObject);
            var param = new[] { json };

            var response =
                await CallAction<IReadOnlyList<FilePackage>>("/downloadsV2/queryPackages", param);
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
                await CallAction<object>("/downloadsV2/movePackages", param);
            return response != null;
        }

        /// <summary>
        /// Removes one or multiple links or packages.
        /// </summary>
        /// <param name="linkIds">The ids of the links you want to remove.</param>
        /// <param name="packageIds">The ids of the packages you want to remove.</param>
        /// <returns>True if successfull.</returns>
        public async Task<bool> RemoveLinks(long[] linkIds, long[] packageIds)
        {
            var param = new object[] { linkIds ?? new long[0], packageIds ?? new long[0] };

            var response =
                await CallAction<object>("/downloadsV2/removeLinks", param);
            return response != null;
        }

        /// <summary>
        /// Removes one or multiple links or packages.
        /// </summary>
        /// <param name="linkIds">The ids of the links you want to remove.</param>
        /// <param name="packageIds">The ids of the packages you want to remove.</param>
        /// <returns>True if successfull.</returns>
        public async Task<bool> ResetLinks(long[] linkIds = null, long[] packageIds = null)
        {
            var param = new object[] { linkIds ?? new long[0], packageIds ?? new long[0] };

            var response =
                await CallAction<object>("/downloadsV2/resetLinks", param);
            return response != null;
        }

        /// <summary>
        /// Gets all links that are currently in the download list.
        /// </summary>
        /// <param name="queryLink">The request object which contains properties to define the return properties.</param>
        /// <returns>Returns a list of all links that are currently in the download list.</returns>
        public async Task<IReadOnlyList<DownloadLink>> QueryLinks(LinkQuery queryLink)
        {
            if (queryLink == null)
                queryLink = new LinkQuery();
            var json = JsonConvert.SerializeObject(queryLink);
            var param = new[] { json };

            var response =
                await CallAction<IReadOnlyList<DownloadLink>>("/downloadsV2/queryLinks", param);
            return response;
        }

    }
}
