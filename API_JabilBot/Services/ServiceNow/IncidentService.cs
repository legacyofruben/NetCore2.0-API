﻿using API_JabilBot.Models;
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
    public class IncidentService : IIncidentService
    {
        private readonly AppSettings _appSettings;

        private static HttpClient client;
        private static HttpResponseMessage response;
        private static Error errorObj;
        private static StringBuilder url;
        private static string responseBody;

        public IncidentService(IOptions<AppSettings> appsettings)
        {
            this._appSettings = appsettings.Value;
        }
        public async Task<JObject> GetIncidenByNumberAsync(string inc)
        {
            try
            {
                client = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes(_appSettings.UserSN + ":" + _appSettings.PassSN);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                url = new StringBuilder(_appSettings.ServiceNow_BaseUrl);
                url.Append("incident?number=")
                   .Append(inc);
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
                errorObj.Method = "GetIncidenByNumberAsync";
                errorObj.Message = response.ReasonPhrase.ToString();
                return JObject.Parse(JsonConvert.SerializeObject(errorObj));
            }

            return JObject.Parse(responseBody);
        }

        public async Task<JObject> GetIncidentsByServerAsync(string configItem)
        {
            try
            {
                client = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes(_appSettings.UserSN + ":" + _appSettings.PassSN);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                url = new StringBuilder(_appSettings.ServiceNow_BaseUrl);
                url.Append("incident?sysparm_list_mode=grid&sysparm_fields=sys_id,number,short_description,opened_at&")
                   .Append("sysparm_query=cmdb_ciSTARTSWITH")
                   .Append(configItem)
                   .Append("^sys_created_onONlast7days@javascript:gs.daysAgoStart(7)@javascript:gs.daysAgoEnd(0)")
                   .Append("^ORDERBYnumber");
                //response = await client.GetAsync(url.ToString());
                response = client.GetAsync(url.ToString()).Result;
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    //responseBody = await response.Content.ReadAsStringAsync();
                    responseBody = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                errorObj.StatusCode = response.StatusCode.ToString();
                errorObj.Action = response.RequestMessage.RequestUri.LocalPath.ToString();
                errorObj.Method = "GetIncidentByServerAsync";
                errorObj.Message = response.ReasonPhrase.ToString();
                return JObject.Parse(JsonConvert.SerializeObject(errorObj));
            }

            return JObject.Parse(responseBody);
        }
    }
}
