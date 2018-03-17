using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using LicenseManagementSystemBusinessLayer.Code;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace LicenseManagementSystemBusinessLayer.WebService
{
    /// <summary>
    /// Summary description for LicenseManagementSystemWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LicenseManagementSystemWebService : System.Web.Services.WebService
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DBCS_LMS"].ConnectionString;
        SqlConnection sqlConnectionToDatabase = new SqlConnection();

        /// <summary>
        /// Checks if user is authorized.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [WebMethod]
        public Guid Login(string userEmail, string password)
        {

            sqlConnectionToDatabase.ConnectionString = connectionString;
            UserDataProvider userDataProvider = new UserDataProvider(sqlConnectionToDatabase);
            Authorizer authorizer;
            try
            {
                authorizer = new Authorizer(password, () => { return userDataProvider.ReadUserPasswordFromDatabase(userEmail); });
            }
            catch
            {
                return Guid.Empty;
            }

            // If user is authorized, create a new guid and save it to database and return it to a current user.
            if (authorizer.IsUserAuthorized)
            {
                Guid userloginGuid = Guid.NewGuid();
                sqlConnectionToDatabase.ConnectionString = connectionString;
                userDataProvider.SaveUserGuidToDatabase(userEmail, userloginGuid);
                return userloginGuid;
            }
            else
            {
                // If user is not authorised return an empty guid.
                return Guid.Empty;
            }

        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userEmail">A user's email to save as login.</param>
        /// <param name="userPassword">A user's password </param>
        /// <returns>Returns true if user was registered anf false if not.</returns>
        [WebMethod]
        public bool Registration(string userEmail, string userPassword)
        {
            sqlConnectionToDatabase.ConnectionString = connectionString;
            UserDataProvider userDataProvider = new UserDataProvider(sqlConnectionToDatabase);
            return userDataProvider.SaveNewUserIntoDatabase(userEmail, userPassword) == 1;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public LicensesContainer GetLicensesData(string userEmail, Guid loggedUsersAccessNumber, int pageNumber, int columnToSort, int numberOfRowsToDisplay, bool typeOfSorting)
        {
            sqlConnectionToDatabase.ConnectionString = connectionString;
            UserDataProvider userDataProvider = new UserDataProvider(sqlConnectionToDatabase);

            if (userDataProvider.CheckExistenceUserGuidInDatabase(userEmail, loggedUsersAccessNumber))
            {
                sqlConnectionToDatabase.ConnectionString = connectionString;
                LicensesDataProvider licensesDataProvider = new LicensesDataProvider(sqlConnectionToDatabase);
                Tuple<DataSet,int> queryResult = licensesDataProvider.GetLicensesData(pageNumber, columnToSort, numberOfRowsToDisplay, typeOfSorting);
                LicensesContainer container = new LicensesContainer();
                container.LicensesDataSet = queryResult.Item1;
                container.NumberOfAllLicenses = queryResult.Item2;
                return container;
            }

            return null;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="loggedUsersAccessNumber"></param>
        [WebMethod]
        public void LogoutUser(string userEmail, Guid loggedUsersAccessNumber)
        {
            sqlConnectionToDatabase.ConnectionString = connectionString;
            UserDataProvider userDataProvider = new UserDataProvider(sqlConnectionToDatabase);

            userDataProvider.RemoveUserGuidFromDatabase(userEmail, loggedUsersAccessNumber);
        }

        /// <summary>
        /// TODO:
        /// </summary>
        [WebMethod]
        public bool AddNewLicenseData(string userEmail, Guid loggedUsersAccessNumber, string licenseDataUserName, string licenseDataUserEmail)
        {
            sqlConnectionToDatabase.ConnectionString = connectionString;
            UserDataProvider userDataProvider = new UserDataProvider(sqlConnectionToDatabase);
            if (userDataProvider.CheckExistenceUserGuidInDatabase(userEmail, loggedUsersAccessNumber))
            {
                sqlConnectionToDatabase.ConnectionString = connectionString;
                return new LicensesDataProvider(sqlConnectionToDatabase).AddNewLicense(licenseDataUserName, licenseDataUserEmail);
            }

            return false;
        }
    }

    public class LicensesContainer
    {
        public DataSet LicensesDataSet { get; set; }
        public int NumberOfAllLicenses { get; set; }
    }  
}
