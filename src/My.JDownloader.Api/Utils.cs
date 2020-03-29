using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using My.JDownloader.Api.ApiObjects.Action;
using My.JDownloader.Api.ApiObjects.Devices;
using My.JDownloader.Api.ApiObjects.Login;
using Newtonsoft.Json;
using Polly;

namespace My.JDownloader.Api
{
    internal static class Utils
    {
        internal static string ServerDomain = "server";
        internal static string DeviceDomain = "device";
        internal static string AppKey = "my.jdownloader.api.wrapper";
        private static readonly HttpClient httpClient = new HttpClient();
        static readonly AsyncPolicy asyncRetryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        public static string ApiUrl = "http://api.jdownloader.org";

        public static async Task<T> CallAction<T>(DeviceObject device, LoginObject loginObject,string action, object param, bool eventListener = false)
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
                var direct = JsonConvert.DeserializeObject<T>(decryptedResponse);
                if (direct != null)
                    return direct;
            }
            var res = JsonConvert.DeserializeObject<ApiObjects.DefaultReturnObject>(decryptedResponse);
            if (res == null || res.Data == null)
                return default;
            if (res.Data.GetType() == typeof(Newtonsoft.Json.Linq.JObject))
                return ((Newtonsoft.Json.Linq.JObject)res.Data).ToObject<T>();
            if (res.Data.GetType() == typeof(Newtonsoft.Json.Linq.JArray))
                return ((Newtonsoft.Json.Linq.JArray)res.Data).ToObject<T>();
            return (T)res.Data;
        }

        #region "Secret and Encryption tokens"

        internal static byte[] GetSecret(string email, string password, string domain)
        {
            return EncodeStringToSha256(email.ToLower() + password + domain);
        }

        internal static readonly SHA256Managed SHA256_MANAGED = new SHA256Managed();

        internal static byte[] EncodeStringToSha256(string text)
        {
            return SHA256_MANAGED.ComputeHash(Encoding.UTF8.GetBytes(text));
        }

        internal static byte[] UpdateEncryptionToken(byte[] oldToken, string updatedToken)
        {
            var newToken = GetByteArrayByHexString(updatedToken);
            var newHash = new byte[oldToken.Length + newToken.Length];
            oldToken.CopyTo(newHash, 0);
            newToken.CopyTo(newHash, 32);
            var hashString = new SHA256Managed();
            hashString.ComputeHash(newHash);
            return hashString.Hash;
        }

        internal static byte[] GetByteArrayByHexString(string hexString)
        {
            hexString = hexString.Replace("-", "");
            var ret = new byte[hexString.Length / 2];
            for (var i = 0; i < ret.Length; i++)
            {
                ret[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return ret;
        }

        #endregion

        public static async Task<string> PostMethod(string url, string body, bool eventListener = false)
        {
            try
            {
                if (String.IsNullOrEmpty(body))
                    return null;

                var content = new StringContent(body, Encoding.UTF8, "application/aesjson");
                using (var response = eventListener ? await httpClient.PostAsync(url, content) : await asyncRetryPolicy.ExecuteAsync(() => httpClient.PostAsync(url, content)))
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

        public static long GetUniqueRid()
        {
            var d = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            return (long)d;
        }

        #region "Encrypt, Decrypt and Signature"

        public static string GetSignature(string data, byte[] key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "The ivKey is null. Please check your login informations. If it's still null the server may has disconnected you.");
            }
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var hmacsha256 = new HMACSHA256(key);
            hmacsha256.ComputeHash(dataBytes);
            var hash = hmacsha256.Hash;
            var binaryString = hash.Aggregate("", (current, t) => current + t.ToString("X2"));
            return binaryString.ToLower();
        }

        public static string Encrypt(string data, byte[] ivKey)
        {
            if (ivKey == null)
            {
                throw new ArgumentNullException(nameof(ivKey), "The ivKey is null. Please check your login informations. If it's still null the server may has disconnected you.");
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
            var encryptor = rj.CreateEncryptor();
            var msEncrypt = new MemoryStream();
            var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(data);
            }
            var encrypted = msEncrypt.ToArray();
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string data, byte[] ivKey)
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

            var cypher = Convert.FromBase64String(data);
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
    }
}
