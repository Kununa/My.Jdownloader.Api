using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;

namespace My.JDownloader.Api
{
    internal static class Utils
    {
        internal static string ServerDomain = "server";
        internal static string DeviceDomain = "device";
        internal static string AppKey = "my.jdownloader.api.wrapper";
        private static readonly HttpClient HttpClient = new HttpClient();
        static readonly AsyncPolicy AsyncRetryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        public static string ApiUrl = "http://api.jdownloader.org";

        #region "Secret and Encryption tokens"

        internal static byte[] GetSecret(string email, string password, string domain)
        {
            return EncodeStringToSha256(email.ToLower() + password + domain);
        }

        internal static readonly SHA256Managed _Sha256Managed = new SHA256Managed();

        internal static byte[] EncodeStringToSha256(string text)
        {
            return _Sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(text));
        }

        internal static byte[] UpdateEncryptionToken(byte[] oldToken, string UpdatedToken)
        {
            var newToken = GetByteArrayByHexString(UpdatedToken);
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
                using (var response = eventListener ? await HttpClient.PostAsync(url, content) : await AsyncRetryPolicy.ExecuteAsync(() => HttpClient.PostAsync(url, content)))
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
