using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API_JabilBot.Services.Interfaces
{
    public interface IEmployeeService
    {
        DataTable GetEmployeeByUserName(string username);
        DataTable GetEmployeeByEmail(string email);

        DataTable GetEmployeesBySite(string site);
    }
}
