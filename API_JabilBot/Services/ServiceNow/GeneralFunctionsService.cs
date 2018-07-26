using API_JabilBot.Models;
using API_JabilBot.Services.Interfaces.ServiceNow;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API_JabilBot.Services.ServiceNow
{
    public class GeneralFunctionsService : IGeneralFunctionsService
    {

        private readonly AppSettings _appSettings;

        private static HttpClient client;
        private static HttpResponseMessage response;
        private static Error errorObj;
        private static StringBuilder url;
        private static string responseBody;


        public GeneralFunctionsService(IOptions<AppSettings> appsettings)
        {
            this._appSettings = appsettings.Value;
        }
        public async Task<bool> ValidationServerAsync(string server)
        {
            try
            {
                client = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes(_appSettings.UserSN + ":" + _appSettings.PassSN);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                url = new StringBuilder(_appSettings.ServiceNow_BaseUrl);
                url.Insert(url.ToString().IndexOf("://") + 3, server);
                response = await client.GetAsync(url.ToString().Substring(0,(url.ToString().IndexOf(".com")+4)));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
