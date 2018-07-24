using API_JabilBot.Models;
using API_JabilBot.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public async Task<JObject> GetChangeRequestByNumberAsync(string changeRequest)
        {
            try
            {
                client = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes(_appSettings.UserSN + ":" + _appSettings.PassSN);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                url = new StringBuilder(_appSettings.ServiceNow_BaseUrl);
                url.Append("change_request?number=")
                   .Append(changeRequest);
                //url.Insert(url.ToString().IndexOf("://") + 3, server)
                //    .Append("change_request?number=")
                //    .Append(changeRequest);
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

        public async Task<JObject> GetChangeRequestsByServerAsync(string configItem)
        {
            JObject aux = null;
            JObject result = new JObject(new JProperty("result", new JArray()));
            JArray arrayResult = result["result"] as JArray;
            SortedList<string, JToken> sortedList = new SortedList<string, JToken>();
            try
            {
                client = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes(_appSettings.UserSN + ":" + _appSettings.PassSN);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                url = new StringBuilder(_appSettings.ServiceNow_BaseUrl);
                url.Append("change_request?sysparm_list_mode=grid&sysparm_fields=sys_id,number,description,start_date,end_date,phase_state&active=true")
                   .Append("&sysparm_query=cmdb_ciSTARTSWITH")
                   .Append(configItem)
                   .Append("^start_dateONlast7days@javascript:gs.daysAgoStart(7)@javascript:gs.daysAgoEnd(0)")
                   .Append("^ORDERBYnumber");
                
                //response = await client.GetAsync(url.ToString());
                response = client.GetAsync(url.ToString()).Result;
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                    result = JObject.Parse(responseBody);

                }
                else
                {
                    Debug.WriteLine("ERROR: HTTP request: " + response.StatusCode);
                    Debug.WriteLine("ERROR: HTTP request: " + url.ToString());
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

            return result;
        }

        //public async Task<JObject> GetChangeRequestsByServerAsync(string configItem)
        //{
        //    JObject aux = null;
        //    JObject result = new JObject(new JProperty("result", new JArray()));
        //    JArray arrayResult = result["result"] as JArray;
        //    SortedList<string, JToken> sortedList =new SortedList<string, JToken>();
        //    try
        //    {
        //        client = new HttpClient();
        //        var byteArray = Encoding.ASCII.GetBytes(_appSettings.UserSN + ":" + _appSettings.PassSN);
        //        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        //        url = new StringBuilder(_appSettings.ServiceNow_CMDB_CI);
        //        // Search Configuration Item by name
        //        url.Append("?sysparm_limit=100&sysparm_fields=sys_id,name&u_substatus=in_use&sysparm_query=nameCONTAINS")
        //           .Append(configItem);

        //        //Debug.WriteLine("Get CI: " + url.ToString() );

        //        //response = await client.GetAsync(url.ToString());
        //        response = client.GetAsync(url.ToString()).Result;
        //        response.EnsureSuccessStatusCode();
        //        if (response.IsSuccessStatusCode)
        //        {
        //            responseBody = await response.Content.ReadAsStringAsync();

        //            JObject configItemSearch = JObject.Parse(responseBody);
        //            //Debug.WriteLine("Get CI count: " + (JObject)configItemSearch["result"].Count().ToString());
        //            foreach (var item in configItemSearch["result"])
        //            {
        //                //Debug.WriteLine("ITEM: " + item.ToString());
        //                if (!item["sys_id"].Equals("") && item["sys_id"] != null)
        //                {
        //                    url = null;
        //                    url = new StringBuilder(_appSettings.ServiceNow_BaseUrl);
        //                    // Search by Configuration Item
        //                    url.Append("change_request?sysparm_list_mode=grid&sysparm_fields=sys_id,number,description,start_date,end_date,phase_state&active=true&cmdb_ci.value=")
        //                       .Append(item["sys_id"])
        //                       .Append("&sysparm_query=sys_created_on>javascript:gs.daysAgoStart(7)")
        //                       .Append("^ORDERBYnumber");

        //                    //response = await client.GetAsync(url.ToString());
        //                    response = client.GetAsync(url.ToString()).Result;
        //                    response.EnsureSuccessStatusCode();
        //                    if (response.IsSuccessStatusCode)
        //                    {
        //                        Debug.WriteLine("URL to CH: " + url.ToString());
        //                        responseBody = response.Content.ReadAsStringAsync().Result;
        //                        aux = JObject.Parse(responseBody);
        //                        foreach (var itemCh in aux["result"])
        //                        {
        //                            try
        //                            {
        //                                var jitem = itemCh as JObject;
        //                                jitem.Property("start_date").AddAfterSelf(new JProperty("config_item", item["name"].ToString()));
        //                            }
        //                            catch (Exception)
        //                            {
        //                                continue;
        //                            }

        //                        }
        //                        foreach (var i in aux["result"] as JToken)
        //                        {
        //                            if (!sortedList.ContainsKey(i["number"].ToString()))
        //                            {
        //                                sortedList.Add(i["number"].ToString(), i);
        //                                Debug.WriteLine("Item Added: " + i["number"].ToString());
        //                            }
        //                        }
        //                    }
        //                }

        //            }
        //            foreach (var item in sortedList)
        //            {
        //                arrayResult.Add(item.Value);
        //            }

        //            //if (!configItemSearch["result"][0]["sys_id"].Equals("") && configItemSearch["result"][0]["sys_id"] != null)
        //            //{
        //            //    url = new StringBuilder(_appSettings.ServiceNow_BaseUrl);
        //            //    // Search by Configuration Item
        //            //    url.Append("change_request?sysparm_list_mode=grid&sysparm_fields=sys_id,number,description,start_date,end_date,phase_state&active=true&cmdb_ci.value=")
        //            //       .Append(configItemSearch["result"][0]["sys_id"])
        //            //       .Append("&sysparm_query=sys_created_on>javascript:gs.daysAgoStart(14)")
        //            //       .Append("^ORDERBYnumber");
        //            //    //url.Insert(url.ToString().IndexOf("://") + 3, server)
        //            //    //    .Append("change_request?sysparm_list_mode=grid&sysparm_fields=sys_id,number,description,start_date,end_date,phase_state&active=true&sysparm_query=sys_created_on>javascript:gs.daysAgoStart(14)")
        //            //    //    .Append("^ORDERBYnumber");
        //            //    response = await client.GetAsync(url.ToString());
        //            //    response.EnsureSuccessStatusCode();
        //            //    if (response.IsSuccessStatusCode)
        //            //    {
        //            //        responseBody = await response.Content.ReadAsStringAsync();
        //            //        result = JObject.Parse(responseBody);
        //            //        foreach (var item in result["result"])
        //            //        {
        //            //            var jitem = item as JObject;
        //            //            jitem.Property("start_date").AddAfterSelf(new JProperty("config_item", configItemSearch["result"][0]["name"].ToString()));
        //            //        }
        //            //    }
        //            //}

        //        }
        //        else
        //        {
        //            Debug.WriteLine("ERROR: HTTP request: " + response.StatusCode);
        //            Debug.WriteLine("ERROR: HTTP request: " + url.ToString());
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        errorObj.StatusCode = response.StatusCode.ToString();
        //        errorObj.Action = response.RequestMessage.RequestUri.LocalPath.ToString();
        //        errorObj.Method = "GetChangeRequestsByServerAsync";
        //        errorObj.Message = response.ReasonPhrase.ToString();
        //        return JObject.Parse(JsonConvert.SerializeObject(errorObj));
        //    }

        //    return result;
        //}

    }
}
