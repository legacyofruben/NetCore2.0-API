using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_JabilBot.Services.Interfaces.ServiceNow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_JabilBot.Controllers.ServiceNow
{
    //[Produces("application/json")]
    [Route("apibot/v1/ServiceNowValidation")]
    public class ValidationServerController : Controller
    {
        private IGeneralFunctionsService igeneralFunctionsService;

        public ValidationServerController(IGeneralFunctionsService generalFunctionsService)
        {
            this.igeneralFunctionsService = generalFunctionsService;
        }


        [HttpGet("{server}", Name = "GetValidationServer")]
        public async Task<IActionResult> GetValidationServer(string server)
        {
            try
            {
                if (await igeneralFunctionsService.ValidationServerAsync(server))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error");
            }

        }
    }
}
