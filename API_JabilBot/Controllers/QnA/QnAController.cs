using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_JabilBot.Models;
using API_JabilBot.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API_JabilBot.Controllers
{
    //[Produces("application/json")]
    [Route("apibot/v1/QnA")]
    public class QnAController : Controller
    {
        private static JObject result;
        private static Error errorObj = new Error();
        private IQnAService iQnAService;
        private readonly AppSettings _appSettings;

        public QnAController(IOptions<AppSettings> appsettings,
                             IQnAService qnaservice)
        {
            this._appSettings = appsettings.Value;
            this.iQnAService = qnaservice;
        }

        [HttpPost]
        [ActionName("GetQnA")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        //[AllowAnonymous]
        public async Task<IActionResult> GetQnA([FromBody] JObject question)
        {
            try
            {
                result = await iQnAService.GetQnAAsync(question);
            }
            catch (Exception ex)
            {
                errorObj.Action = this.ControllerContext.RouteData.Values["action"].ToString();
                errorObj.Controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                errorObj.Method = "POST";
                errorObj.Message = ex.Message;
                errorObj.StatusCode = "500";
                return StatusCode(500, JObject.Parse(JsonConvert.SerializeObject(errorObj)));
            }

            return (result["answers"][0]["answer"] != null) ? Ok(result["answers"][0]["answer"]) : StatusCode(500, result);
        }
    }
}
