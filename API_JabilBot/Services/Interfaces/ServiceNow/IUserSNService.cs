using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_JabilBot.Services.Interfaces
{
    public interface IUserSNService
    {
        Task<JObject> GetUserInfoByEmployeeNumberAsync(string server, string employeeNumber);
        Task<JObject> GetUserInfoBySysIdAsync(string server, string id);
    }
}
