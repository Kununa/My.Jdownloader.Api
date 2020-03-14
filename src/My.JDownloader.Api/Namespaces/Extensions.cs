using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Extensions;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace My.JDownloader.Api.Namespaces
{
    public class Extensions
    {
        private readonly DeviceObject _Device;

        internal Extensions(DeviceObject device)
        {
            _Device = device;
        }

        /// <summary>
        /// Installs an extension to the client.
        /// </summary>
        /// <param name="extensionId">The id of the extension you want to install</param>
        /// <returns>True if successfull</returns>
        public async Task<bool> Install(string extensionId)
        {
            var param = new[] { extensionId };
            var response = await JDownloaderApiHandler.CallAction<bool>(_Device, "/extensions/install",
                param, JDownloaderHandler.LoginObject);

            return response;
        }

        /// <summary>
        /// Checks if the extension is enabled.
        /// </summary>
        /// <param name="className">Name/id of the extension.</param>
        /// <returns>True if enabled.</returns>
        public async Task<bool> IsEnabled(string className)
        {
            var param = new[] { className };
            var response = await JDownloaderApiHandler.CallAction<bool>(_Device, "/extensions/isEnabled",
                param, JDownloaderHandler.LoginObject);

            return response;
        }

        /// <summary>
        /// Checks if the extension is installed.
        /// </summary>
        /// <param name="extensionId">The id of the extension you want to install.</param>
        /// <returns>True if successfull</returns>
        public async Task<bool> IsInstalled(string extensionId)
        {
            var param = new[] { extensionId };
            var response = await JDownloaderApiHandler.CallAction<bool>(_Device, "/extensions/isInstalled",
                param, JDownloaderHandler.LoginObject);

            return response;
        }

        /// <summary>
        /// Gets all extensions that are available.
        /// </summary>
        /// <param name="requestObject">The request object which contains informations about which properties are returned.</param>
        /// <returns>A list of all extensions that are available.</returns>
        public async Task<ExtensionResponseObject[]> List(ExtensionRequestObject requestObject)
        {
            var json = JsonConvert.SerializeObject(requestObject);
            var param = new[] { json };
            var response = await JDownloaderApiHandler.CallAction<ExtensionResponseObject[]>(_Device, "/extensions/list",
                param, JDownloaderHandler.LoginObject);

            return response;

        }

        /// <summary>
        /// Enableds or disables an extension
        /// </summary>
        /// <param name="className">Name/id of the extension.</param>
        /// <param name="enabled">If true the extension gets enabled else it disables it.</param>
        /// <returns>True if successfull</returns>
        public async Task<bool> SetEnabled(string className, bool enabled)
        {
            var param = new[] { className, enabled.ToString() };
            var response = await JDownloaderApiHandler.CallAction<DefaultReturnObject>(_Device, "/extensions/setEnabled",
                param, JDownloaderHandler.LoginObject);

            return response?.Data != null;
        }
    }
}
