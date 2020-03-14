using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Login;
using My.JDownloader.Api.Namespaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Extensions = My.JDownloader.Api.Namespaces.Extensions;

namespace My.JDownloader.Api
{
    public class DeviceHandler
    {
        private readonly DeviceObject device;

        private LoginObject loginObject;

        private byte[] loginSecret;
        private byte[] deviceSecret;

        public bool IsConnected { get; set; }

        public AccountsV2 AccountsV2;
        public DownloadController DownloadController;
        public Extensions Extensions;
        public Extraction Extraction;
        public LinkCrawler LinkCrawler;
        public LinkGrabberV2 LinkgrabberV2;
        public DownloadsV2 DownloadsV2;
        public Update Update;
        public JD Jd;
        public Toolbar Toolbar;
        public Events Events;
        public Namespaces.System System;

        public event EventHandler<SubscriptionEventArgs> SubscriptionEvent;
        private Timer timer;
        private readonly SemaphoreSlim isBusy;


        internal DeviceHandler(DeviceObject device, LoginObject loginObject, bool useDirectConnect = true)
        {
            this.device = device;
            this.loginObject = loginObject;

            AccountsV2 = new AccountsV2(this.device);
            DownloadController = new DownloadController(this.device);
            Extensions = new Extensions(this.device);
            Extraction = new Extraction(this.device);
            LinkCrawler = new LinkCrawler(this.device);
            LinkgrabberV2 = new LinkGrabberV2(this.device);
            DownloadsV2 = new DownloadsV2(this.device);
            Update = new Update(this.device);
            Jd = new JD(this.device);
            System = new Namespaces.System(this.device);
            Toolbar = new Toolbar(this.device);
            Events = new Events(this.device);
            if (useDirectConnect)
                DirectConnect();
            else
                Connect("http://api.jdownloader.org").Wait();
            isBusy = new SemaphoreSlim(1, 1);
            StartPolling();
        }

        ~DeviceHandler()
        {
            timer.Dispose();
            SubscriptionEvent = null;
        }

        /// <summary>
        /// Tries to directly connect to the JDownloader Client.
        /// </summary>
        private void DirectConnect()
        {
            var connected = false;
            foreach (var conInfos in GetDirectConnectionInfos().Result)
            {
                if (Connect(string.Concat("http://", conInfos.Ip, ":", conInfos.Port), true).Result)
                {
                    connected = true;
                    break;
                }
            }

            if (connected == false)
            {
                Connect("http://api.jdownloader.org").Wait();
            }
        }


        private async Task<bool> Connect(string apiUrl, bool fast = false)
        {
            //Calculating the Login and Device secret
            loginSecret = Utils.GetSecret(loginObject.Email, loginObject.Password, Utils.ServerDomain);
            deviceSecret = Utils.GetSecret(loginObject.Email, loginObject.Password, Utils.DeviceDomain);

            //Creating the query for the connection request
            var connectQueryUrl =
                $"/my/connect?email={HttpUtility.UrlEncode(loginObject.Email)}&appkey={HttpUtility.UrlEncode(Utils.AppKey)}";
            Utils.ApiUrl = apiUrl;
            //Calling the query
            var response = await JDownloaderApiHandler.CallServer<LoginObject>(connectQueryUrl, loginSecret, fast: fast);

            //If the response is null the connection was not successfull
            if (response == null)
                return false;

            response.Email = loginObject.Email;
            response.Password = loginObject.Password;

            //Else we are saving the response which contains the SessionToken, RegainToken and the RequestId
            loginObject = response;
            loginObject.ServerEncryptionToken = Utils.UpdateEncryptionToken(loginSecret, loginObject.SessionToken);
            loginObject.DeviceEncryptionToken = Utils.UpdateEncryptionToken(deviceSecret, loginObject.SessionToken);
            IsConnected = true;
            return true;
        }

        private async Task<List<DeviceConnectionInfoObject>> GetDirectConnectionInfos()
        {
            var tmp = await JDownloaderApiHandler.CallAction<DeviceConnectionInfoReturnObject>(device, "/device/getDirectConnectionInfos",
                null, loginObject);
            if (string.IsNullOrEmpty(tmp.ToString()))
                return new List<DeviceConnectionInfoObject>();

            return tmp.Infos;
        }

        private void StartPolling()
        {
            if (timer == null)
            {
                timer = new Timer(TimerTick, null, 0, 15000);
            }
        }

        private async void TimerTick(object state)
        {
            await isBusy.WaitAsync();
            try
            {
                if (Events.SubscriptionIDs.Count > 0)
                {
                    try
                    {
                        foreach (var t in await Task.WhenAll(Events.SubscriptionIDs.Select(x => Events.Listen(x)).ToArray()))
                            foreach (var e in t)
                            {
                                var args = new SubscriptionEventArgs
                                {
                                    EventId = e.EventId,
                                    EventData = e.EventData,
                                    EventPublisher = e.Publisher
                                };
                                SubscriptionEvent?.Invoke(this, args);
                            }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            finally
            {
                isBusy.Release();
            }
        }
    }

    public class SubscriptionEventArgs : EventArgs
    {
        public string EventId { get; set; }
        public string EventData { get; set; }
        public string EventPublisher { get; set; }
    }
}
