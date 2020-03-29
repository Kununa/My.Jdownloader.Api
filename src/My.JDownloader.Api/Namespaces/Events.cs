using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Events;
using System.Threading.Tasks;
using System.Collections.Generic;
using My.JDownloader.Api.ApiObjects.Login;

namespace My.JDownloader.Api.Namespaces
{
    public class Events : NamespaceBase
    {

        public List<long> SubscriptionIDs;

        public Events(DeviceObject device, LoginObject loginObject) : base(device, loginObject)
        {
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
            var response = await CallAction<SubscriptionResponse>("/events/addsubscription", param);
            return response;
        }

        public async Task<SubscriptionResponse> Subscribe(string[] subscriptions = null, string[] exclusions = null)
        {
            if (exclusions == null)
                exclusions = new string[0];
            var param = new object[] { new[] { "STOPPED" }, exclusions };
            var response = await CallAction<SubscriptionResponse>("/events/subscribe", param);
            if (!SubscriptionIDs.Contains(response.SubscriptionId))
                SubscriptionIDs.Add(response.SubscriptionId);

            return response;
        }

        public async Task<IReadOnlyList<SubscriptionEventObject>> Listen(long subscriptionid)
        {
            var param = new object[] { subscriptionid };
            var response = await CallAction<IReadOnlyList<SubscriptionEventObject>>("/events/listen", param, true);
            return response ?? new SubscriptionEventObject[0];
        }

        public async Task<SubscriptionResponse> ChangeSubscriptionTimeouts(long subscriptionid, long polltimeout, long maxkeepalive)
        {
            var param = new object[] { subscriptionid, polltimeout, maxkeepalive };
            var response = await CallAction<SubscriptionResponse>("/events/changesubscriptiontimeouts", param);

            return response;
        }

        public async Task<SubscriptionResponse> GetSubscriptionStatus(long subscriptionid)
        {
            var param = new object[] { subscriptionid };
            var response = await CallAction<SubscriptionResponse>("/events/getsubscriptionstatus", param);

            return response;
        }

        public async Task<SubscriptionResponse> Unsubscribe(long subscriptionid)
        {
            var param = new object[] { subscriptionid };
            var response = await CallAction<SubscriptionResponse>("/events/unsubscribe", param);

            return response;
        }

        public async Task<IReadOnlyList<PublisherResponse>> ListPublisher()
        {
            var response = await CallEventAction<IReadOnlyList<PublisherResponse>>("/events/listpublisher", null);

            return response;
        }
    }
}
