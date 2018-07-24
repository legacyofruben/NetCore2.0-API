using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_JabilBot.Services.Interfaces
{
    public interface IIncidentService
    {
        Task<JObject> GetIncidenByNumberAsync(string inc);

        Task<JObject> GetIncidentsByServerAsync(string configItem);
    }
}
