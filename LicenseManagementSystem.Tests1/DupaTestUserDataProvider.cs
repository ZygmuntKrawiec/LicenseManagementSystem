using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseManagementSystemBusinessLayer.Code;
using NUnit.Framework;
using System.Data.SqlClient;

namespace LicenseManagementSystem.Tests
{
    [TestFixture]
    public class DupaTestUserDataProvider
    {
        string userName = "DupaEmail6";
        string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = D:\VisualStudioProjects\LicenseManagementSystemPresentationLayer\LicenseManagementSystemBusinessLayer\App_Data\LicenseManagementSystemDatabase.mdf; Integrated Security = True";
        SqlConnection con = new SqlConnection();

        [Test]
        public void TestClassUserDataProvider_Add_NewDupaUser_DupaPassword_Recive_1()
        {
            con.ConnectionString = connectionString;
            UserDataProvider udp = new UserDataProvider(con);
            string newUser = userName + new RandomNumbersGenerator().GetNumber();
            int i = udp.SaveNewUserIntoDatabase(newUser, "DupaPassword");
            Assert.AreEqual(1, i);
        }

        [Test]
        public void TestClassUserDataProvider_Read_DupaUsers_DupaPassword()
        {
            con.ConnectionString = connectionString;
            UserDataProvider udp = new UserDataProvider(con);
            byte[] passwordFromDatabase = udp.ReadUserPasswordFromDatabase(userName);
            HashedPassword hPass = new HashedPassword(passwordFromDatabase);
            Assert.AreEqual(true, hPass.Verify("DupaPassword"));
        }

        [Test]
        public void TestClassUserDataProvider_Read_DupaUsers_WrongDupaPassword()
        {
            con.ConnectionString = connectionString;
            UserDataProvider udp = new UserDataProvider(con);
            byte[] passwordFromDatabase = udp.ReadUserPasswordFromDatabase(userName);
            HashedPassword hPass = new HashedPassword(passwordFromDatabase);
            Assert.AreEqual(false, hPass.Verify("DupaPassword1"));
        }

        [Test]
        public void TestClassUserDataProvider_Read_WrongDupaUsers_WrongDupaPassword()
        {
            con.ConnectionString = connectionString;
            UserDataProvider udp = new UserDataProvider(con);
            byte[] passwordFromDatabase = udp.ReadUserPasswordFromDatabase("Dupa");
            Assert.AreEqual(null, passwordFromDatabase);
        }

        [Test]
        public void TestClassUserDataProvider_SaveUserGuidToDatabase()
        {
            // #Add a new userGuidNumber to the database
            con.ConnectionString = connectionString;
            UserDataProvider udp = new UserDataProvider(con);
            Guid newUserGuid = Guid.NewGuid();
            Guid resultGuid = Guid.Empty;
            string newUser = "NewUser" + new RandomNumbersGenerator().GetNumber();
            bool result = udp.SaveUserGuidToDatabase(newUser, newUserGuid);
            // #Check if userGuidNumber in database is the same as current one.
            string command = $"SELECT UserGuidNumber FROM tblLoggedUsers WHERE UserEmail = '{newUser}'";
            con.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand(command, con);
            // If A new user was added to a database, read his userGuidNumber and compare to current one.
            if (result)
            {
                try
                {
                    using (con)
                    {
                        con.Open();
                        object readResult = cmd.ExecuteScalar();
                        resultGuid = readResult is Guid ? (Guid)readResult : Guid.Empty;
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                Assert.AreEqual(newUserGuid, resultGuid);
            }
        }

        [Test]
        public void TestClassUserDataProvider_RemoveUserGuidFromDatabase()
        {
            con.ConnectionString = connectionString;
            UserDataProvider udp = new UserDataProvider(con);
            Guid newUserGuid = Guid.NewGuid();

            string newUser = "NewUser" + new RandomNumbersGenerator().GetNumber();
            udp.SaveUserGuidToDatabase(newUser, newUserGuid);
            // #Remove user from database
            con.ConnectionString = connectionString;          
            bool result = udp.RemoveUserGuidFromDatabase(newUser, newUserGuid);

            // A new user was removed from a database
            Assert.AreEqual(true, result);
        }

        [Test]
        public void TestClassUserDataProvider_CheckExistenceUserGuidInDatabase()
        {
            con.ConnectionString = connectionString;
            UserDataProvider udp = new UserDataProvider(con);
            Guid newUserGuid = Guid.NewGuid();

            string newUser = "NewUser" + new RandomNumbersGenerator().GetNumber();
            udp.SaveUserGuidToDatabase(newUser, newUserGuid);
            // #Check if userGuidNumber exist in database.
            con.ConnectionString = connectionString;
            bool result = udp.CheckExistenceUserGuidInDatabase(newUser, newUserGuid);
            // #Remove user from database
            con.ConnectionString = connectionString;
           udp.RemoveUserGuidFromDatabase(newUser, newUserGuid);

            // A new user exists in a database
            Assert.AreEqual(true, result);
        }
    }
}

