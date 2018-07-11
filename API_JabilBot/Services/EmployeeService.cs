using API_JabilBot.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace API_JabilBot.Services
{
    public class EmployeeService : IEmployeeService
    {

        public static string DataTableToJSON(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        public DataTable GetEmployeeByEmail(string email)
        {
            string result = "";
            DataTable dataTable = new DataTable();
            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            //SqlDataReader dataReader;
            connetionString = @"Data Source=MXGDLD0SQLTST01\GDLDEVSQL2014;Initial Catalog=EmployeesMainDB;Integrated Security=True; User Id=JABIL\SVCGDL_WEBAPPS; Password=Th3p@55w0rdt0d@y";
            sql = @"SELECT U.PKUser, U.FKSite, U.EmployeeNumber, U.UserName, U.FullName, U.EMail,
	                [U].[FirstName], [U].[LastName], [U].[SecondLastName], [U].[FKCostCenter], [U].[FKPosition]
                    FROM vw_Users U (NOLOCK)
                    WHERE [U].[EMail] = '" + email.Trim() + "'";
            connection = new SqlConnection(connetionString);
            try
            {
                command = new SqlCommand(sql, connection);
                // create data adapter
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dataTable);
                da.Dispose();
                result = DataTableToJSON(dataTable);
            }
            catch (Exception ex)
            {
                result = "Error: " + ex.Message;
            }

            return dataTable;
            //return result;
        }

        public DataTable GetEmployeeByUserName(string username)
        {
            string result = "";
            DataTable dataTable = new DataTable();
            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            //SqlDataReader dataReader;
            connetionString = @"Data Source=MXGDLD0SQLTST01\GDLDEVSQL2014;Initial Catalog=EmployeesMainDB;Integrated Security=True; User Id=JABIL\SVCGDL_WEBAPPS; Password=Th3p@55w0rdt0d@y";
            sql = @"SELECT U.PKUser, U.FKSite, U.EmployeeNumber, U.UserName, U.FullName, U.EMail,
	                [U].[FirstName], [U].[LastName], [U].[SecondLastName], [U].[FKCostCenter], [U].[FKPosition]
                    FROM vw_Users U (NOLOCK)
                    WHERE [U].[EmployeeNumber] = '" + username.Trim() + "'";
            connection = new SqlConnection(connetionString);
            try
            {
                command = new SqlCommand(sql, connection);
                // create data adapter
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dataTable);
                da.Dispose();
                result = DataTableToJSON(dataTable);
            }
            catch (Exception ex)
            {
                result = "Error: " + ex.Message;
            }

            return dataTable;
            //return result;
        }

        public DataTable GetEmployeesBySite(string site)
        {
            string result = "";
            DataTable dataTable = new DataTable();
            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            //SqlDataReader dataReader;
            connetionString = @"Data Source=MXGDLD0SQLTST01\GDLDEVSQL2014;Initial Catalog=EmployeesMainDB;Integrated Security=True; User Id=JABIL\SVCGDL_WEBAPPS; Password=Th3p@55w0rdt0d@y";
            sql = @"SELECT U.PKUser, U.FKSite, U.EmployeeNumber, U.UserName, U.FullName, U.EMail,
	                [U].[FirstName], [U].[LastName], [U].[SecondLastName], [U].[FKCostCenter], [U].[FKPosition]
                    FROM vw_Users U (NOLOCK)
                    WHERE [U].[FKSite] = (SELECT [CS].[PKSite] 
                                          FROM [dbo].[CT_Sites] AS [CS] WITH ( NOLOCK )
						                  WHERE [CS].[Site] = '" + site.Trim() + @"') AND [U].[FirstName] != '' 

                    ORDER BY[U].[FirstName] ASC";
            connection = new SqlConnection(connetionString);
            try
            {
                command = new SqlCommand(sql, connection);
                // create data adapter
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dataTable);
                da.Dispose();
                result = DataTableToJSON(dataTable);
            }
            catch (Exception ex)
            {
                result = "Error: " + ex.Message;
            }

            return dataTable;
            //return result;
        }
    }
}
