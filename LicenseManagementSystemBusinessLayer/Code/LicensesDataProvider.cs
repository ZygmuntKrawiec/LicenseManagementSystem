﻿using System;
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
            Func<SqlCommand, object> queryToExecute = (cmd) => new SqlDataAdapter(cmd).Fill(resultDataSet);
            
            resultOfQuery(sqlCommand, queryToExecute);          

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
            // TODO: To REFACTOR along with DeleteLicense method.
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "spAddNewLicenseDataToDatabase";
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@UserName", LicenseDataUserName);
            sqlCommand.Parameters.AddWithValue("@UserEmail", licenseDatauserEmail);

            return (int)resultOfQuery(sqlCommand, (cmd) => cmd.ExecuteNonQuery()) == 1 ? true : false;
        }

        public bool DeleteLicense(string LicenseDataUserName, string licenseDatauserEmail)
        {
            // TODO: To REFACTOR along with AddNewLicense method.
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "spDeleteLicenseDataFromDatabase";
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@LicenseUserName", LicenseDataUserName);
            sqlCommand.Parameters.AddWithValue("@LicenseUserEmail", licenseDatauserEmail);

            return (int)resultOfQuery(sqlCommand, (cmd) => cmd.ExecuteNonQuery()) == 1 ? true : false;
        }

        public bool ModifyLicenseData(string newLicenseDataUserName, string newlicenseDatauserEmail, string oldLicenseDataUserName, string oldlicenseDatauserEmail)
        {
            // TODO: To REFACTOR along with AddNewLicense method.
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "spModifyLicenseDataInDatabase";
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@newLicenseUserName", newLicenseDataUserName);
            sqlCommand.Parameters.AddWithValue("@newLicenseUserEmail", newlicenseDatauserEmail);

            sqlCommand.Parameters.AddWithValue("@oldLicenseUserName", oldLicenseDataUserName);
            sqlCommand.Parameters.AddWithValue("@oldLicenseUserEmail", oldlicenseDatauserEmail);

            return (int)resultOfQuery(sqlCommand, (cmd) => cmd.ExecuteNonQuery()) == 1 ? true : false;
        }

    }
}