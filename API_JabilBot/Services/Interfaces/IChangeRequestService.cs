using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_JabilBot.Services.Interfaces
{
    public interface IChangeRequestService
    {
        Task<JObject> GetChangeRequestByNumberAsync(string server, string chg);
        Task<JObject> GetChangeRequestsByServerAsync(string server);
        //Task<JObject> GetChangeRequestsAsync();
    }
}
