using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace LicenseManagementSystemBusinessLayer.Code
{
    internal class LicensesDataProvider
    {
        SqlConnection sqlConnection;

        // Public
        public LicensesDataProvider(SqlConnection con)
        {
            sqlConnection = con;
        }

        public Tuple<DataSet, int> GetLicensesData(int pageNumber, int columnToSort, int numberOfRowsToDisplay, bool typeOfSorting)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "spGetPagedLicenseData";
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@page", pageNumber);
            sqlCommand.Parameters.AddWithValue("@sortColumn", columnToSort);
            sqlCommand.Parameters.AddWithValue("@rows", numberOfRowsToDisplay);
            sqlCommand.Parameters.AddWithValue("@sortDirection", typeOfSorting);

            sqlCommand.Parameters.Add("@maximumNumberOfRows", SqlDbType.Int, 4);
            sqlCommand.Parameters["@maximumNumberOfRows"].Direction = ParameterDirection.Output;

            DataSet resultDataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            // Exception handling to REFACTOR!

            try
            {
                using (sqlConnection)
                {
                    sqlConnection.Open();
                    sqlDataAdapter.Fill(resultDataSet);
                }
            }
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

            int numberOfAllRows = (int)sqlCommand.Parameters["@maximumNumberOfRows"].Value;
            return new Tuple<DataSet, int>(resultDataSet, numberOfAllRows);
        }

        public bool AddNewLicense(string LicenseDataUserName, string licenseDatauserEmail)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "spAddNewLicenseDataToDatabase";
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@UserName", LicenseDataUserName);
            sqlCommand.Parameters.AddWithValue("@UserEmail", licenseDatauserEmail);

            return (int)resultOfQuery(sqlCommand, (cmd) => cmd.ExecuteNonQuery()) == 1 ? true : false;
        }

        // Private

        /// <summary>
        /// Executes a sqlcommand instance.
        /// </summary>
        /// <param name="cmd">A command to execute.</param>
        /// <param name="executeQuery">A provides a method to execute a command to recive a proper result. </param>
        /// <returns>Returns a result of the sqlCommand executions.</returns>
        private object resultOfQuery(SqlCommand cmd, Func<SqlCommand, object> executeQuery)
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