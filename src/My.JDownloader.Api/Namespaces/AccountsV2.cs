﻿using System.Collections.Generic;
using My.JDownloader.Api.ApiHandler;
using My.JDownloader.Api.ApiObjects;
using My.JDownloader.Api.ApiObjects.AccountV2;
using My.JDownloader.Api.ApiObjects.Devices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace My.JDownloader.Api.Namespaces
{
    public class AccountsV2
    {
        private readonly DeviceObject _Device;

        internal AccountsV2(DeviceObject device)
        {
            _Device = device;
        }

        /// <summary>
        /// Adds an premium account to your JDownloader device.
        /// </summary>
        /// <param name="hoster">The hoster e.g. mega.co.nz</param>
        /// <param name="email">Your email</param>
        /// <param name="password">Your password</param>
        /// <returns>True if the account was successfully added.</returns>
        public bool AddAccount(string hoster, string email, string password)
        {
            var param = new[] {hoster, email, password};
            var response = JDownloaderApiHandler.CallAction<DefaultReturnObject>(_Device, "/accountsV2/addAccount",
                param, JDownloaderHandler.LoginObject);

            return response != null;
        }

        /// <summary>
        /// Adds an basic authorization to the client.
        /// </summary>
        /// <param name="requestObject">Contains the needed properties for the request e.g. the username and password.</param>
        /// <returns>True if successfull.</returns>
        public bool AddBasicAuth(BasicAuthObject requestObject)
        {
            var param = new[]
                {requestObject.Type.ToString(), requestObject.Hostmask, requestObject.Username, requestObject.Password};
            var response = JDownloaderApiHandler.CallAction<DefaultReturnObject>(_Device, "/accountsV2/addBasicAuth",
                param, JDownloaderHandler.LoginObject);

            return response != null;
        }

        /// <summary>
        /// Disables an account to download.
        /// </summary>
        /// <param name="accountIds">The account ids you want to disable.</param>
        /// <returns>True if succesfull</returns>
        public bool DisableAccounts(long[] accountIds)
        {
            var param = new[] {accountIds};
            var response = JDownloaderApiHandler.CallAction<DefaultReturnObject>(_Device, "/accountsV2/disableAccounts",
                param, JDownloaderHandler.LoginObject);

            return response != null;
        }

        /// <summary>
        /// Enables an account to download.
        /// </summary>
        /// <param name="accountIds">The account ids you want to enable.</param>
        /// <returns>True if succesfull</returns>
        public bool EnableAccounts(long[] accountIds)
        {
            var param = new[] {accountIds};
            var response = JDownloaderApiHandler.CallAction<DefaultReturnObject>(_Device, "/accountsV2/enableAccounts",
                param, JDownloaderHandler.LoginObject);

            return response != null;
        }

        /// <summary>
        /// Gets a link of a hoster by the name of it.
        /// </summary>
        /// <param name="hoster">Name of the hoster you want the url from.</param>
        /// <returns>The url of the hoster.</returns>
        public async Task<string> GetPremiumHosterUrl(string hoster)
        {
            var param = new[] {hoster};
            var response = await JDownloaderApiHandler.CallAction<DefaultReturnObject>(_Device, "/accountsV2/getPremiumHosterUrl",
                param, JDownloaderHandler.LoginObject);
            if (response?.Data != null)
                return response.Data.ToString();
            return "";
        }

        /// <summary>
        /// Lists all accounts which are stored on the device.
        /// </summary>
        /// <param name="requestObject">Contains properties like Username (boolean) etc. If set to true the api will return the Username.</param>
        /// <returns>A list of all accounts stored on the device.</returns>
        public async Task<ListAccountResponseObject[]> ListAccounts(ListAccountRequestObject requestObject)
        {
            string json = JsonConvert.SerializeObject(requestObject);
            var param = new[] {json};
            var response = await JDownloaderApiHandler.CallAction<ListAccountResponseObject[]>(_Device, "/accountsV2/listAccounts", param,
                JDownloaderHandler.LoginObject);

            return response;
        }

        /// <summary>
        /// Gets all basic authorization informations of the client.
        /// </summary>
        /// <returns>A list with all basic authorization informations.</returns>
        public async Task<ListBasicAuthResponseObject[]> ListBasicAuth()
        {
            var response = await JDownloaderApiHandler.CallAction<ListBasicAuthResponseObject[]>(_Device, "/accountsV2/listBasicAuth", null,
                JDownloaderHandler.LoginObject);

            return response;
        }

        /// <summary>
        /// Gets all available premium hoster names of the client.
        /// </summary>
        /// <returns>A list of all available premium hoster names.</returns>
        public async Task<string[]> ListPremiumHoster()
        {
            var response = await JDownloaderApiHandler.CallAction<string[]>(_Device, "/accountsV2/listPremiumHoster", null,
                JDownloaderHandler.LoginObject);

            return response;
        }

        /// <summary>
        /// Gets all premium hoster names + urls that JDownloader supports.
        /// </summary>
        /// <returns>Returns a dictionary containing the hostername as the key and the url as the value.</returns>
        public async Task<Dictionary<string, string>> ListPremiumHosterUrls()
        {
            var response = await JDownloaderApiHandler.CallAction<DefaultReturnObject>(_Device, "/accountsV2/listPremiumHosterUrls",
                null,
                JDownloaderHandler.LoginObject);
            var tmp = ((JObject) response.Data);
            if (tmp != null)
                return tmp.ToObject<Dictionary<string, string>>();

            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Refreshes all the account informations stored on the device.
        /// </summary>
        /// <param name="accountIds">The account ids you want to refresh.</param>
        /// <returns>True if successfull</returns>
        public bool RefreshAccounts(long[] accountIds)
        {
            var param = new[] {accountIds};
            var response = JDownloaderApiHandler.CallAction<DefaultReturnObject>(_Device, "/accountsV2/refreshAccounts",
                param, JDownloaderHandler.LoginObject);

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
            var response = await JDownloaderApiHandler.CallAction<bool>(_Device,"/accountsV2/removeAccounts",
                param, JDownloaderHandler.LoginObject);

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
            var response = await JDownloaderApiHandler.CallAction<bool>(_Device, "/accountsV2/removeBasicAuths",
                param, JDownloaderHandler.LoginObject);

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
            var response = await JDownloaderApiHandler.CallAction<bool>(_Device, "/accountsV2/setUserNameAndPassword",
                param, JDownloaderHandler.LoginObject);

            return response;
        }

        /// <summary>
        /// Updates an basic auth entry.
        /// </summary>
        /// <param name="requestObject">The updated basic auth informations.</param>
        /// <returns>True if successfull.</returns>
        public async Task<bool> UpdateBasicAuth(BasicAuthObject requestObject)
        {
            var param = new[] {  JsonConvert.SerializeObject(requestObject)};
            var response = await JDownloaderApiHandler.CallAction<bool>(_Device, "/accountsV2/updateBasicAuth",
                param, JDownloaderHandler.LoginObject);

            return response;
        }
    }
}