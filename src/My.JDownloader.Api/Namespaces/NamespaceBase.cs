using System;
using System.Threading.Tasks;
using System.Web;
using My.JDownloader.Api.ApiObjects.Action;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Login;
using Newtonsoft.Json;

namespace My.JDownloader.Api.Namespaces
{
    public class NamespaceBase
    {
        private readonly DeviceObject device;
        private readonly LoginObject loginObject;

        internal NamespaceBase(DeviceObject device, LoginObject loginObject)
        {
            this.device = device;
            this.loginObject = loginObject;
        }

        protected async Task<T> CallAction<T>( string action, object param, bool eventListener = false)
        {
            return await Utils.CallAction<T>(device, loginObject, action, param, eventListener);
        }

        protected async Task<T> CallEventAction<T>(string action, object param, bool eventListener = false)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device), "The device can't be null.");
            if (string.IsNullOrEmpty(device.Id))
                throw new ArgumentException("The id of the device is empty. Please call again the GetDevices Method and try again.", nameof(device));

            var query = $"/t_{HttpUtility.UrlEncode(loginObject.SessionToken)}_{HttpUtility.UrlEncode(device.Id)}{action}";
            var callActionObject = new CallActionObject
            {
                ApiVer = 1,
                Params = param,
                RequestId = Utils.GetUniqueRid(),
                Url = action
            };

            var url = Utils.ApiUrl + query;
            var json = JsonConvert.SerializeObject(callActionObject);
            var encryptedJson = Utils.Encrypt(json, loginObject.DeviceEncryptionToken);
            var encryptedResponse = await Utils.PostMethod(url, encryptedJson, eventListener);

            if (encryptedResponse == null)
                return default;

            var decryptedResponse = Utils.Decrypt(encryptedResponse, loginObject.DeviceEncryptionToken);
            if (decryptedResponse == null)
                return default;

            //special case as event responses are completly differerent
            if (decryptedResponse.Contains("subscriptionid"))
            {
                var direct = (T)JsonConvert.DeserializeObject(decryptedResponse, typeof(T));
                if (direct != null)
                    return direct;
            }

            var res = JsonConvert.DeserializeObject<T>(decryptedResponse);
            return res;
        }
    }
}
