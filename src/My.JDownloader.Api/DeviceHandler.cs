using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Login;
using My.JDownloader.Api.Namespaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Extensions = My.JDownloader.Api.Namespaces.Extensions;

namespace My.JDownloader.Api
{
    public class DeviceHandler
    {
        private readonly DeviceObject _Device;

        private LoginObject _LoginObject;

        private byte[] _LoginSecret;
        private byte[] _DeviceSecret;

        public bool IsConnected;

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

        private bool RunEventListener;


        internal DeviceHandler(DeviceObject device,LoginObject LoginObject)
        {
            _Device = device;
            _LoginObject = LoginObject;

            AccountsV2 = new AccountsV2(_Device);
            DownloadController = new DownloadController(_Device);
            Extensions = new Extensions(_Device);
            Extraction = new Extraction(_Device);
            LinkCrawler = new LinkCrawler(_Device);
            LinkgrabberV2 = new LinkGrabberV2(_Device);
            DownloadsV2 = new DownloadsV2( _Device);
            Update = new Update( _Device);
            Jd = new JD( _Device);
            System = new Namespaces.System( _Device);
            Toolbar = new Toolbar( _Device);
            Events = new Events( _Device);
            DirectConnect();
            RunEventListener = true;
            new Task(() => EventListener()).Start();
        }

        ~DeviceHandler()
        {
            SubscriptionEvent = null;
            RunEventListener = false;
        }

        /// <summary>
        /// Tries to directly connect to the JDownloader Client.
        /// </summary>
        private void DirectConnect()
        {
            bool connected = false;
            foreach (var conInfos in GetDirectConnectionInfos().Result)
            {
                if (Connect(string.Concat("http://", conInfos.Ip, ":", conInfos.Port)).Result)
                {
                    connected = true;
                    break;
                }
            }

            if (connected == false)
            {
                connected = Connect("http://api.jdownloader.org").Result;
            }
        }


        private async Task<bool> Connect(string apiUrl)
        {
            //Calculating the Login and Device secret
            _LoginSecret = Utils.GetSecret(_LoginObject.Email, _LoginObject.Password, Utils.ServerDomain);
            _DeviceSecret = Utils.GetSecret(_LoginObject.Email, _LoginObject.Password, Utils.DeviceDomain);

            //Creating the query for the connection request
            string connectQueryUrl =
                $"/my/connect?email={HttpUtility.UrlEncode(_LoginObject.Email)}&appkey={HttpUtility.UrlEncode(Utils.AppKey)}";
            JDownloaderApiHandler._ApiUrl = apiUrl;
            //Calling the query
            var response = await JDownloaderApiHandler.CallServer<LoginObject>(connectQueryUrl, _LoginSecret);

            //If the response is null the connection was not successfull
            if (response == null)
                return false;

            response.Email = _LoginObject.Email;
            response.Password = _LoginObject.Password;

            //Else we are saving the response which contains the SessionToken, RegainToken and the RequestId
            _LoginObject = response;
            _LoginObject.ServerEncryptionToken = Utils.UpdateEncryptionToken(_LoginSecret, _LoginObject.SessionToken);
            _LoginObject.DeviceEncryptionToken = Utils.UpdateEncryptionToken(_DeviceSecret, _LoginObject.SessionToken);
            IsConnected = true;
            return true;
        }

        private async Task<List<DeviceConnectionInfoObject>> GetDirectConnectionInfos()
        {
            var tmp = await JDownloaderApiHandler.CallAction<DeviceConnectionInfoReturnObject>(_Device, "/device/getDirectConnectionInfos",
                null, _LoginObject);
            if (string.IsNullOrEmpty(tmp.ToString()))
                return new List<DeviceConnectionInfoObject>();

            return tmp.Infos;
        }

        private async void EventListener()
        {
            while (RunEventListener)
            {
                if (Events.SubscriptionIDs.Count > 0)
                {
                    foreach (var t in await Task.WhenAll(Events.SubscriptionIDs.Select(x => Events.Listen(x)).ToArray()))
                        foreach (var e in t)
                        {
                            SubscriptionEventArgs args = new SubscriptionEventArgs();
                            args.EventId = e.EventId;
                            SubscriptionEvent?.Invoke(this, args);
                        }
                }
            }
        }
    }

    public class SubscriptionEventArgs : EventArgs
    {
        public string EventId { get; set; }
    }
}
