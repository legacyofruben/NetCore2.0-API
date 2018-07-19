using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_JabilBot.Models
{
    public class AppSettings
    {
        public string Environment { get; set; }
        public string ServiceNow_BaseUrl { get; set; }
        public string UserSN { get; set; }
        public string PassSN { get; set; }
        public string QnAEndpointHostName { get; set; }
        public string QnAKnowledgebaseId { get; set; }
        public string QnAAuthKey { get; set; }

    }
}
