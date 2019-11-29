using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Events;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace My.JDownloader.Api.Namespaces
{
    public class Events
    {
        private readonly DeviceObject device;

        public List<long> SubscriptionIDs;

        internal Events(DeviceObject device)
        {
            this.device = device;
            SubscriptionIDs = new List<long>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptionid">id that identified the subscription the call should be applied to</param>
        /// <param name="subscriptions">Array of Strings used as Regex to query for events.</param>
        /// <param name="exclusions">Array of Strings used as Regex to enable to further narrow down the subscribed events</param>
        /// <returns></returns>
        public async Task<SubscriptionResponse> AddSubscription(long subscriptionid, string[] subscriptions, string[] exclusions)
        {
            var param = new object[] { subscriptionid, subscriptions, exclusions };
            var response = await JDownloaderApiHandler.CallAction<SubscriptionResponse>(device, "/events/addsubscription",
                param, JDownloaderHandler.LoginObject);
            return response;
        }

        public async Task<SubscriptionResponse> Subscribe(string[] subscriptions = null, string[] exclusions = null)
        {
            var param = new object[] { new[] { "STOPPED" }, exclusions};
            var response = await JDownloaderApiHandler.CallAction<SubscriptionResponse>(device, "/events/subscribe",
                param, JDownloaderHandler.LoginObject);
            if (!SubscriptionIDs.Contains(response.SubscriptionId))
                SubscriptionIDs.Add(response.SubscriptionId);

            return response;
        }

        public async Task<SubscriptionEventObject[]> Listen(long subscriptionid)
        {
            var param = new object[] { subscriptionid };
            var response = await JDownloaderApiHandler.CallAction<SubscriptionEventObject[]>(device, "/events/listen",
                param, JDownloaderHandler.LoginObject, true);
            return response ?? new SubscriptionEventObject[0];
        }

        public async Task<SubscriptionResponse> ChangeSubscriptionTimeouts(long subscriptionid, long polltimeout, long maxkeepalive)
        {
            var param = new object[] { subscriptionid, polltimeout, maxkeepalive };
            var response = await JDownloaderApiHandler.CallAction<SubscriptionResponse>(device, "/events/changesubscriptiontimeouts",
                param, JDownloaderHandler.LoginObject);

            return response;
        }

        public async Task<SubscriptionResponse> GetSubscriptionStatus(long subscriptionid)
        {
            var param = new object[] { subscriptionid };
            var response = await JDownloaderApiHandler.CallAction<SubscriptionResponse>(device, "/events/getsubscriptionstatus",
                param, JDownloaderHandler.LoginObject);

            return response;
        }

        public async Task<SubscriptionResponse> Unsubscribe(long subscriptionid)
        {
            var param = new object[] { subscriptionid };
            var response = await JDownloaderApiHandler.CallAction<SubscriptionResponse>(device, "/events/unsubscribe",
                param, JDownloaderHandler.LoginObject);

            return response;
        }

        public async Task<PublisherResponse[]> ListPublisher()
        {
            var response = await JDownloaderEventApiHandler.CallAction<PublisherResponse[]>(device, "/events/listpublisher",
                null, JDownloaderHandler.LoginObject);

            return response;
        }
    }
}
