using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Events;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace My.JDownloader.Api.Namespaces
{
    public class Events
    {
        private readonly DeviceObject _Device;
        
        public List<long> SubscriptionIDs;

        internal Events(DeviceObject device)
        {
            _Device = device;
            SubscriptionIDs = new List<long>();
        }

        public async Task<SubscriptionResponse> AddSubscription(long subscriptionid, string[] subscriptions, string[] exclusions)
        {
            var param = new object[] { subscriptionid, subscriptions, exclusions };
            var response = await JDownloaderApiHandler.CallAction<SubscriptionResponse>(_Device, "/events/addsubscription",
                param, JDownloaderHandler.LoginObject);
            return response;
        }

        public async Task<SubscriptionResponse> Subscribe(string[] subscriptions, string[] exclusions)
        {
            var param = new object[] { subscriptions, exclusions };
            var response = await JDownloaderApiHandler.CallAction<SubscriptionResponse>(_Device, "/events/subscribe",
                param, JDownloaderHandler.LoginObject);
            if (!SubscriptionIDs.Contains(response.SubscriptionId))
                SubscriptionIDs.Add(response.SubscriptionId);

            return response;
        }

        public async Task<SubscriptionEventObject[]> Listen(long subscriptionid)
        {
            var param = new object[] { subscriptionid };
            var response = await JDownloaderApiHandler.CallAction<SubscriptionEventObject[]>(_Device, "/events/listen",
                param, JDownloaderHandler.LoginObject, true);
            if (response == null)
                return new SubscriptionEventObject[0];

            return response;
        }

        public async Task<SubscriptionResponse> ChangeSubscriptionTimeouts(long subscriptionid, long polltimeout, long maxkeepalive)
        {
            var param = new object[] { subscriptionid, polltimeout, maxkeepalive };
            var response = await JDownloaderApiHandler.CallAction<SubscriptionResponse>(_Device, "/events/changesubscriptiontimeouts",
                param, JDownloaderHandler.LoginObject);

            return response;
        }

        public async Task<SubscriptionResponse> GetSubscriptionStatus(long subscriptionid)
        {
            var param = new object[] { subscriptionid };
            var response = await JDownloaderApiHandler.CallAction<SubscriptionResponse>(_Device, "/events/getsubscriptionstatus",
                param, JDownloaderHandler.LoginObject);

            return response;
        }

        public async Task<SubscriptionResponse> Unsubscribe(long subscriptionid)
        {
            var param = new object[] { subscriptionid };
            var response = await JDownloaderApiHandler.CallAction<SubscriptionResponse>(_Device, "/events/unsubscribe",
                param, JDownloaderHandler.LoginObject);

            return response;
        }
    }
}
