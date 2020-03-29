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
        private const string JdApiUrl = "http://api.jdownloader.org";

        private byte[] loginSecret;
        private byte[] deviceSecret;

        private readonly DeviceObject device;
        private LoginObject loginObject;

        private bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            private set
            {
                if (value)
                {
                    //Set device and loginObject when connection succesful
                    AccountsV2 = new AccountsV2(device, loginObject);
                    DownloadController = new DownloadController(device, loginObject);
                    Extensions = new Extensions(device, loginObject);
                    Extraction = new Extraction(device, loginObject);
                    LinkCrawler = new LinkCrawler(device, loginObject);
                    LinkgrabberV2 = new LinkGrabberV2(device, loginObject);
                    DownloadsV2 = new DownloadsV2(device, loginObject);
                    Update = new Update(device, loginObject);
                    Jd = new Jd(device, loginObject);
                    System = new Namespaces.System(device, loginObject);
                    Toolbar = new Toolbar(device, loginObject);
                    Events = new Events(device, loginObject);
                }

                isConnected = value;
            }
        }

        public AccountsV2 AccountsV2 { get; private set; }
        public DownloadController DownloadController { get; private set; }
        public Extensions Extensions { get; private set; }
        public Extraction Extraction { get; private set; }
        public LinkCrawler LinkCrawler { get; private set; }
        public LinkGrabberV2 LinkgrabberV2 { get; private set; }
        public DownloadsV2 DownloadsV2 { get; private set; }
        public Update Update { get; private set; }
        public Jd Jd { get; private set; }
        public Toolbar Toolbar { get; private set; }
        public Events Events { get; private set; }
        public Namespaces.System System { get; private set; }

        public event EventHandler<SubscriptionEventArgs> SubscriptionEvent;
        private Timer timer;
        private readonly SemaphoreSlim isBusy;


        internal DeviceHandler(DeviceObject device, LoginObject loginObject, bool useDirectConnect = false)
        {
            this.device = device;
            this.loginObject = loginObject;

            if (useDirectConnect)
                DirectConnect();
            else
                Connect(JdApiUrl).Wait();

            //Events
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
                Connect(JdApiUrl).Wait();
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

        private async Task<List<DirectConnectionInfo>> GetDirectConnectionInfos()
        {
            var tmp = await Utils.CallAction<DeviceConnectionInfo>(device, loginObject, "/device/getDirectConnectionInfos", null);
            if (string.IsNullOrEmpty(tmp.ToString()))
                return new List<DirectConnectionInfo>();

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
