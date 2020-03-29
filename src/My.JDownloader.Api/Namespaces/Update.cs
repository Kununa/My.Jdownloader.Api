using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects.Devices;
using System.Threading.Tasks;
using My.JDownloader.Api.ApiObjects.Login;

namespace My.JDownloader.Api.Namespaces
{
    public class Update : NamespaceBase
    {
        public Update(DeviceObject device, LoginObject loginObject) : base(device, loginObject, "update") { }

        /// <summary>
        /// Checks if the client has an update available.
        /// </summary>
        /// <returns>True if an update is available.</returns>
        public async Task<bool> IsUpdateAvailable()
        {
            var response = await CallAction<bool>("isUpdateAvailable", null);

            return response;
        }

        /// <summary>
        /// Restarts the client and starts the update.
        /// </summary>
        public async Task RestartAndUpdate()
        {
            await CallAction<object>("restartAndUpdate", null);
        }

        /// <summary>
        /// Start the update check on the client.
        /// </summary>
        public async Task RunUpdateCheck()
        {
            await CallAction<object>("runUpdateCheck", null);
        }
    }
}
