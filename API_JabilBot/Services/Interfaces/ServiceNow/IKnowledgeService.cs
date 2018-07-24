using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_JabilBot.Services.Interfaces
{
    public interface IKnowledgeService
    {
        Task<JObject> GetKnowledgeByNumberAsync(string kb);

        Task<JObject> GetKnowledgesByServerAsync(string description);

        Task<JObject> GetAuthorKnowledgeAsync(string link);

    }
}
