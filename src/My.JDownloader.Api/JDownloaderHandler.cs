using System.Collections.Generic;
using System.Linq;
using System.Web;
using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Login;
using System.Threading.Tasks;

namespace My.JDownloader.Api
{
    public class JDownloaderHandler
    {
        public bool IsConnected { get; set; }

        internal static LoginObject LoginObject;

        private byte[] loginSecret;
        private byte[] deviceSecret;


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
            loginSecret = Utils.GetSecret(email, password, Utils.ServerDomain);
            deviceSecret = Utils.GetSecret(email, password, Utils.DeviceDomain);

            //Creating the query for the connection request
            var connectQueryUrl =
                $"/my/connect?email={HttpUtility.UrlEncode(email)}&appkey={HttpUtility.UrlEncode(Utils.AppKey)}";

            //Calling the query
            var response = await JDownloaderApiHandler.CallServer<LoginObject>(connectQueryUrl, loginSecret);

            //If the response is null the connection was not successfull
            if (response == null)
                return false;

            //Else we are saving the response which contains the SessionToken, RegainToken and the RequestId
            LoginObject = response;
            LoginObject.Email = email;
            LoginObject.Password = password;
            LoginObject.ServerEncryptionToken = Utils.UpdateEncryptionToken(loginSecret, LoginObject.SessionToken);
            LoginObject.DeviceEncryptionToken = Utils.UpdateEncryptionToken(deviceSecret, LoginObject.SessionToken);
            IsConnected = true;
            return true;
        }

        /// <summary>
        /// Tries to reconnect your client to the api.
        /// </summary>
        /// <returns>True if successfull else false</returns>
        public async Task<bool> Reconnect()
        {
            var query =
                $"/my/reconnect?appkey{HttpUtility.UrlEncode(Utils.AppKey)}&sessiontoken={HttpUtility.UrlEncode(LoginObject.SessionToken)}&regaintoken={HttpUtility.UrlEncode(LoginObject.RegainToken)}";
            var response = await JDownloaderApiHandler.CallServer<LoginObject>(query, LoginObject.ServerEncryptionToken);
            if (response == null)
                return false;

            LoginObject = response;
            LoginObject.ServerEncryptionToken = Utils.UpdateEncryptionToken(loginSecret, LoginObject.SessionToken);
            LoginObject.DeviceEncryptionToken = Utils.UpdateEncryptionToken(deviceSecret, LoginObject.SessionToken);
            IsConnected = true;
            return IsConnected;
        }

        /// <summary>
        /// Disconnects the your client from the api
        /// </summary>
        /// <returns>True if successfull else false</returns>
        public async Task<bool> Disconnect()
        {
            var query = $"/my/disconnect?sessiontoken={HttpUtility.UrlEncode(LoginObject.SessionToken)}";
            var response = await JDownloaderApiHandler.CallServer<object>(query, LoginObject.ServerEncryptionToken);
            if (response == null)
                return false;

            IsConnected = false;
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
            var devices = new List<DeviceObject>();
            var query = $"/my/listdevices?sessiontoken={HttpUtility.UrlEncode(LoginObject.SessionToken)}";
            var response = await JDownloaderApiHandler.CallServer<DeviceJsonReturnObject>(query, LoginObject.ServerEncryptionToken);
            if (response == null)
                return devices;

            devices = response.Devices;
            return devices;
        }

        /// <summary>
        /// Get device by name
        /// </summary>
        /// <param name="name">The name of device you want.</param>
        /// <param name="useDirectConnect">Direct connect to JD instance</param>
        /// <returns>Returns device. null when not found</returns>
        public async Task<DeviceHandler> GetDevice(string name, bool useDirectConnect = false)
        {
            var devices = await GetDevices();
            return devices != null ? GetDeviceHandler(devices.FirstOrDefault(x => x.Name == name), useDirectConnect) : null;
        }

        /// <summary>
        /// Creates an instance of the DeviceHandler class. 
        /// This is neccessary to call methods!
        /// </summary>
        /// <param name="device">The device you want to call the methods on.</param>
        /// <param name="useDirectConnect"></param>
        /// <returns>An deviceHandler instance.</returns>
        public DeviceHandler GetDeviceHandler(DeviceObject device, bool useDirectConnect = false)
        {
            return IsConnected && device != null ? new DeviceHandler(device, LoginObject, useDirectConnect) : null;
        }
    }
}