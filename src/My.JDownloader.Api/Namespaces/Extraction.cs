﻿using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Extraction;
using System.Collections.Generic;
using System.Threading.Tasks;
using My.JDownloader.Api.ApiObjects.Login;

namespace My.JDownloader.Api.Namespaces
{
    public class Extraction : NamespaceBase
    {
        public Extraction(DeviceObject device, LoginObject loginObject) : base(device, loginObject, "extraction") { }

        /// <summary>
        /// Adds an archive password to the client.
        /// </summary>
        /// <param name="password">The password to add.</param>
        /// <returns>True if successfull.</returns>
        public async Task<bool> AddArchivePassword(string password)
        {
            var param = new[] { password };
            var response = await CallAction<bool>("addArchivePassword", param);

            return response;
        }

        /// <summary>
        /// Cancels an extraction process.
        /// </summary>
        /// <param name="controllerId">The id of the controll you want to cancel.</param>
        /// <returns>True if successfull.</returns>
        public async Task<bool> CancelExtraction(string controllerId)
        {
            var param = new[] { controllerId };
            var response = await CallAction<bool>("cancelExtraction", param);

            return response;
        }

        public async Task<ArchiveStatus> GetArchiveInfo(long[] linkIds, long[] packageIds)
        {
            var param = new[] { linkIds, packageIds };
            var response = await CallAction<ArchiveStatus>("cancelExtraction", param);

            return response;
        }

        public async Task<IReadOnlyList<ArchiveSettings>> GetArchiveSettings(string[] archiveIds)
        {
            var param = new[] { archiveIds };
            var response = await CallAction<List<ArchiveSettings>>("cancelExtraction", param);

            return response;
        }

        public async Task<IReadOnlyList<ArchiveStatus>> GetQueue()
        {
            var response = await CallAction<List<ArchiveStatus>>("cancelExtraction");
            return response;
        }

        public async Task<bool> SetArchiveSettings(string archiveId, ArchiveSettings archiveSettings)
        {
            var param = new object[] { archiveId, archiveSettings };
            var response = await CallAction<bool>("cancelExtraction", param);

            return response;
        }

        public async Task<IReadOnlyDictionary<string, bool>> StartExtractionNow(long[] linkIds, long[] packageIds)
        {
            var param = new[] { linkIds, packageIds };
            var response = await CallAction<Dictionary<string, bool>>("cancelExtraction", param);

            return response;
        }

    }
}
