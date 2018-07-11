using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_JabilBot.Models.Contexts;
using API_JabilBot.Models;
using API_JabilBot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using API_JabilBot.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace API_JabilBot.Controllers
{
    //[Produces("application/json")]
    [Route("apibot/v1/employees")]
    public class EmployeeController : Controller
    {

       
        // Contexts
        private readonly EmployeeContext _Econtext;
        private readonly AppSettings _appSettings;
        //private readonly ItemContext _Icontext;

        // Services
        private IEmployeeService iEmployeeService;


        //public EmployeeController(ItemContext context)
        //{
        //    _Icontext = context;

        //    if (_Icontext.TodoItems.Count() == 0)
        //    {
        //        _Icontext.TodoItems.Add(new Item { Name = "Full name" });
        //        _Icontext.SaveChanges();
        //    }
        //}

        public EmployeeController(EmployeeContext context,
                                  IEmployeeService EmployeeService,
                                  IOptions<AppSettings> appsettings)
        {
            _Econtext = context;

            this.iEmployeeService = EmployeeService;
            this._appSettings = appsettings.Value;

            if (_Econtext.Employees.Count() == 0)
            {
                _Econtext.Employees.Add(new Employee()
                {
                    EMail = "test@jabil.com",
                    FullName = _appSettings.UserSN
                });
                _Econtext.SaveChanges();
            }
        }


        //---------------------------------------------------

        // GET: api/Employee
        [HttpGet]
        public List<Employee> Get()
        {
            return _Econtext.Employees.ToList();
            //return _Icontext.TodoItems.ToList();
        }

        // GET: apibot/v1/employees/5
        [HttpGet("{id}", Name = "GetByEmployeeNumber")]
        public IActionResult GetByEmployeeNumber(long id)
        {
            var employee = _Econtext.Employees.Find(id);
            if (employee == null)
            {
                return BadRequest();
            }
            return Ok(employee);
        }

        [Produces("application/json")]
        // GET: apibot/v1/employees/employeenumber/5
        [HttpGet("employeenumber/{EmployeeNumber}", Name = "Get")]
        public IActionResult Get(string EmployeeNumber)
        {
          //EmployeeService IServiceEmployee = new EmployeeService();
            return Ok(iEmployeeService.GetEmployeeByUserName(EmployeeNumber));
        }

        // GET: apibot/v1/employees/email/email@jabil.com
        [HttpGet("email/{email}", Name = "GetByEmail")]
        public IActionResult GetByEmail(string email)
        {
            return Ok(iEmployeeService.GetEmployeeByEmail(email));
        }

        // GET: apibot/v1/employees/site/Guadalajara
        [HttpGet("site/{site}", Name = "GetBySite")]
        public IActionResult GetBySite(string site)
        {
            return Ok(iEmployeeService.GetEmployeesBySite(site));
        }






        // POST: api/Employee
        [HttpPost]
        public IActionResult Post([FromBody]Employee employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }
            _Econtext.Employees.Add(employee);
            _Econtext.SaveChanges();

            return CreatedAtRoute("GetByEmployeeNumber", new { id = employee.id }, employee);
        }
        
        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        
    }
}
