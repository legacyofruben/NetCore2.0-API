using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_JabilBot.Models
{
    public class Employee
    {
        public long id { get; set; }
        public long PkUser { get; set; }
        public int FKSite { get; set; }
        public string EmployeeNumber { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string EMail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public int FKCostCenter { get; set; }
        public int FKPosition { get; set; }
    }
}
