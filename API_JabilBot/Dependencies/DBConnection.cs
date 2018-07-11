using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using API_JabilBot.Dependencies.Interfaces;

namespace API_JabilBot.Dependencies
{
    public class DBConnection : IDBConnection
    {

        private string connetionString = @"Data Source=MXGDLD0SQLTST01\GDLDEVSQL2014;Initial Catalog=EmployeesMainDB;Integrated Security=True; User Id=JABIL\SVCGDL_WEBAPPS; Password=Th3p@55w0rdt0d@y";

        public SqlCommand getCommand(string sql, SqlConnection connection)
        {
            return new SqlCommand(sql, connection);
        }

        public SqlConnection getConnection()
        {
            return new SqlConnection(connetionString);
        }
    }
}
