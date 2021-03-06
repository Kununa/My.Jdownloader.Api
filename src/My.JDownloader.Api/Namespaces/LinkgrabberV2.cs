﻿using System.Collections.Generic;
using My.JDownloader.Api.ApiObjects;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.LinkgrabberV2;
using Newtonsoft.Json;
using System.Threading.Tasks;
using My.JDownloader.Api.ApiObjects.Login;

namespace My.JDownloader.Api.Namespaces
{
    public class LinkGrabberV2 : NamespaceBase
    {
        public LinkGrabberV2(DeviceObject device, LoginObject loginObject) : base(device, loginObject, "linkgrabberv2")
        {
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
        /// <param name="jobId">The jobId you wnat to abort.</param>
        /// <returns>True if successfull.</returns>
        public async Task<bool> Abort(long jobId)
        {
            var param = new[] {jobId};
            if (jobId == -1)
                param = null;
            var response = await CallAction<bool>("abort", param);
            return response;
        }

        /// <summary>
        /// Adds a container to the linkcollector list.
        /// </summary>
        /// <param name="type">The value can be: DLC, RSDF, CCF or CRAWLJOB</param>
        /// <param name="content">File as dataurl. https://de.wikipedia.org/wiki/Data-URL </param>
        public async Task AddContainer(ContainerType type, string content)
        {
            var containerObject = new AddContainerObject
            {
                Type = type.ToString(),
                Content = content
            };
            var json = JsonConvert.SerializeObject(containerObject);
            var param = new[] {json};
            await CallAction<object>("addContainer", param);
        }

        /// <summary>
        /// Adds the download links
        /// </summary>
        /// <param name="links">Links to add to the Linkgrabber</param>
        /// <returns>id of package</returns>
        public async Task<long> AddLinks(IEnumerable<string> links)
        {
            return await AddLinks(new AddLinksQuery()
            {
                Links = string.Join(";", links)
            });
        }

        /// <summary>
        /// Adds download links using a request object for more configuration
        /// </summary>
        /// <param name="requestObject">Contains informations like the links itself or the priority.</param>
        /// <returns>id of package</returns>
        public async Task<long> AddLinks(AddLinksQuery requestObject)
        {
            // if (requestObject.Links != null)
            //     requestObject.Links = requestObject.Links.Replace(";", "\\r\\n");
            var json = JsonConvert.SerializeObject(requestObject, Formatting.Indented, new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Include
            });
            var param = new[] {json};
            var response = await CallAction<LinkCollectingJob>("addLinks", param);
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
        public async Task AddVariantCopy(long linkId, long destinationAfterLinkId, long destinationPackageId,
            string variantId)
        {
            var param = new[]
                {linkId.ToString(), destinationAfterLinkId.ToString(), destinationPackageId.ToString(), variantId};
            await CallAction<DefaultReturnObject>("addVariantCopy", param);
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
        public async Task CleanUp(long[] linkIds, long[] packageIds, CleanUpActionType action, CleanUpModeType mode,
            CleanUpSelectionType selection)
        {
            var param = new object[] {linkIds, packageIds, action, mode, selection};
            await CallAction<object>("cleanUp", param);
        }

        /// <summary>
        /// Clears the downloader list.
        /// </summary>
        /// <returns>True if successfull</returns>
        public async Task ClearList()
        {
            await CallAction<object>("clearList");
        }

        /// <summary>
        /// Not documented what it really does.
        /// </summary>
        /// <param name="structureWatermark"></param>
        /// <returns></returns>
        public async Task<long> GetChildrenChanged(long structureWatermark)
        {
            var response = await CallAction<long>("getChildrenChanged");

            return response;
        }

        /// <summary>
        /// Gets the selection base of the download folder history.
        /// </summary>
        /// <returns>An array which contains the download folder history.</returns>
        public async Task<List<string>> GetDownloadFolderHistorySelectionBase()
        {
            var response = await CallAction<List<string>>("getDownloadFolderHistorySelectionBase");
            return response;
        }

        // TODO: Describe what this function does.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="links"></param>
        /// <param name="afterLinkId"></param>
        /// <param name="destPackageId"></param>
        /// <returns></returns>
        public async Task<IReadOnlyDictionary<string, IReadOnlyList<long>>> GetDownloadUrls(long[] links, long afterLinkId, long destPackageId)
        {
            var response = await CallAction<Dictionary<string, IReadOnlyList<long>>>("getDownloadUrls");
            return response;
        }

        /// <summary>
        /// Checks how many packages are inside the linkcollector.
        /// </summary>
        /// <returns>The amount of links which are in the linkcollector.</returns>
        public async Task<int> GetPackageCount()
        {
            var response = await CallAction<int>("getPackageCount");
            return response;
        }

        /// <summary>
        /// Gets variants of the link.
        /// </summary>
        /// <param name="linkId">The link id you want to get the variants of.</param>
        /// <returns>Returns variants of this link.</returns>
        public async Task<IReadOnlyList<GetVariantsReturnObject>> GetVariants(long linkId)
        {
            var response = await CallAction<List<GetVariantsReturnObject>>("getVariants");
            return response;
        }

        /// <summary>
        /// Checks if the JDownloader client is still collecting files from links.
        /// </summary>
        /// <returns>Returns true or false. Depending on if the client is still collecting files.</returns>
        public async Task<bool> IsCollecting()
        {
            var response = await CallAction<bool>("isCollection");
            return response;
        }

        /// <summary>
        /// Moves one or multiple links after another link or inside a package.
        /// </summary>
        /// <param name="linkIds">The ids of the links you want to move.</param>
        /// <param name="afterLinkId">The id of the link you want to move the other links to.</param>
        /// <param name="destPackageId">The id of the package where you want to add the links to.</param>
        /// <returns>True if successfull.</returns>
        public async Task MoveLinks(long[] linkIds, long? afterLinkId, long destPackageId)
        {
            var param = new object[] {linkIds, afterLinkId, destPackageId};
            await CallAction<object>("moveLinks", param);
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

        /// <summary>
        /// Moves one or multiple links/packages to the download list.
        /// </summary>
        /// <param name="linkIds">The ids of the links you want to move.</param>
        /// <param name="packageIds">The ids of the packages you want to move.</param>
        /// <returns>True if successfull.</returns>
        public async Task MoveToDownloadlist(long[] linkIds, long[] packageIds)
        {
            var param = new[] {linkIds, packageIds};
            await CallAction<object>("moveToDownloadlist", param);
        }

        /// <summary>
        /// Move one or multiple links/packages to a new package.
        /// </summary>
        /// <param name="linkIds">The ids of the links you want to move.</param>
        /// <param name="packageIds">The ids of the packages you want to move.</param>
        /// <param name="newPackageName">The name of the new package.</param>
        /// <param name="downloadPath">The download path.</param>
        /// <returns>True if successfull.</returns>
        public async Task MoveToNewPackage(long[] linkIds, long[] packageIds, string newPackageName, string downloadPath)
        {
            var param = new object[] {linkIds, packageIds, newPackageName, downloadPath};
            await CallAction<object>("movetoNewPackage", param);
        }

        /// <summary>
        /// Gets links that are currently in the linkcollector.
        /// </summary>
        /// <returns>Returns a list of all links that are currently in the linkcollector list.</returns>
        public async Task<IReadOnlyList<QueryLinksResponseObject>> QueryLinks(CrawledLinkQuery query)
        {
            var json = JsonConvert.SerializeObject(query);
            var param = new[] {json};
            var response = await CallAction<List<QueryLinksResponseObject>>("queryLinks", param);
            return response;
        }

        /// <summary>
        /// Gets a list of available packages that are currently in the linkcollector.
        /// </summary>
        /// <param name="requestObject">The request object which contains properties to define the return properties.</param>
        /// <returns>Returns a list of all available packages.</returns>
        public async Task<IReadOnlyList<CrawledPackage>> QueryPackages(CrawledPackageQuery requestObject)
        {
            var json = JsonConvert.SerializeObject(requestObject);
            var param = new[] {json};
            var response = await CallAction<List<CrawledPackage>>("queryPackages", param);
            return response;
        }

        /// <summary>
        /// Allows you to change the download directory of multiple packages.
        /// </summary>
        /// <param name="directory">The new download directory.</param>
        /// <param name="packageIds">The ids of the packages.</param>
        /// <returns>True if successfull</returns>
        public async Task SetDownloadDirectory(string directory, long[] packageIds)
        {
            var param = new object[] {directory, packageIds};
            await CallAction<object>("setDownloadDirectory", param);
        }

        public async Task RemoveLinks(long[] linkIds, long[] packageIds)
        {
            var param = new object[] {linkIds, packageIds};
            await CallAction<object>("removeLinks", param);
        }
    }
}