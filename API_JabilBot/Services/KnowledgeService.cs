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
    public class KnowledgeService : IKnowledgeService
    {

        private readonly AppSettings _appSettings;

        private static HttpClient client;
        private static HttpResponseMessage response;
        private static Error errorObj;
        private static StringBuilder url;
        private static string responseBody;
        private static string author_name;

        public KnowledgeService(IOptions<AppSettings> appsettings)
        {
            this._appSettings = appsettings.Value;
        }
        public async Task<JObject> GetKnowledgeByNumberAsync(string server, string kb)
        {
            
            try
            {
                client = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes(_appSettings.UserSN + ":" + _appSettings.PassSN);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                url = new StringBuilder(_appSettings.ServiceNow_BaseUrl);
                url.Insert(url.ToString().IndexOf("://") + 3, server)
                    .Append("kb_knowledge?number=")
                    .Append(kb);
                HttpResponseMessage response = await client.GetAsync(url.ToString());
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();


                    //jsonBefore = JObject.Parse(responseBody);
                    //link = jsonBefore["result"][0]["author"]["link"].ToString();
                    //userInfo = await GetAuthorKnowledgeAsync(link);
                    //resultBefore = new StringBuilder(responseBody);
                    //author_name_username = "\"author_name_username\":\"" + userInfo["result"]["u_name_id"].ToString() + "\",";
                    //resultBefore.Insert(responseBody.IndexOf("\"author\"") + author_name_username.Length,
                    //                    author_name_username);
                    //responseBody = resultBefore.ToString();


                    //resultBefore = JObject.Parse(responseBody);
                    //userInfo = await GetAuthorKnowledgeAsync(resultBefore["result"][0]["author"]["link"].ToString());
                    //var item = resultBefore["result"][0] as JObject;
                    //item.Add("author_name_username", userInfo["result"]["u_name_id"].ToString());
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
                errorObj.Method = "GetKnowledgeByNumberAsync";
                errorObj.Message = response.ReasonPhrase.ToString();
                return JObject.Parse(JsonConvert.SerializeObject(errorObj));
            }


            return JObject.Parse(responseBody);
            //return (resultBefore == null)? JObject.Parse(responseBody):resultBefore;
        }

        public async Task<JObject> GetKnowledgesByServerAsync(string server, string description)
        {
            JObject userInfo = null;
            JObject resultBefore = null;
            try
            {
                client = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes(_appSettings.UserSN + ":" + _appSettings.PassSN);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                url = new StringBuilder(_appSettings.ServiceNow_BaseUrl);
                url.Insert(url.ToString().IndexOf("://") + 3, server)
                    .Append("kb_knowledge?sysparm_limit=50&sysparm_fields=number,short_description&active=true&sysparm_query=short_descriptionCONTAINS")
                    .Append(description.Trim())
                    .Append("^ORDERBYnumber");
                //url.Insert(url.ToString().IndexOf("://") + 3, server)
                //    .Append("kb_knowledge?sysparm_list_mode=grid&sysparm_query=sys_created_on>javascript:gs.daysAgoStart(30)&sysparm_offset=");
                response = await client.GetAsync(url.ToString());
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                    //resultBefore = JObject.Parse(responseBody);
                    //foreach (JObject i in resultBefore["result"])
                    //{
                    //    userInfo = await GetAuthorKnowledgeAsync(i["author"]["link"].ToString());
                    //    i.Add("author_name_username", userInfo["result"]["name"].ToString());
                    //}


                }
            }
            catch (Exception ex)
            {
                errorObj.StatusCode = response.StatusCode.ToString();
                errorObj.Action = response.RequestMessage.RequestUri.LocalPath.ToString();
                errorObj.Method = "GetKnowledgesByServerAsync";
                errorObj.Message = response.ReasonPhrase.ToString();
                return JObject.Parse(JsonConvert.SerializeObject(errorObj));
            }

            return JObject.Parse(responseBody);
        }

        public async Task<JObject> GetAuthorKnowledgeAsync(string link)
        {
            try
            {
                client = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes(_appSettings.UserSN + ":" + _appSettings.PassSN);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                response = await client.GetAsync(link);
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
                errorObj.Method = "GetAuthorKnowledgeAsync";
                errorObj.Message = response.ReasonPhrase.ToString();
                return JObject.Parse(JsonConvert.SerializeObject(errorObj));
            }

            return JObject.Parse(responseBody);
        }
    }
}
