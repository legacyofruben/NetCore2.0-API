using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_JabilBot.Services.Interfaces
{
    public interface IQnAService
    {
        Task<JObject> GetQnAAsync(JObject body);
    }
}
