using My.JDownloader.Api.ApiObjects.Action;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Login;
using Newtonsoft.Json;
using Polly;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

[assembly: Fody.ConfigureAwait(false)]

namespace My.JDownloader.Api.ApiHandler
{
    internal class JDownloaderApiHandler
    {
        public static string _ApiUrl = "http://api.jdownloader.org";
        private static HttpClient httpClient = new HttpClient();
        static Policy policy = Policy.Handle<Exception>()
                        .WaitAndRetryAsync(4, retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        private static HttpClient fastHttpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(3) };

        public void SetApiUrl(string newApiUrl)
        {
            _ApiUrl = newApiUrl;
        }

        public static async Task<T> CallServer<T>(string query, byte[] key, bool fast = false)
        {
            string rid = GetUniqueRid().ToString();
            query += "&rid=" + rid;
            string signature = GetSignature(query, key);
            query += "&signature=" + signature;

            string url = _ApiUrl + query;
            string response = "";
            try
            {
                using (HttpResponseMessage httpResponse = fast ? await fastHttpClient.GetAsync(url) : await policy.ExecuteAsync(() => httpClient.GetAsync(url)))
                {
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                        response = await httpResponse.Content.ReadAsStringAsync();
                }
            }
            catch
            {
                return default(T);
            }
            if (key != null)
            {
                response = Decrypt(response, key);
            }
            if (string.IsNullOrEmpty(response))
                return default(T);
            dynamic jsonResponse = JsonConvert.DeserializeObject(response);
            if (rid != jsonResponse?.rid.ToString() ?? "")
                throw new Exceptions.InvalidRequestIdException("The 'RequestId' differs from the 'Requestid' from the query.");
            return (T)JsonConvert.DeserializeObject(response, typeof(T));
        }

        public static async Task<T> CallAction<T>(DeviceObject device, string action, object param, LoginObject loginObject,
            bool eventListener = false)
        {
            if (device == null)
                throw new ArgumentNullException("The device can't be null.");
            if (string.IsNullOrEmpty(device.Id))
                throw new ArgumentException(
                    "The id of the device is empty. Please call again the GetDevices Method and try again.");

            string query =
                $"/t_{HttpUtility.UrlEncode(loginObject.SessionToken)}_{HttpUtility.UrlEncode(device.Id)}{action}";
            CallActionObject callActionObject = new CallActionObject
            {
                ApiVer = 1,
                Params = param,
                RequestId = GetUniqueRid(),
                Url = action
            };

            string url = _ApiUrl + query;
            string json = JsonConvert.SerializeObject(callActionObject);
            json = Encrypt(json, loginObject.DeviceEncryptionToken);
            string response = await PostMethod(url, json, loginObject.DeviceEncryptionToken, eventListener);

            if (response == null)
                return default(T);

            string tmp = Decrypt(response, loginObject.DeviceEncryptionToken);
            if (tmp == null)
                return default(T);

            //special case as event responses are completly differerent
            if (tmp.Contains("subscriptionid"))
            {
                var direct = (T)JsonConvert.DeserializeObject(tmp, typeof(T));
                if (direct != null)
                    return direct;
            }
            var res = (ApiObjects.DefaultReturnObject)JsonConvert.DeserializeObject(tmp, typeof(ApiObjects.DefaultReturnObject));
            if (res == null || res.Data == null)
                return default(T);
            if (res.Data.GetType() == typeof(Newtonsoft.Json.Linq.JObject))
                return ((Newtonsoft.Json.Linq.JObject)res.Data).ToObject<T>();
            else if (res.Data.GetType() == typeof(Newtonsoft.Json.Linq.JArray))
                return ((Newtonsoft.Json.Linq.JArray)res.Data).ToObject<T>();
            else
                return (T)res.Data;
        }

        private static async Task<string> PostMethod(string url, string body, byte[] ivKey = null, bool eventListener = false)
        {
            try
            {
                if (string.IsNullOrEmpty(body))
                    return null;

                StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = eventListener ? await httpClient.PostAsync(url, content) : await policy.ExecuteAsync(() => httpClient.PostAsync(url, content)))
                {
                    if (response != null)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }

                return null;
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine(e);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        #region "Encrypt, Decrypt and Signature"

        private static string GetSignature(string data, byte[] key)
        {
            if (key == null)
            {
                throw new Exception(
                    "The ivKey is null. Please check your login informations. If it's still null the server may has disconnected you.");
            }
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var hmacsha256 = new HMACSHA256(key);
            hmacsha256.ComputeHash(dataBytes);
            var hash = hmacsha256.Hash;
            string binaryString = hash.Aggregate("", (current, t) => current + t.ToString("X2"));
            return binaryString.ToLower();
        }

        private static string Encrypt(string data, byte[] ivKey)
        {
            if (ivKey == null)
            {
                throw new Exception(
                    "The ivKey is null. Please check your login informations. If it's still null the server may has disconnected you.");
            }
            var iv = new byte[16];
            var key = new byte[16];
            Array.Copy(ivKey, iv, 16);
            Array.Copy(ivKey, 16, key, 0, 16);

            var rj = new RijndaelManaged
            {
                Key = key,
                IV = iv,
                Mode = CipherMode.CBC,
                BlockSize = 128
            };
            ICryptoTransform encryptor = rj.CreateEncryptor();
            var msEncrypt = new MemoryStream();
            var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(data);
            }
            byte[] encrypted = msEncrypt.ToArray();
            return Convert.ToBase64String(encrypted);
        }

        private static string Decrypt(string data, byte[] ivKey)
        {
            if (data == null)
            {
                Console.WriteLine("The data is null. This might happen due to overloading the server.");
                return null;
            }
            if (ivKey == null)
            {
                throw new Exception(
                    "The ivKey is null. Please check your login informations. If it's still null the server may has disconnected you.");
            }
            var iv = new byte[16];
            var key = new byte[16];
            Array.Copy(ivKey, iv, 16);
            Array.Copy(ivKey, 16, key, 0, 16);

            byte[] cypher = Convert.FromBase64String(data);
            var rj = new RijndaelManaged
            {
                BlockSize = 128,
                Mode = CipherMode.CBC,
                IV = iv,
                Key = key
            };
            var ms = new MemoryStream(cypher);
            string result;
            using (var cs = new CryptoStream(ms, rj.CreateDecryptor(), CryptoStreamMode.Read))
            {
                using (var sr = new StreamReader(cs))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }

        #endregion

        private static long GetUniqueRid()
        {
            double d = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            return (long)d;
        }
    }
}