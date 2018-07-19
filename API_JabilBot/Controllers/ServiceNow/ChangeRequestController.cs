using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
    [Route("apibot/v1/ChangeRequest")]
    public class ChangeRequestController : Controller
    {

        private static string server;
        private static JObject result;
        private static Error errorObj = new Error();
        private static IHeaderDictionary headerValues;

        private IChangeRequestService iChangeRequestService;
        private readonly AppSettings _appSettings;

        public ChangeRequestController(IOptions<AppSettings> appsettings,
                                       IChangeRequestService changeRequestService)
        {
            this._appSettings = appsettings.Value;
            this.iChangeRequestService = changeRequestService;
        }


        // GET: api/ChangeRequest
        //[Produces("application/json")]
        //[HttpGet (Name = "GetChangeRequests")]
        //public async Task<IActionResult> GetChangeRequestsAsync()
        //{

        //    var result = await iChangeRequestService.GetChangeRequestsAsync();
        //    if (result == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(result["result"]);
        //}


        //[Produces("application/json")]
        [HttpGet("{chg}", Name = "GetChangeRequestsByNumber")]
        public async Task<IActionResult> GetChangeRequestsByNumber(string chg)
        {
            try
            {
                headerValues = this.HttpContext.Request.Headers;
                if (headerValues.ContainsKey("server"))
                {
                    result = await iChangeRequestService.GetChangeRequestByNumberAsync(headerValues["server"].ToString(), chg);
                }
                else
                {
                    return Unauthorized();
                }
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



        [HttpGet(Name = "GetChangeRequestsByServer")]
        public async Task<IActionResult> GetChangeRequestsByServer()
        {
            try
            {
                headerValues = this.HttpContext.Request.Headers;
                if (headerValues.ContainsKey("server"))
                {
                    result = await iChangeRequestService.GetChangeRequestsByServerAsync(headerValues["server"].ToString());
                }
                else
                {
                    return Unauthorized();
                }
                
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
            return (result["result"] != null)? Ok(result["result"]) : StatusCode(500, result);
        }

    }
}
