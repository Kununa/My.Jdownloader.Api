using Newtonsoft.Json;
using Polly;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Fody;

namespace My.JDownloader.Api.ApiHandler
{
    [ConfigureAwait(false)]
    internal static class JDownloaderApiHandler
    {
        private static readonly HttpClient fastHttpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(3) };
        private static readonly AsyncPolicy asyncRetryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        private static readonly HttpClient httpClient = new HttpClient();

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
                using var httpResponse = fast ? await fastHttpClient.GetAsync(url) : await asyncRetryPolicy.ExecuteAsync(() => httpClient.GetAsync(url));
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                    response = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
            if (key != null)
            {
                response = await Utils.Decrypt(response, key);
            }
            if (string.IsNullOrEmpty(response))
                return default;
            dynamic jsonResponse = JsonConvert.DeserializeObject(response);
            if (rid != jsonResponse?.rid.ToString() ?? "")
                throw new Exceptions.InvalidRequestIdException("The 'RequestId' differs from the 'Requestid' from the query.");
            return JsonConvert.DeserializeObject<T>(response);
        }
    }
}