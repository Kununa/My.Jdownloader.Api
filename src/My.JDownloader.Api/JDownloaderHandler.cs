using System.Collections.Generic;
using System.Web;
using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Login;
using System.Threading.Tasks;

namespace My.JDownloader.Api
{
    public class JDownloaderHandler
    {
        public bool IsConnected { get; set; } = false;

        internal static LoginObject LoginObject;

        private byte[] _LoginSecret;
        private byte[] _DeviceSecret;

        
        //private readonly JDownloaderApiHandler JDownloaderApiHandler = new JDownloaderApiHandler();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appkey">The name of the app. Should be unique!</param>
        public JDownloaderHandler(string appkey)
        {
            Utils.AppKey = appkey;
        }

        /// <summary>
        /// </summary>
        /// <param name="email">Your email of your my.jdownloader.org account.</param>
        /// <param name="password">Your password of your my.jdownloader.org account.</param>
        /// <param name="appKey">The name of the app. Should be unique!</param>
        public JDownloaderHandler(string email, string password, string appKey)
        {
            Connect(email, password).Wait();
            Utils.AppKey = appKey;
        }

        #region "Connection methods"

        /// <summary>
        /// Fires a connection request to the api.
        /// </summary>
        /// <param name="email">Email of the User</param>
        /// <param name="password">Password of the User</param>
        /// <returns>Return if the Connection was succesfull</returns>
        public async Task<bool> Connect(string email, string password)
        {
            //Calculating the Login and Device secret
            _LoginSecret = Utils.GetSecret(email, password, Utils.ServerDomain);
            _DeviceSecret = Utils.GetSecret(email, password, Utils.DeviceDomain);

            //Creating the query for the connection request
            string connectQueryUrl =
                $"/my/connect?email={HttpUtility.UrlEncode(email)}&appkey={HttpUtility.UrlEncode(Utils.AppKey)}";

            //Calling the query
            var response = await JDownloaderApiHandler.CallServer<LoginObject>(connectQueryUrl, _LoginSecret);

            //If the response is null the connection was not successfull
            if (response == null)
                return false;

            //Else we are saving the response which contains the SessionToken, RegainToken and the RequestId
            LoginObject = response;
            LoginObject.Email = email;
            LoginObject.Password = password;
            LoginObject.ServerEncryptionToken = Utils.UpdateEncryptionToken(_LoginSecret, LoginObject.SessionToken);
            LoginObject.DeviceEncryptionToken = Utils.UpdateEncryptionToken(_DeviceSecret, LoginObject.SessionToken);
            IsConnected = true;
            return true;
        }

        /// <summary>
        /// Tries to reconnect your client to the api.
        /// </summary>
        /// <returns>True if successfull else false</returns>
        public async Task<bool> Reconnect()
        {
            string query =
                $"/my/reconnect?appkey{HttpUtility.UrlEncode(Utils.AppKey)}&sessiontoken={HttpUtility.UrlEncode(LoginObject.SessionToken)}&regaintoken={HttpUtility.UrlEncode(LoginObject.RegainToken)}";
            var response = await JDownloaderApiHandler.CallServer<LoginObject>(query, LoginObject.ServerEncryptionToken);
            if (response == null)
                return false;

            LoginObject = response;
            LoginObject.ServerEncryptionToken = Utils.UpdateEncryptionToken(_LoginSecret, LoginObject.SessionToken);
            LoginObject.DeviceEncryptionToken = Utils.UpdateEncryptionToken(_DeviceSecret, LoginObject.SessionToken);
            IsConnected = true;
            return IsConnected;
        }

        /// <summary>
        /// Disconnects the your client from the api
        /// </summary>
        /// <returns>True if successfull else false</returns>
        public bool Disconnect()
        {
            string query = $"/my/disconnect?sessiontoken={HttpUtility.UrlEncode(LoginObject.SessionToken)}";
            var response = JDownloaderApiHandler.CallServer<object>(query, LoginObject.ServerEncryptionToken);
            if (response == null)
                return false;

            LoginObject = null;
            return true;
        }
        #endregion
        

        /// <summary>
        /// Lists all Devices which are currently connected to your my.jdownloader.org account.
        /// </summary>
        /// <returns>Returns a list of your currently connected devices.</returns>
        public async Task<List<DeviceObject>> GetDevices()
        {
            List<DeviceObject> devices = new List<DeviceObject>();
            string query = $"/my/listdevices?sessiontoken={HttpUtility.UrlEncode(LoginObject.SessionToken)}";
            var response = await JDownloaderApiHandler.CallServer<DeviceJsonReturnObject>(query, LoginObject.ServerEncryptionToken);
            if (response == null)
                return devices;

            foreach (DeviceObject device in response.Devices)
            {
                devices.Add(device);
            }

            return devices;
        }

        /// <summary>
        /// Creates an instance of the DeviceHandler class. 
        /// This is neccessary to call methods!
        /// </summary>
        /// <param name="device">The device you want to call the methods on.</param>
        /// <returns>An deviceHandler instance.</returns>
        public DeviceHandler GetDeviceHandler(DeviceObject device)
        {
            if (IsConnected)
            {
                //TODO: Make it possible to directly connect to the jdownloader client. If it's not working use the relay server.
                //var tmp = JDownloaderApiHandler.CallAction<DefaultReturnObject>(device, "/device/getDirectConnectionInfos",
                //    null, LoginObject, true);
                return new DeviceHandler(device, LoginObject);
            }
            return null;
        }
    }
}