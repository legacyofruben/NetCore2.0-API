using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API_JabilBot.Services.Interfaces
{
    interface IHttpClient
    {
        HttpClient Create(string endpoint);
    }
}
