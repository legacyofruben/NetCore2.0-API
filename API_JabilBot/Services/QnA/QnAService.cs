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
    public class QnAService : IQnAService
    {

        private readonly AppSettings _appSettings;

        private static HttpClient client;
        private static HttpResponseMessage response;
        private static Error errorObj;
        private static StringBuilder url;
        private static string responseBody;
        private static string author_name;

        public QnAService(IOptions<AppSettings> appsettings)
        {
            this._appSettings = appsettings.Value;
        }

        public async Task<JObject> GetQnAAsync(JObject body)
        {
            try
            {

                client = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes(_appSettings.QnAAuthKey);
                client.DefaultRequestHeaders.Add("Authorization", "EndpointKey ccaf8121-990b-4f03-b2b5-9106203d2059");
                url = new StringBuilder(_appSettings.QnAEndpointHostName +
                                        _appSettings.QnAKnowledgebaseId);
                HttpResponseMessage response = await client.PostAsync(url.ToString(),
                                                                      new StringContent(body.ToString(), Encoding.UTF8, "application/json"));
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
                errorObj.Method = "GetQnAAsync";
                errorObj.Message = response.ReasonPhrase.ToString();
                return JObject.Parse(JsonConvert.SerializeObject(errorObj));
            }


            return JObject.Parse(responseBody);
        }
    }
}
