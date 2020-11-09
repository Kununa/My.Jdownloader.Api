using System.Collections.Generic;
using System.Linq;
using System.Web;
using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Login;
using System.Threading.Tasks;

[assembly: Fody.ConfigureAwait(false)]

namespace My.JDownloader.Api
{
    public class JDownloaderHandler
    {
        public bool IsConnected { get; set; }

        private static LoginObject loginObject;

        private byte[] loginSecret;
        private byte[] deviceSecret;

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
            loginSecret = Utils.GetSecret(email,  password, Utils.ServerDomain);
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
            loginObject = response;
            loginObject.Email = email;
            loginObject.Password = password;
            loginObject.ServerEncryptionToken = Utils.UpdateEncryptionToken(loginSecret,  loginObject.SessionToken);
            loginObject.DeviceEncryptionToken = Utils.UpdateEncryptionToken(deviceSecret, loginObject.SessionToken);
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
                $"/my/reconnect?appkey{HttpUtility.UrlEncode(Utils.AppKey)}&sessiontoken={HttpUtility.UrlEncode(loginObject.SessionToken)}&regaintoken={HttpUtility.UrlEncode(loginObject.RegainToken)}";
            var response = await JDownloaderApiHandler.CallServer<LoginObject>(query, loginObject.ServerEncryptionToken);
            if (response == null)
                return false;

            loginObject = response;
            loginObject.ServerEncryptionToken = Utils.UpdateEncryptionToken(loginSecret,  loginObject.SessionToken);
            loginObject.DeviceEncryptionToken = Utils.UpdateEncryptionToken(deviceSecret, loginObject.SessionToken);
            IsConnected = true;
            return IsConnected;
        }

        /// <summary>
        /// Disconnects the your client from the api
        /// </summary>
        /// <returns>True if successfull else false</returns>
        public async Task<bool> Disconnect()
        {
            var query = $"/my/disconnect?sessiontoken={HttpUtility.UrlEncode(loginObject.SessionToken)}";
            var response = await JDownloaderApiHandler.CallServer<object>(query, loginObject.ServerEncryptionToken);
            if (response == null)
                return false;

            IsConnected = false;
            loginObject = null;
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
            var query = $"/my/listdevices?sessiontoken={HttpUtility.UrlEncode(loginObject.SessionToken)}";
            var response = await JDownloaderApiHandler.CallServer<DeviceJsonReturnObject>(query, loginObject.ServerEncryptionToken);
            if (response == default)
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
        public async Task<DeviceHandler?> GetDevice(string name, bool useDirectConnect = false)
        {
            var devices = await GetDevices();
            if (devices.Count == 0)
                return null;
            return await GetDeviceHandler(devices.First(x => x.Name == name), useDirectConnect);
        }

        /// <summary>
        /// Creates an instance of the DeviceHandler class. 
        /// This is neccessary to call methods!
        /// </summary>
        /// <param name="device">The device you want to call the methods on.</param>
        /// <param name="useDirectConnect"></param>
        /// <returns>An deviceHandler instance.</returns>
        public async Task<DeviceHandler?> GetDeviceHandler(DeviceObject device, bool useDirectConnect = false)
        {
            return IsConnected ? await DeviceHandler.GetDeviceHandler(device, loginObject, useDirectConnect) : null;
        }
    }
}