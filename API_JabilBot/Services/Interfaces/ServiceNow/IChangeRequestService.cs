using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_JabilBot.Services.Interfaces
{
    public interface IChangeRequestService
    {
        Task<JObject> GetChangeRequestByNumberAsync(string chg);
        Task<JObject> GetChangeRequestsByServerAsync(string configItem);
        //Task<JObject> GetChangeRequestsAsync();
    }
}
