using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_JabilBot.Services.Interfaces.ServiceNow
{
    public interface IGeneralFunctionsService
    {
        Task<Boolean> ValidationServerAsync(string server);
    }
}
