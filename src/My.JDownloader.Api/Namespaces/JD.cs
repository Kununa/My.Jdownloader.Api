using My.JDownloader.Api.ApiObjects.Devices;
using System.Threading.Tasks;
using My.JDownloader.Api.ApiObjects.Login;

namespace My.JDownloader.Api.Namespaces
{
    public class Jd : NamespaceBase
    {
        public Jd(DeviceObject device, LoginObject loginObject) : base(device, loginObject, "jd") { }

        /// <summary>
        /// Keep an eye on your JDownloader Client ;)
        /// </summary>
        public async Task DoSomethingCool()
        {
            await CallAction<object>("doSomethingCool");
        }

        /// <summary>
        /// Gets the core revision of the jdownloader client.
        /// </summary>
        /// <returns>Returns the core revision of the jdownloader client.</returns>
        public async Task<int> GetCoreRevision()
        {
            var response = await CallAction<int>("getCoreRevision");
            return response;
        }

        /// <summary>
        /// Refreshes the plugins.
        /// </summary>
        /// <returns>True if successfull.</returns>
        public async Task<bool> RefreshPlugins()
        {
            var response = await CallAction<bool>("refreshPlugins");
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
            var response = await CallAction<int>("sum", param);
            return response;
        }

        /// <summary>
        /// Gets the current uptime of the JDownloader client.
        /// </summary>
        /// <returns>The current uptime of the JDownloader client as long.</returns>
        public async Task<long> Uptime()
        {
            var response = await CallAction<long>("uptime");
            return response;
        }

        /// <summary>
        /// Gets the version of the JDownloader client.
        /// </summary>
        /// <returns>The current version of the JDownloader client.</returns>
        public async Task<long> Version()
        {
            var response = await CallAction<long>("version");
            return response;
        }
    }
}
