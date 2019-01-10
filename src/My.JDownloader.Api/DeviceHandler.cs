﻿using My.JDownloader.Api.ApiHandler;
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
        private Timer timer;
        private bool isBusy;


        internal DeviceHandler(DeviceObject device, LoginObject LoginObject)
        {
            _Device = device;
            _LoginObject = LoginObject;

            AccountsV2 = new AccountsV2(_Device);
            DownloadController = new DownloadController(_Device);
            Extensions = new Extensions(_Device);
            Extraction = new Extraction(_Device);
            LinkCrawler = new LinkCrawler(_Device);
            LinkgrabberV2 = new LinkGrabberV2(_Device);
            DownloadsV2 = new DownloadsV2(_Device);
            Update = new Update(_Device);
            Jd = new JD(_Device);
            System = new Namespaces.System(_Device);
            Toolbar = new Toolbar(_Device);
            Events = new Events(_Device);
            DirectConnect();
            isBusy = false;
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
            bool connected = false;
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
                connected = Connect("http://api.jdownloader.org").Result;
            }
        }


        private async Task<bool> Connect(string apiUrl, bool fast = false)
        {
            //Calculating the Login and Device secret
            _LoginSecret = Utils.GetSecret(_LoginObject.Email, _LoginObject.Password, Utils.ServerDomain);
            _DeviceSecret = Utils.GetSecret(_LoginObject.Email, _LoginObject.Password, Utils.DeviceDomain);

            //Creating the query for the connection request
            string connectQueryUrl =
                $"/my/connect?email={HttpUtility.UrlEncode(_LoginObject.Email)}&appkey={HttpUtility.UrlEncode(Utils.AppKey)}";
            JDownloaderApiHandler._ApiUrl = apiUrl;
            //Calling the query
            var response = await JDownloaderApiHandler.CallServer<LoginObject>(connectQueryUrl, _LoginSecret, fast: fast);

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

        private void StartPolling()
        {
            if (timer == null)
            {
                timer = new Timer(TimerTick, null, 0, 15000);
            }
        }

        private async void TimerTick(object state)
        {
            if (isBusy == true)
            {
                return;
            }

            try
            {
                isBusy = true;
                if (Events.SubscriptionIDs.Count > 0)
                {
                    try
                    {
                        foreach (var t in await Task.WhenAll(Events.SubscriptionIDs.Select(x => Events.Listen(x))?.ToArray()))
                            foreach (var e in t)
                            {
                                SubscriptionEventArgs args = new SubscriptionEventArgs
                                {
                                    EventId = e.EventId,
                                    EventData = e.EventData,
                                    EventPublisher = e.Publisher
                                };
                                SubscriptionEvent?.Invoke(this, args);
                            }
                    }
                    catch { }
                }
            }
            finally
            {
                isBusy = false;
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
