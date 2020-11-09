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
        public DownloadsV2(DeviceObject device, LoginObject loginObject) : base(device, loginObject, "downloadsV2")
        {
        }

        public async Task<bool> Cleanup(long[] linkIds, long[] packageIds, Enums.Action action, Enums.Mode mode, Enums.SelectionType selectionType)
        {
            var param = new object[] {linkIds, packageIds, action.ToString(), mode.ToString(), selectionType.ToString()};
            var response = await CallAction<object>("cleanup", param);
            return !string.IsNullOrEmpty(response.ToString());
        }

        public async Task<bool> ForceDownload(long[] linkIds, long[] packageIds)
        {
            var param = new object[] {linkIds, packageIds};
            var response = await CallAction<bool>("queryPackages", param);
            return response;
        }

        /*public async Task<bool> GetDownloadUrls(long[] linkIds, long[] packageIds, UrlDisplayTypeStorable[] urlDisplayTypeStorable)
        {
            var param = new object[] { linkIds, packageIds };

            var response =
                await JDownloaderApiHandler.CallAction<bool>(_Device, "queryPackages", param,
                    JDownloaderHandler.LoginObject);
            return response;
        }*/

        /// <summary>
        /// Gets the stop mark as long.
        /// </summary>
        /// <returns>The stop mark as long.</returns>
        public async Task<long> GetStopMark()
        {
            var response = await CallAction<long>("getStopMark");
            return response;
        }

        /// <summary>
        /// Gets informations about a stop marked link.
        /// </summary>
        /// <returns>Returns informations about a stop marked link.</returns>
        public async Task<DownloadLink> GetStopMarkedLink()
        {
            var response = await CallAction<DownloadLink>("getStopMarkedLink");
            return response;
        }

        public async Task<long> GetStructureChangeCounter(long oldCounterValue)
        {
            var param = new object[] {oldCounterValue};
            var response = await CallAction<long>("getStructureChangeCounter", param);
            return response;
        }

        public async Task MoveLinks(long[] packageIds, long afterLinkID, long? destPackageId)
        {
            var param = new object[] {packageIds, afterLinkID, destPackageId};

            await CallAction<object>(" moveLinks", param);
        }

        /// <summary>
        /// Moves one or multiple packages after antoher package.
        /// </summary>
        /// <param name="packageIds">The ids of the packages you want to move.</param>
        /// <param name="afterDestPackageId">The id of the package you want to move the others to.</param>
        /// <returns>True if successfull.</returns>
        public async Task MovePackages(long[] packageIds, long afterDestPackageId)
        {
            var param = new object[] {packageIds, afterDestPackageId};

            await CallAction<object>("movePackages", param);
        }

        public async Task MovetoNewPackage(long[] linkIds, long[] pkgIds, string newPkgName, string downloadPath)
        {
            var param = new object[] {linkIds, pkgIds, newPkgName, downloadPath};

            await CallAction<object>("movetoNewPackage", param);
        }

        public async Task<int> PackageCount()
        {
            var response = await CallAction<int>("packageCount");
            return response;
        }

        /// <summary>
        /// Gets all links that are currently in the download list.
        /// </summary>
        /// <param name="queryLink">The request object which contains properties to define the return properties.</param>
        /// <returns>Returns a list of all links that are currently in the download list.</returns>
        public async Task<IReadOnlyList<DownloadLink>> QueryLinks(LinkQuery? queryLink)
        {
            queryLink ??= new LinkQuery();
            var json = JsonConvert.SerializeObject(queryLink);
            var param = new[] {json};

            var response =
                await CallAction<List<DownloadLink>>("queryLinks", param);
            return response;
        }

        /// <summary>
        /// Gets a list of available packages that are currently in the download list.
        /// </summary>
        /// <param name="requestObject">The request object which contains properties to define the return properties.</param>
        /// <returns>Returns a list of all available packages.</returns>
        public async Task<IReadOnlyList<FilePackage>> QueryPackages(PackageQuery? requestObject)
        {
            if (requestObject == null)
                requestObject = new PackageQuery();
            var json = JsonConvert.SerializeObject(requestObject);
            var param = new[] {json};

            var response =
                await CallAction<List<FilePackage>>("queryPackages", param);
            return response;
        }

        /// <summary>
        /// Removes one or multiple links or packages.
        /// </summary>
        /// <param name="linkIds">The ids of the links you want to remove.</param>
        /// <param name="packageIds">The ids of the packages you want to remove.</param>
        /// <returns>True if successfull.</returns>
        public async Task RemoveLinks(long[]? linkIds, long[]? packageIds)
        {
            var param = new object[] {linkIds ?? new long[0], packageIds ?? new long[0]};

            await CallAction<object>("removeLinks", param);
        }

        public async Task<int> RemoveStopMark()
        {
            var response = await CallAction<int>("removeStopMark");
            return response;
        }

        public async Task RenameLink(long linkId, string newName)
        {
            var param = new object[] {linkId, newName};
            await CallAction<object>("renameLink", param);
        }

        public async Task RenamePackage(long packageId, string newName)
        {
            var param = new object[] {packageId, newName};

            await CallAction<object>("renamePackage", param);
        }

        /// <summary>
        /// Removes one or multiple links or packages.
        /// </summary>
        /// <param name="linkIds">The ids of the links you want to remove.</param>
        /// <param name="packageIds">The ids of the packages you want to remove.</param>
        /// <returns>True if successfull.</returns>
        public async Task ResetLinks(long[]? linkIds = null, long[]? packageIds = null)
        {
            var param = new object[] {linkIds ?? new long[0], packageIds ?? new long[0]};

            await CallAction<object>("resetLinks", param);
        }

        public async Task ResumeLinks(long[]? linkIds = null, long[]? packageIds = null)
        {
            var param = new object[] {linkIds ?? new long[0], packageIds ?? new long[0]};

            await CallAction<object>("resumeLinks", param);
        }

        public async Task SetDownloadDirectory(string directory, long[] packageIds)
        {
            var param = new object[] {directory, packageIds};
            await CallAction<object>("setDownloadDirectory", param);
        }

        public async Task SetDownloadPassword(string pass, long[]? linkIds = null, long[]? packageIds = null)
        {
            var param = new object[] {linkIds ?? new long[0], packageIds ?? new long[0], pass};
            await CallAction<bool>("setDownloadPassword", param);
        }

        public async Task SetEnabled(bool enabled, long[]? linkIds = null, long[]? packageIds = null)
        {
            var param = new object[] {enabled, linkIds ?? new long[0], packageIds ?? new long[0]};

            var response =
                await CallAction<object>("setEnabled", param);
        }

        public async Task SetPriority(Enums.PriorityType priority, long[] linkIds = null!, long[] packageIds = null)
        {
            var param = new object[] {priority, linkIds, packageIds};

            var response =
                await CallAction<object>("setPriority", param);
        }

        public async Task SetStopMark(long? linkId, long? packageId)
        {
            var param = new object?[] {linkId, packageId};

            await CallAction<object>("setStopMark", param);
        }

        public async Task SplitPackageByHoster(long[]? linkIds = null, long[]? pkgIds = null)
        {
            var param = new object[] {linkIds ?? new long[0], pkgIds ?? new long[0]};

            await CallAction<object>("splitPackageByHoster", param);
        }

        public async Task StartOnlineStatusCheck(long[]? linkIds = null, long[]? packageIds = null)
        {
            var param = new object[] {linkIds ?? new long[0], packageIds ?? new long[0]};

            await CallAction<object>("startOnlineStatusCheck", param);
        }

        public async Task Unskip(Enums.Reason filterByReason, long[]? linkIds = null, long[]? packageIds = null)
        {
            var param = new object[] {packageIds ?? new long[0], linkIds ?? new long[0], filterByReason};

            await CallAction<object>("setEnabled", param);
        }
    }
}