using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("apibot/v1/[controller]")]
    public class IncidentController : Controller
    {

        private static JObject result;
        private static Error errorObj = new Error();
        private static IHeaderDictionary headerValues;

        private IIncidentService iIncidentService;
        private readonly AppSettings _appSettings;

        public IncidentController(IOptions<AppSettings> appsettings,
                                  IIncidentService incidentService)
        {
            this.iIncidentService = incidentService;
            this._appSettings = appsettings.Value;
        }

        [HttpGet("{inc}", Name = "GetIncidentByNumber")]
        public async Task<IActionResult> GetIncidentByNumber(string inc)
        {
            try
            {
                result = await iIncidentService.GetIncidenByNumberAsync(inc);
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

        [HttpGet(Name = "GetIncidentsByServer")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByHeader = "config_item")]
        public async Task<IActionResult> GetChangeRequestsByServer()
        {
            try
            {
                headerValues = this.HttpContext.Request.Headers;
                if (headerValues.ContainsKey("config_item"))
                {
                    result = await iIncidentService.GetIncidentsByServerAsync(headerValues["config_item"].ToString());
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
    }
}
