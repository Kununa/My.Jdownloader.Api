using System.Collections.Generic;
using My.JDownloader.Api.ApiObjects;
using My.JDownloader.Api.ApiObjects.AccountV2;
using My.JDownloader.Api.ApiObjects.Devices;
using Newtonsoft.Json;
using System.Threading.Tasks;
using My.JDownloader.Api.ApiObjects.Login;

namespace My.JDownloader.Api.Namespaces
{
    public class AccountsV2 : NamespaceBase
    {
        public AccountsV2(DeviceObject device, LoginObject loginObject) : base(device, loginObject, "accountsV2")
        {
        }

        /// <summary>
        /// Adds an premium account to your JDownloader device.
        /// </summary>
        /// <param name="hoster">The hoster e.g. mega.co.nz</param>
        /// <param name="email">Your email</param>
        /// <param name="password">Your password</param>
        /// <returns>True if the account was successfully added.</returns>
        public async Task<bool> AddAccount(string hoster, string email, string password)
        {
            var param = new[] {hoster, email, password};
            var response = await CallAction<DefaultReturnObject>("addAccount", param);
            return true;
        }

        /// <summary>
        /// Adds an basic authorization to the client.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="hostmask"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>True if successfull.</returns>
        public async Task<bool> AddBasicAuth(HostType type, string hostmask, string username, string password)
        {
            var param = new object[] {type, hostmask, username, password};
            var response = await CallAction<DefaultReturnObject>("addBasicAuth", param);
            return response != null;
        }

        /// <summary>
        /// Disables an account to download.
        /// </summary>
        /// <param name="accountIds">The account ids you want to disable.</param>
        /// <returns>True if succesfull</returns>
        public async Task<bool> DisableAccounts(long[] accountIds)
        {
            var param = new[] {accountIds};
            var response = await CallAction<DefaultReturnObject>("disableAccounts", param);
            return response != null;
        }

        /// <summary>
        /// Enables an account to download.
        /// </summary>
        /// <param name="accountIds">The account ids you want to enable.</param>
        /// <returns>True if succesfull</returns>
        public async Task<bool> EnableAccounts(long[] accountIds)
        {
            var param = new[] {accountIds};
            var response = await CallAction<DefaultReturnObject>("enableAccounts", param);
            return response != null;
        }

        /// <summary>
        /// Gets a link of a hoster by the name of it.
        /// </summary>
        /// <param name="hoster">Name of the hoster you want the url from.</param>
        /// <returns>The url of the hoster.</returns>
        public async Task<string?> GetPremiumHosterUrl(string hoster)
        {
            var param = new[] {hoster};
            return (await CallAction<object>("getPremiumHosterUrl", param)).ToString();
        }

        /// <summary>
        /// Lists all accounts which are stored on the device.
        /// </summary>
        /// <param name="requestObject">Contains properties like Username (boolean) etc. If set to true the api will return the Username.</param>
        /// <returns>A list of all accounts stored on the device.</returns>
        public async Task<IReadOnlyList<ListAccountResponseObject>> ListAccounts(ListAccountRequestObject requestObject)
        {
            var json = JsonConvert.SerializeObject(requestObject);
            var param = new[] {json};
            var response = await CallAction<List<ListAccountResponseObject>>("listAccounts", param);
            return response;
        }

        /// <summary>
        /// Gets all basic authorization informations of the client.
        /// </summary>
        /// <returns>A list with all basic authorization informations.</returns>
        public async Task<IReadOnlyList<ListBasicAuthResponseObject>> ListBasicAuth()
        {
            var response = await CallAction<List<ListBasicAuthResponseObject>>("listBasicAuth");
            return response;
        }

        /// <summary>
        /// Gets all available premium hoster names of the client.
        /// </summary>
        /// <returns>A list of all available premium hoster names.</returns>
        public async Task<IReadOnlyList<string>> ListPremiumHoster()
        {
            var response = await CallAction<List<string>>("listPremiumHoster");
            return response;
        }

        /// <summary>
        /// Gets all premium hoster names + urls that JDownloader supports.
        /// </summary>
        /// <returns>Returns a dictionary containing the hostername as the key and the url as the value.</returns>
        public async Task<IReadOnlyDictionary<string, string>> ListPremiumHosterUrls()
        {
            var response = await CallAction<Dictionary<string, string>>("listPremiumHosterUrls");
            return response;
        }

        /// <summary>
        /// Refreshes all the account informations stored on the device.
        /// </summary>
        /// <param name="accountIds">The account ids you want to refresh.</param>
        /// <returns>True if successfull</returns>
        public async Task<bool> RefreshAccounts(long[] accountIds)
        {
            var param = new[] {accountIds};
            var response = await CallAction<DefaultReturnObject>("refreshAccounts", param);
            return response != null;
        }

        /// <summary>
        /// Removes accounts stored on the device.
        /// </summary>
        /// <param name="accountIds">The account ids you want to remove.</param>
        /// <returns>True if successfull.</returns>
        public async Task<bool> RemoveAccounts(long[] accountIds)
        {
            var param = new[] {accountIds};
            var response = await CallAction<bool>("removeAccounts", param);
            return response;
        }

        /// <summary>
        /// Removes basic auth informations stored on the device.
        /// </summary>
        /// <param name="basicAuthIds">The basic auth ids you want to remove.</param>
        /// <returns>True if successfull.</returns>
        public async Task<bool> RemoveBasicAuths(long[] basicAuthIds)
        {
            var param = new[] {basicAuthIds};
            var response = await CallAction<bool>("removeBasicAuths", param);
            return response;
        }

        /// <summary>
        /// Updates the account data for the given account id.
        /// </summary>
        /// <param name="accountId">The id of the account you want to update.</param>
        /// <param name="email">The old/new email.</param>
        /// <param name="password">The old/new password</param>
        /// <returns>Ture if successfull</returns>
        public async Task<bool> SetUsernameAndPassword(long accountId, string email, string password)
        {
            var param = new[] {accountId.ToString(), email, password};
            var response = await CallAction<bool>("setUserNameAndPassword", param);
            return response;
        }

        /// <summary>
        /// Updates an basic auth entry.
        /// </summary>
        /// <param name="requestObject">The updated basic auth informations.</param>
        /// <returns>True if successfull.</returns>
        public async Task<bool> UpdateBasicAuth(BasicAuthObject requestObject)
        {
            var param = new[] {JsonConvert.SerializeObject(requestObject)};
            var response = await CallAction<bool>("updateBasicAuth", param);
            return response;
        }
    }
}