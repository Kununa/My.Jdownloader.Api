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
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace My.JDownloader.Api
{
    public class DeviceHandler
    {
        private const string JdApiUrl = "http://api.jdownloader.org";

        private byte[]? loginSecret;
        private byte[]? deviceSecret;

        private readonly DeviceObject device;
        private LoginObject loginObject;

        private bool isConnected;

        public bool IsConnected
        {
            get => isConnected;
            private set => isConnected = value;
        }

        public AccountsV2 AccountsV2 { get; }
        public DownloadController DownloadController { get; }
        public Extensions Extensions { get; }
        public Extraction Extraction { get; }
        public LinkCrawler LinkCrawler { get; }
        public LinkGrabberV2 LinkgrabberV2 { get; }
        public DownloadsV2 DownloadsV2 { get; }
        public Update Update { get; }
        public Jd Jd { get; }
        public Toolbar Toolbar { get; }
        public Events Events { get; }
        public Namespaces.System System { get; }

        public event EventHandler<SubscriptionEventArgs>? SubscriptionEvent;
        private readonly Timer timer;
        private readonly SemaphoreSlim isBusy;

        public static async Task<DeviceHandler> GetDeviceHandler(DeviceObject device, LoginObject loginObject, bool useDirectConnect = false)
        {
            var dh = new DeviceHandler(device, loginObject);
            if (useDirectConnect)
                await dh.DirectConnect();
            else
                await dh.Connect(JdApiUrl);
            return dh;
        }


        private DeviceHandler(DeviceObject device, LoginObject loginObject)
        {
            this.device = device;
            this.loginObject = loginObject;
            isBusy = new SemaphoreSlim(1, 1);
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
            timer = new Timer(TimerTick, null, 0, 15000);
        }

        ~DeviceHandler()
        {
            timer.Dispose();
            SubscriptionEvent = null;
        }

        /// <summary>
        /// Tries to directly connect to the JDownloader Client.
        /// </summary>
        private async Task DirectConnect()
        {
            var connected = GetDirectConnectionInfos().Result.Any(conInfos => Connect(string.Concat("http://", conInfos.Ip, ":", conInfos.Port), true).Result);

            if (connected == false)
            {
                await Connect(JdApiUrl);
            }
        }


        private async Task<bool> Connect(string apiUrl, bool fast = false)
        {
            //Calculating the Login and Device secret
            loginSecret = Utils.GetSecret(loginObject.Email,  loginObject.Password, Utils.ServerDomain);
            deviceSecret = Utils.GetSecret(loginObject.Email, loginObject.Password, Utils.DeviceDomain);

            //Creating the query for the connection request
            var connectQueryUrl =
                $"/my/connect?email={HttpUtility.UrlEncode(loginObject.Email)}&appkey={HttpUtility.UrlEncode(Utils.AppKey)}";
            Utils.ApiUrl = apiUrl;
            //Calling the query
            var response = await JDownloaderApiHandler.CallServer<LoginObject>(connectQueryUrl, loginSecret, fast);

            response.Email = loginObject.Email;
            response.Password = loginObject.Password;

            //Else we are saving the response which contains the SessionToken, RegainToken and the RequestId
            loginObject = response;
            loginObject.ServerEncryptionToken = Utils.UpdateEncryptionToken(loginSecret,  loginObject.SessionToken);
            loginObject.DeviceEncryptionToken = Utils.UpdateEncryptionToken(deviceSecret, loginObject.SessionToken);
            IsConnected = true;
            return true;
        }

        private async Task<List<DirectConnectionInfo>> GetDirectConnectionInfos()
        {
            var tmp = await Utils.CallAction<DeviceConnectionInfo>(device, loginObject, "/device/getDirectConnectionInfos", null);
            return string.IsNullOrEmpty(tmp?.ToString()) ? new List<DirectConnectionInfo>() : tmp.Infos;
        }

        private async void TimerTick(object? state)
        {
            if (!isConnected)
                return;
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
        public string? EventId { get; set; }
        public string? EventData { get; set; }
        public string? EventPublisher { get; set; }
    }
}