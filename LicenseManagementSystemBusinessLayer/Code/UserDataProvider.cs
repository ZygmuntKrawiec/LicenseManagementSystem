using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LicenseManagementSystemBusinessLayer.Code
{
    /// <summary>
    /// Represents a user's data provider which reads user's password from a database, 
    /// save a new user into database and saves or deletes users guid number.  
    /// </summary>
    internal class UserDataProvider : DataProvider
    {
        /// <summary>
        /// Initializes a new instance of UserDataProvider class that contains a connection to database.
        /// </summary>
        /// <param name="sqlDatabaseConnection">A connection to database.</param>
        public UserDataProvider(SqlConnection sqlDatabaseConnection) : base(sqlDatabaseConnection)
        {
        }

        /// <summary>
        /// Reads a password from a database.
        /// </summary>
        /// <param name="userEmail">A useremail/login for which a password is read.</param>
        /// <returns>Returns a password read from a database.</returns>
        public byte[] ReadUserPasswordFromDatabase(string userEmail)
        {
            SqlCommand command = new SqlCommand($"SELECT [UserPassword] FROM [dbo].[tblUsersWithAccessToLicensesData] WHERE [UserEmail]=@UserEmail");
            command.Parameters.AddWithValue("@UserEmail", userEmail);
            object result = resultOfQuery(command, (cmd) => cmd.ExecuteScalar());
            return result is string ? HashedPassword.RemoveSeparators((string)result) : null;
        }

        /// <summary>
        /// Saves a new user into database.
        /// </summary>
        /// <param name="userEmail">A users email/login.</param>
        /// <param name="userPassword">A users password.</param>
        /// <returns>Returns 1 if user is saved correctly.</returns>
        public int SaveNewUserIntoDatabase(string userEmail, string userPassword)
        {
            HashedPassword userHashedPassword = new HashedPassword(userPassword);
            string password = userHashedPassword.ToString("|");
            string command =
                $@"IF NOT EXISTS (SELECT [UserEmail] FROM [dbo].[tblUsersWithAccessToLicensesData] WHERE [UserEmail]=@UserEmail)
                        BEGIN
                            INSERT INTO [dbo].[tblUsersWithAccessToLicensesData] VALUES (@UserEmail, @Password)                            
                        END";

            SqlCommand sqlCommand = new SqlCommand(command);
            sqlCommand.Parameters.AddWithValue("@UserEmail", userEmail);
            sqlCommand.Parameters.AddWithValue("@Password", userPassword);
            return (int)resultOfQuery(sqlCommand, (cmd) => cmd.ExecuteNonQuery());
        }

        /// <summary>
        /// Saves a logged user's userEmail and userGuidNumber into database.
        /// </summary>
        /// <param name="userEmail">A users email/login.</param>
        /// <param name="userLoginGuid">A users Guid.</param>
        /// <returns>Returns true if use's data was saved correctly.</returns>
        public bool SaveUserGuidToDatabase(string userEmail, Guid userLoginGuid)
        {
            object result = checkSaveOrRemoveUserGuidNumber(userEmail, userLoginGuid, "spSaveUserGuidToDatabase");

            return result is int && (int)result == 1 ? true : false;
        }

        /// <summary>
        /// Removes a logout user's userEmail and userGuidNumber from database.
        /// </summary>
        /// <param name="userEmail">A users email/login.</param>
        /// <param name="userLoginGuid">A users Guid.</param>
        /// <returns>Returns true if use's data was removed correctly.</returns>
        public bool RemoveUserGuidFromDatabase(string userEmail, Guid userLoginGuid)
        {
            object result = checkSaveOrRemoveUserGuidNumber(userEmail, userLoginGuid, "spRemoveUserGuidFromDatabase");
            return result is int && (int)result == 1 ? true : false;
        }

        /// <summary>
        /// Checks if user's guid number exists in a database.
        /// </summary>
        /// <param name="userEmail">A users email/login.</param>
        /// <param name="userLoginGuid">A users Guid.</param>
        /// <returns> Returns true if user's guid number exists in a database.</returns>
        public bool CheckExistenceUserGuidInDatabase(string userEmail, Guid userLoginGuid)
        {
            object result = checkSaveOrRemoveUserGuidNumber(userEmail, userLoginGuid, "spCheckExistenceUserGuidInDatabase");
            return result is int && (int)result == 1 ? true : false;
        }

        /// <summary>
        /// Prepares and executes a SqlCommand to check, save or remove user's data from database.
        /// </summary>
        /// <param name="userEmail">A users email/login.</param>
        /// <param name="userloginGuid">A users Guid.</param>
        /// <param name="storedProcedure">A name ofstored procedure to execute.</param>
        /// <returns>A value returned by an executed stored procedure. </returns>
        private object checkSaveOrRemoveUserGuidNumber(string userEmail, Guid userloginGuid, string storedProcedure)
        {
            SqlCommand command = new SqlCommand(storedProcedure);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserEmail", userEmail);
            command.Parameters.AddWithValue("@UserGuidNumber", userloginGuid);

            return resultOfQuery(command, (cmd) => cmd.ExecuteScalar());
        }
    }
}