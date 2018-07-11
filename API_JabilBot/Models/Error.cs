using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_JabilBot.Models
{
    public class Error
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public string Method { get; set; }
        public string StatusCode { get; set; }
    }
}
