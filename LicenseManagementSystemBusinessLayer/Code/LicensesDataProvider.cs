using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace LicenseManagementSystemBusinessLayer.Code
{
    internal class LicensesDataProvider : DataProvider
    {
        // Public
        public LicensesDataProvider(SqlConnection con) : base(con)
        {
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

        /// <summary>
        /// Adds new license data to database.
        /// </summary>
        /// <param name="LicenseDataUserName">A new license username </param>
        /// <param name="licenseDatauserEmail">A new license useremail</param>
        /// <returns></returns>
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

    }
}