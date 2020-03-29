using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects.Devices;
using System.Threading.Tasks;
using My.JDownloader.Api.ApiObjects.Login;

namespace My.JDownloader.Api.Namespaces
{
    public class Jd : NamespaceBase
    {
        public Jd(DeviceObject device, LoginObject loginObject) : base(device, loginObject) { }

        /// <summary>
        /// Keep an eye on your JDownloader Client ;)
        /// </summary>
        public async Task<bool> DoSomethingCool()
        {
            var response = await CallAction<object>("/jd/doSomethingCool", null);
            return response != null;
        }

        /// <summary>
        /// Gets the core revision of the jdownloader client.
        /// </summary>
        /// <returns>Returns the core revision of the jdownloader client.</returns>
        public async Task<int> GetCoreRevision()
        {
            var response = await CallAction<int>("/jd/getCoreRevision", null);

            return response;
        }

        /// <summary>
        /// Refreshes the plugins.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public async Task<bool> RefreshPlugins()
        {
            var response = await CallAction<bool>("/jd/refreshPlugins", null);

            return response;
        }

        /// <summary>
        /// Creates the sum of two numbers.
        /// </summary>
        /// <param name="a">First number.</param>
        /// <param name="b">Second number.</param>
        /// <returns>Returns the sum of two numbers.</returns>
        public async Task<int> Sum(int a, int b)
        {
            var param = new[] { a, b };
            var response = await CallAction<int>("/jd/sum", param);

            return response;
        }

        /// <summary>
        /// Gets the current uptime of the JDownloader client.
        /// </summary>
        /// <returns>The current uptime of the JDownloader client as long.</returns>
        public async Task<long> Uptime()
        {
            var response = await CallAction<long>("/jd/uptime", null);

            return response;
        }

        /// <summary>
        /// Gets the version of the JDownloader client.
        /// </summary>
        /// <returns>The current version of the JDownloader client.</returns>
        public async Task<long> Version()
        {
            var response = await CallAction<long>("/jd/version", null);

            return response;
        }
    }
}
