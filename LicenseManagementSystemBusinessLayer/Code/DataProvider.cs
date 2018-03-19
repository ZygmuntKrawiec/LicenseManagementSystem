using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LicenseManagementSystemBusinessLayer.Code
{
    internal abstract class DataProvider
    {
        protected SqlConnection sqlConnection;

        public DataProvider(SqlConnection con)
        {
            sqlConnection = con;
        }

        /// <summary>
        /// Executes a sqlcommand instance.
        /// </summary>
        /// <param name="cmd">A command to execute.</param>
        /// <param name="executeQuery">A provides a method to execute a command to recive a proper result. </param>
        /// <returns>Returns a result of the sqlCommand executions.</returns>
        public object resultOfQuery(SqlCommand cmd, Func<SqlCommand, object> executeQuery)
        {
            try
            {
                using (sqlConnection)
                {
                    cmd.Connection = sqlConnection;
                    sqlConnection.Open();
                    return executeQuery(cmd);
                }
            }// TODO: To Implement better exception handling.
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (ConfigurationErrorsException ex)
            {
                throw ex;
            }
        }
    }
}