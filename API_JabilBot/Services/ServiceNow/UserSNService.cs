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
    public class UserSNService : IUserSNService
    {
        private readonly AppSettings _appSettings;

        private static HttpClient client;
        private static HttpResponseMessage response;
        private static Error errorObj;
        private static StringBuilder url;
        private static string responseBody;

        public UserSNService(IOptions<AppSettings> appsettings)
        {
            this._appSettings = appsettings.Value;
        }

        public async Task<JObject> GetUserInfoByEmployeeNumberAsync(string server, string employeeNumber)
        {
            try
            {
                client = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes(_appSettings.UserSN + ":" + _appSettings.PassSN);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                url = new StringBuilder(_appSettings.ServiceNow_BaseUrl);
                url.Insert(url.ToString().IndexOf("://") + 3, server)
                    .Append("sys_user?employee_number=")
                    .Append(employeeNumber);
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
                errorObj.Method = "GetUserInfoByEmployeeNumberAsync";
                errorObj.Message = response.ReasonPhrase.ToString();
                return JObject.Parse(JsonConvert.SerializeObject(errorObj));
            }

            return JObject.Parse(responseBody);
        }

        public async Task<JObject> GetUserInfoBySysIdAsync(string server, string id)
        {
            try
            {
                client = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes(_appSettings.UserSN + ":" + _appSettings.PassSN);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                url = new StringBuilder(_appSettings.ServiceNow_BaseUrl);
                url.Insert(url.ToString().IndexOf("://") + 3, server)
                    .Append("sys_user/")
                    .Append(id);
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
                errorObj.Method = "GetUserInfoBySysIdAsync";
                errorObj.Message = response.ReasonPhrase.ToString();
                return JObject.Parse(JsonConvert.SerializeObject(errorObj));
            }

            return JObject.Parse(responseBody);
        }
    }
}
