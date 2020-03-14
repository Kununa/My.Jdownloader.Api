using My.JDownloader.Api.ApiObjects.Action;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Login;
using Newtonsoft.Json;
using Polly;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

[assembly: Fody.ConfigureAwait(false)]

namespace My.JDownloader.Api.ApiHandler
{
    internal class JDownloaderApiHandler
    {
        private static readonly HttpClient FastHttpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(3) };
        static readonly AsyncPolicy AsyncRetryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        private static readonly HttpClient HttpClient = new HttpClient();

        public void SetApiUrl(string newApiUrl)
        {
            Utils.ApiUrl = newApiUrl;
        }

        public static async Task<T> CallServer<T>(string query, byte[] key, bool fast = false)
        {
            var rid = Utils.GetUniqueRid().ToString();
            query += "&rid=" + rid;
            var signature = Utils.GetSignature(query, key);
            query += "&signature=" + signature;

            var url = Utils.ApiUrl + query;
            var response = "";
            try
            {
                using (var httpResponse = fast ? await FastHttpClient.GetAsync(url) : await AsyncRetryPolicy.ExecuteAsync(() => HttpClient.GetAsync(url)))
                {
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                        response = await httpResponse.Content.ReadAsStringAsync();
                }
            }
            catch
            {
                return default;
            }
            if (key != null)
            {
                response = Utils.Decrypt(response, key);
            }
            if (string.IsNullOrEmpty(response))
                return default;
            dynamic jsonResponse = JsonConvert.DeserializeObject(response);
            if (rid != jsonResponse?.rid.ToString() ?? "")
                throw new Exceptions.InvalidRequestIdException("The 'RequestId' differs from the 'Requestid' from the query.");
            return (T)JsonConvert.DeserializeObject(response, typeof(T));
        }

        public static async Task<T> CallAction<T>(DeviceObject device, string action, object param, LoginObject loginObject, bool eventListener = false)
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
            var res = (ApiObjects.DefaultReturnObject)JsonConvert.DeserializeObject(decryptedResponse, typeof(ApiObjects.DefaultReturnObject));
            if (res == null || res.Data == null)
                return default;
            if (res.Data.GetType() == typeof(Newtonsoft.Json.Linq.JObject))
                return ((Newtonsoft.Json.Linq.JObject)res.Data).ToObject<T>();
            if (res.Data.GetType() == typeof(Newtonsoft.Json.Linq.JArray))
                return ((Newtonsoft.Json.Linq.JArray)res.Data).ToObject<T>();
            return (T)res.Data;
        }

    }
}