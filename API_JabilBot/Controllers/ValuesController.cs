using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API_JabilBot.Controllers
{
    //[Route("apibot/[controller]")]
    [Route("apibot/status")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            //return new string[] { "value1", "value2" };
            return "API BOT is working !!!";
        }
        
    }
}
