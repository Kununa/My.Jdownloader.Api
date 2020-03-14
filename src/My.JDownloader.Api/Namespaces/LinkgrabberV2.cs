using System.Collections.Generic;
using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.LinkgrabberV2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace My.JDownloader.Api.Namespaces
{
    public class LinkGrabberV2
    {
        private readonly DeviceObject _Device;

        internal LinkGrabberV2(DeviceObject device)
        {
            _Device = device;
        }

        /// <summary>
        /// Aborts the linkgrabber process.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public async Task<bool> Abort()
        {
            return await Abort(-1);
        }

        /// <summary>
        /// Aborts the linkgrabber process for a specific job.
        /// </summary>
        /// <param name="jobID">The jobId you wnat to abort.</param>
        /// <returns>True if successfull.</returns>
        public async Task<bool> Abort(long jobID)
        {
            var param = new[] { jobID };
            if (jobID == -1)
                param = null;

            var response = JDownloaderApiHandler.CallAction<bool>(_Device, "/linkgrabberv2/abort",
                param, JDownloaderHandler.LoginObject);

            return await response;
        }

        /// <summary>
        /// Adds a container to the linkcollector list.
        /// </summary>
        /// <param name="type">The value can be: DLC, RSDF, CCF or CRAWLJOB</param>
        /// <param name="content">File as dataurl. https://de.wikipedia.org/wiki/Data-URL </param>
        public bool AddContainer(ContainerType type, string content)
        {
            var containerObject = new AddContainerObject
            {
                Type = type.ToString(),
                Content = content
            };

            var json = JsonConvert.SerializeObject(containerObject);
            var param = new[] { json };
            var response = JDownloaderApiHandler.CallAction<object>(_Device, "/linkgrabberv2/addContainer",
                param, JDownloaderHandler.LoginObject);
            return response != null;
        }

        /// <summary>
        /// Adds the download links
        /// </summary>
        /// <param name="links">Links to add to the Linkgrabber</param>
        /// <returns>id of package</returns>
        public long AddLinks(string[] links)
        {
            return AddLinks(new AddLinksQuery()
            {
                Links = string.Join(";", links)
            });
        }

        /// <summary>
        /// Adds download links using a request object for more configuration
        /// </summary>
        /// <param name="requestObject">Contains informations like the links itself or the priority.</param>
        /// <returns>id of package</returns>
        public long AddLinks(AddLinksQuery requestObject)
        {
            // if (requestObject.Links != null)
            //     requestObject.Links = requestObject.Links.Replace(";", "\\r\\n");
            var json = JsonConvert.SerializeObject(requestObject, Formatting.Indented, new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Include
            });
            var param = new[] { json };
            var response = JDownloaderApiHandler.CallAction<LinkCollectingJob>(_Device, "/linkgrabberv2/addLinks",
                param, JDownloaderHandler.LoginObject);
            return response.Id;
        }

        /// <summary>
        /// Adds a variant copy of the link.
        /// </summary>
        /// <param name="linkId">The link id you want to copy.</param>
        /// <param name="destinationAfterLinkId"></param>
        /// <param name="destinationPackageId"></param>
        /// <param name="variantId"></param>
        /// <returns>True if successfull.</returns>
        public bool AddVariantCopy(long linkId, long destinationAfterLinkId, long destinationPackageId,
            string variantId)
        {
            var param = new[]
                {linkId.ToString(), destinationAfterLinkId.ToString(), destinationPackageId.ToString(), variantId};
            var response = JDownloaderApiHandler.CallAction<DefaultReturnObject>(_Device, "/linkgrabberv2/addVariantCopy",
                param, JDownloaderHandler.LoginObject);
            return response != null;
        }

        /// <summary>
        /// Cleans up the downloader list.
        /// </summary>
        /// <param name="linkIds">Ids of the link you may want to clear.</param>
        /// <param name="packageIds">Ids of the packages you may want to clear.</param>
        /// <param name="action">The action type.</param>
        /// <param name="mode">The mode type.</param>
        /// <param name="selection">The selection Type.</param>
        /// <returns>True if successfull.</returns>
        public bool CleanUp(long[] linkIds, long[] packageIds, CleanUpActionType action, CleanUpModeType mode,
            CleanUpSelectionType selection)
        {
            var param = new object[] { linkIds, packageIds, action, mode, selection };
            var response =
                JDownloaderApiHandler.CallAction<object>(_Device, "/linkgrabberv2/cleanUp", param,
                    JDownloaderHandler.LoginObject);
            if (response == null)
                return false;
            return true;
        }

        /// <summary>
        /// Clears the downloader list.
        /// </summary>
        /// <returns>True if successfull</returns>
        public bool ClearList()
        {
            var response =
                JDownloaderApiHandler.CallAction<object>(_Device, "/linkgrabberv2/clearList", null,
                    JDownloaderHandler.LoginObject);
            if (response == null)
                return false;
            return true;
        }

        /// <summary>
        /// Not documented what it really does.
        /// </summary>
        /// <param name="structureWatermark"></param>
        /// <returns></returns>
        public async Task<long> GetChildrenChanged(long structureWatermark)
        {
            var response =
                JDownloaderApiHandler.CallAction<long>(_Device, "/linkgrabberv2/getChildrenChanged", null,
                    JDownloaderHandler.LoginObject);

            return await response;
        }

        /// <summary>
        /// Gets the selection base of the download folder history.
        /// </summary>
        /// <returns>An array which contains the download folder history.</returns>
        public async Task<string[]> GetDownloadFolderHistorySelectionBase()
        {
            var response = JDownloaderApiHandler.CallAction<string[]>(_Device,
                "/linkgrabberv2/getDownloadFolderHistorySelectionBase", null, JDownloaderHandler.LoginObject);

            return await response;
        }

        // TODO: Describe what this function does.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="links"></param>
        /// <param name="afterLinkId"></param>
        /// <param name="destPackageId"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, List<long>>> GetDownloadUrls(long[] links, long afterLinkId, long destPackageId)
        {
            var response = JDownloaderApiHandler.CallAction<DefaultReturnObject>(_Device, "/linkgrabberv2/getDownloadUrls", null,
                JDownloaderHandler.LoginObject);

            var tmp = (JObject)(await response)?.Data;
            return tmp?.ToObject<Dictionary<string, List<long>>>();
        }

        /// <summary>
        /// Checks how many packages are inside the linkcollector.
        /// </summary>
        /// <returns>The amount of links which are in the linkcollector.</returns>
        public async Task<int> GetPackageCount()
        {
            var response =
                JDownloaderApiHandler.CallAction<int>(_Device, "/linkgrabberv2/getPackageCount", null,
                    JDownloaderHandler.LoginObject);
            return await response;
        }

        /// <summary>
        /// Gets variants of the link.
        /// </summary>
        /// <param name="linkId">The link id you want to get the variants of.</param>
        /// <returns>Returns variants of this link.</returns>
        public async Task<GetVariantsReturnObject[]> GetVariants(long linkId)
        {
            var response =
                JDownloaderApiHandler.CallAction<GetVariantsReturnObject[]>(_Device, "/linkgrabberv2/getVariants", null,
                    JDownloaderHandler.LoginObject);

            return await response;
        }

        /// <summary>
        /// Checks if the JDownloader client is still collecting files from links.
        /// </summary>
        /// <returns>Returns true or false. Depending on if the client is still collecting files.</returns>
        public bool IsCollecting()
        {
            var response =
                JDownloaderApiHandler.CallAction<object>(_Device, "/linkgrabberv2/isCollection", null,
                    JDownloaderHandler.LoginObject);
            if (response == null)
                return false;
            return true;
        }

        /// <summary>
        /// Moves one or multiple links after another link or inside a package.
        /// </summary>
        /// <param name="linkIds">The ids of the links you want to move.</param>
        /// <param name="afterLinkId">The id of the link you want to move the other links to.</param>
        /// <param name="destPackageId">The id of the package where you want to add the links to.</param>
        /// <returns>True if successfull.</returns>
        public bool MoveLinks(long[] linkIds, long afterLinkId, long destPackageId)
        {
            var param = new object[] { linkIds, afterLinkId, destPackageId };

            var response =
                JDownloaderApiHandler.CallAction<object>(_Device, "/linkgrabberv2/moveLinks", param,
                    JDownloaderHandler.LoginObject);
            if (response == null)
                return false;
            return true;
        }

        /// <summary>
        /// Moves one or multiple packages after antoher package.
        /// </summary>
        /// <param name="packageIds">The ids of the packages you want to move.</param>
        /// <param name="afterDestPackageId">The id of the package you want to move the others to.</param>
        /// <returns>True if successfull.</returns>
        public bool MovePackages(long[] packageIds, long afterDestPackageId)
        {
            var param = new object[] { packageIds, afterDestPackageId };

            var response =
                JDownloaderApiHandler.CallAction<object>(_Device, "/linkgrabberv2/movePackages", param,
                    JDownloaderHandler.LoginObject);
            if (response == null)
                return false;
            return true;
        }

        /// <summary>
        /// Moves one or multiple links/packages to the download list.
        /// </summary>
        /// <param name="linkIds">The ids of the links you want to move.</param>
        /// <param name="packageIds">The ids of the packages you want to move.</param>
        /// <returns>True if successfull.</returns>
        public bool MoveToDownloadlist(long[] linkIds, long[] packageIds)
        {
            var param = new[] { linkIds, packageIds };

            var response =
                JDownloaderApiHandler.CallAction<object>(_Device, "/linkgrabberv2/moveToDownloadlist", param,
                    JDownloaderHandler.LoginObject);
            if (response == null)
                return false;
            return true;
        }

        /// <summary>
        /// Move one or multiple links/packages to a new package.
        /// </summary>
        /// <param name="linkIds">The ids of the links you want to move.</param>
        /// <param name="packageIds">The ids of the packages you want to move.</param>
        /// <param name="newPackageName">The name of the new package.</param>
        /// <param name="downloadPath">The download path.</param>
        /// <returns>True if successfull.</returns>
        public bool MoveToNewPackage(long[] linkIds, long[] packageIds, string newPackageName, string downloadPath)
        {
            var param = new object[] { linkIds, packageIds, newPackageName, downloadPath };

            var response =
                JDownloaderApiHandler.CallAction<object>(_Device, "/linkgrabberv2/movetoNewPackage", param,
                    JDownloaderHandler.LoginObject);
            if (response == null)
                return false;
            return true;
        }

        /// <summary>
        /// Gets all links that are currently in the linkcollector.
        /// </summary>
        /// <param name="maxResults">Maximum number of return values.</param>
        /// <returns>Returns a list of all links that are currently in the linkcollector list.</returns>
        public async Task<List<QueryLinksResponseObject>> QueryLinks(CrawledLinkQuery query)
        {
            var json = JsonConvert.SerializeObject(query);
            var param = new[] { json };

            var response =
                await JDownloaderApiHandler.CallAction<CrawledLinkObject>(_Device, "/linkgrabberv2/queryLinks", param,
                    JDownloaderHandler.LoginObject).ConfigureAwait(false);
            return response?.Data;
        }

        /// <summary>
        /// Gets a list of available packages that are currently in the linkcollector.
        /// </summary>
        /// <param name="requestObject">The request object which contains properties to define the return properties.</param>
        /// <returns>Returns a list of all available packages.</returns>
        public async Task<List<CrawledPackage>> QueryPackages(CrawledPackageQuery requestObject)
        {
            var json = JsonConvert.SerializeObject(requestObject);
            var param = new[] { json };

            var response =
                JDownloaderApiHandler.CallAction<List<CrawledPackage>>(_Device, "/linkgrabberv2/queryPackages", param,
                    JDownloaderHandler.LoginObject);
            return await response;
        }

        /// <summary>
        /// Allows you to change the download directory of multiple packages.
        /// </summary>
        /// <param name="directory">The new download directory.</param>
        /// <param name="packageIds">The ids of the packages.</param>
        /// <returns>True if successfull</returns>
        public bool SetDownloadDirectory(string directory, long[] packageIds)
        {
            var param = new object[] { directory, packageIds };
            var response =
                JDownloaderApiHandler.CallAction<object>(_Device, "/linkgrabberv2/setDownloadDirectory", param,
                    JDownloaderHandler.LoginObject);
            return response != null;
        }
    }
}