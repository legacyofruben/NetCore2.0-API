using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_JabilBot.Models;
using API_JabilBot.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API_JabilBot.Controllers
{
    //[Produces("application/json")]
    [Route("apibot/v1/Knowledge")]
    public class KnowledgeController : Controller
    {

        private static JObject result;
        private static Error errorObj = new Error();
        private static IHeaderDictionary headerValues;

        private IKnowledgeService iKnowledgeService;
        private readonly AppSettings _appSettings;

        public KnowledgeController(IOptions<AppSettings> appsettings,
                                  IKnowledgeService KnowledgeService)
        {
            this.iKnowledgeService = KnowledgeService;
            this._appSettings = appsettings.Value;
        }

        [HttpGet("{kb}", Name = "GetKnowledgeByNumber")]
        public async Task<IActionResult> GetKnowledgeByNumber(string kb)
        {
            JObject userInfo = null;
            StringBuilder resultBefore = null;
            string link = "";
            string author_name_username = "";
            try
            {
                result = await iKnowledgeService.GetKnowledgeByNumberAsync(kb);

                link = result["result"][0]["author"]["link"].ToString();
                userInfo = await iKnowledgeService.GetAuthorKnowledgeAsync(link);
                resultBefore = new StringBuilder(result.ToString());
                author_name_username = "\"author_name\":\"" + userInfo["result"]["u_name_id"].ToString() + "\",";
                resultBefore.Insert(resultBefore.ToString().IndexOf(kb) + kb.Length + 2,
                                    author_name_username);
                result = JObject.Parse(resultBefore.ToString());

            }
            catch (Exception ex)
            {
                errorObj.Action = this.ControllerContext.RouteData.Values["action"].ToString();
                errorObj.Controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                errorObj.Method = "GET";
                errorObj.Message = ex.Message;
                errorObj.StatusCode = "500";
                return StatusCode(500, JObject.Parse(JsonConvert.SerializeObject(errorObj)));
            }
            return (result["result"] != null) ? Ok(result["result"]) : StatusCode(500, result);
        }

        [HttpGet(Name = "GetKnowledgesByServer")]
        public async Task<IActionResult> GetKnowledgesByServer()
        {
            JObject userInfo = null;
            StringBuilder resultBefore = null;
            string link = "";
            string resultString = "";
            string name = "";
            string number = "";
            try
            {
                result = await iKnowledgeService.GetKnowledgesByServerAsync("desc");
                resultString = result.ToString();
                resultBefore = new StringBuilder(result.ToString());

                foreach (JObject i in result["result"])
                {
                    link = i["author"]["link"].ToString();
                    number = i["number"].ToString();
                    userInfo = await iKnowledgeService.GetAuthorKnowledgeAsync(link);
                    name = "\"author_name\":\"" + userInfo["result"]["u_name_id"].ToString() + "\",";
                    resultBefore.Insert(resultBefore.ToString().IndexOf(number) + number.Length + 2,
                                        name);
                }

                result = JObject.Parse(resultBefore.ToString());

            }
            catch (Exception ex)
            {
                errorObj.Action = this.ControllerContext.RouteData.Values["action"].ToString();
                errorObj.Controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                errorObj.Method = "GET";
                errorObj.Message = ex.Message;
                errorObj.StatusCode = "500";
                return StatusCode(500, JObject.Parse(JsonConvert.SerializeObject(errorObj)));
            }

            return (result["result"] != null) ? Ok(result["result"]) : StatusCode(500, result);
        }

        [HttpPost]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByHeader = "description")]
        public async Task<IActionResult> GetKnowledgesByDescription([FromForm] string description)
        {
            //JObject userInfo = null;
            //StringBuilder resultBefore = null;
            //string link = "";
            //string resultString = "";
            //string name = "";
            //string number = "";
            try
            {
                result = await iKnowledgeService.GetKnowledgesByServerAsync(description);
                //headerValues = this.HttpContext.Request.Headers;
                //if (headerValues.ContainsKey("server"))
                //{
                //    result = await iKnowledgeService.GetKnowledgesByServerAsync(description);
                //    //resultString = result.ToString();
                //    //resultBefore = new StringBuilder(result.ToString());

                //    //foreach (JObject i in result["result"])
                //    //{
                //    //    link = i["author"]["link"].ToString();
                //    //    number = i["number"].ToString();
                //    //    userInfo = await iKnowledgeService.GetAuthorKnowledgeAsync(link);
                //    //    name = "\"author_name\":\"" + userInfo["result"]["u_name_id"].ToString() + "\",";
                //    //    resultBefore.Insert(resultBefore.ToString().IndexOf(number) + number.Length + 2,
                //    //                        name);
                //    //}

                //    //result = JObject.Parse(resultBefore.ToString());
                //}
                //else
                //{
                //    return Unauthorized();
                //}

            }
            catch (Exception ex)
            {
                errorObj.Action = this.ControllerContext.RouteData.Values["action"].ToString();
                errorObj.Controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                errorObj.Method = "GET";
                errorObj.Message = ex.Message;
                errorObj.StatusCode = "500";
                return StatusCode(500, JObject.Parse(JsonConvert.SerializeObject(errorObj)));
            }

            return (result["result"] != null) ? Ok(result["result"]) : StatusCode(500, result);
        }

    }
}
