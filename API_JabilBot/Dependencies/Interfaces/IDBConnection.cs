using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace API_JabilBot.Dependencies.Interfaces
{
    interface IDBConnection
    {
        SqlConnection getConnection();

        SqlCommand getCommand(string sql, SqlConnection connection);
    }
}
