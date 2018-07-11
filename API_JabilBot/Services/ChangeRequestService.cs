using API_JabilBot.Models;
using API_JabilBot.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API_JabilBot.Services
{
    public class ChangeRequestService : IChangeRequestService
    {

        private readonly AppSettings _appSettings;

        private static HttpClient client;
        private static HttpResponseMessage response;
        private static Error errorObj;
        private static StringBuilder url;
        private static string responseBody;

        public ChangeRequestService(IOptions<AppSettings> appsettings)
        {
            this._appSettings = appsettings.Value;
        }
        public async Task<JObject> GetChangeRequestByNumberAsync(string server, string changeRequest)
        {
            try
            {
                client = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes(_appSettings.UserSN + ":" + _appSettings.PassSN);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                url = new StringBuilder(_appSettings.ServiceNow_BaseUrl);
                url.Insert(url.ToString().IndexOf("://") + 3, server)
                    .Append("change_request?number=")
                    .Append(changeRequest);
                HttpResponseMessage response = await client.GetAsync(url.ToString());
                response.EnsureSuccessStatusCode(); 
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                else
                {

                    responseBody = response.StatusCode.ToString();
                }

            }
            catch (Exception ex)
            {
                errorObj.StatusCode = response.StatusCode.ToString();
                errorObj.Action = response.RequestMessage.RequestUri.LocalPath.ToString();
                errorObj.Method = "GetChangeRequestByNumberAsync";
                errorObj.Message = response.ReasonPhrase.ToString();
                return JObject.Parse(JsonConvert.SerializeObject(errorObj));
            }
            
            return JObject.Parse(responseBody);
        }

        //public async Task<JObject> GetChangeRequestsAsync()
        //{
        //    try
        //    {
        //        client = new HttpClient();
        //        var byteArray = Encoding.ASCII.GetBytes(_appSettings.UserSN + ":" + _appSettings.PassSN);
        //        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        //        HttpResponseMessage response = await client.GetAsync("https://jbldev03.service-now.com/api/now/table/change_request?sysparm_limit=10");
        //        response.EnsureSuccessStatusCode();
        //        if (response.IsSuccessStatusCode)
        //        {
        //            responseBody = await response.Content.ReadAsStringAsync();
        //        }
        //        else
        //        {
        //            responseBody = response.StatusCode.ToString();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        responseBody = @"[ Error: " + ex.Message.ToString() + "]";
        //        throw;
        //    }

        //    return JObject.Parse(responseBody);
        //}

        public async Task<JObject> GetChangeRequestsByServerAsync(string server)
        {
            try
            {
                client = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes(_appSettings.UserSN + ":" + _appSettings.PassSN);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                url = new StringBuilder(_appSettings.ServiceNow_BaseUrl);
                url.Insert(url.ToString().IndexOf("://") + 3, server)
                    .Append("change_request?sysparm_list_mode=grid&sysparm_fields=sys_id,number,description,start_date,end_date,phase_state&active=true&sysparm_query=sys_created_on>javascript:gs.daysAgoStart(14)")
                    .Append("^ORDERBYnumber");
                response = await client.GetAsync(url.ToString());
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                errorObj.StatusCode = response.StatusCode.ToString();
                errorObj.Action = response.RequestMessage.RequestUri.LocalPath.ToString();
                errorObj.Method = "GetChangeRequestsByServerAsync";
                errorObj.Message = response.ReasonPhrase.ToString();
                return JObject.Parse(JsonConvert.SerializeObject(errorObj));
            }

            return JObject.Parse(responseBody);
        }
    }
}
