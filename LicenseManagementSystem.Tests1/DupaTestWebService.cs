using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseManagementSystemBusinessLayer.WebService;

namespace LicenseManagementSystem.Tests
{
    [TestFixture]
    public class DupaTestWebService
    {
        [Test]
        public void TestWSLogin_CorrectPassword_DupaEmail4_CorrectDupaPassword()
        {
            LMSWebService.LicenseManagementSystemWebServiceSoapClient client = new LMSWebService.LicenseManagementSystemWebServiceSoapClient();
            bool result = client.Login("DupaEmail4", "DupaPassword");
            Assert.AreEqual(true, result); 
        }

        [Test]
        public void TestWSLogin_IncorrectPassword_DupaEmail_DupaPassword()
        {
            LMSWebService.LicenseManagementSystemWebServiceSoapClient client = new LMSWebService.LicenseManagementSystemWebServiceSoapClient();
            bool result = client.Login("DupaEmail", "DupaPassword");
            Assert.AreEqual(false, result);
        }

        [Test]
        public void TestWSLogin_IncorrectUserEmail_DupaEmailIncorrect()
        {
            LMSWebService.LicenseManagementSystemWebServiceSoapClient client = new LMSWebService.LicenseManagementSystemWebServiceSoapClient();
            bool result = client.Login("DupaEmailIncorrect", "DupaPassword");
            Assert.AreEqual(false, result);
        }

        [Test]
        public void TestWSRegistration_AddNewUser_CorrectLoginAndPassword()
        {
            LMSWebService.LicenseManagementSystemWebServiceSoapClient client = new LMSWebService.LicenseManagementSystemWebServiceSoapClient();
            RandomNumbersGenerator rng = new RandomNumbersGenerator();
            bool result = client.Registration("DupaEmail7"+rng.GetNumber(), "DupaPassword");
            Assert.AreEqual(true, result);
        }

        [Test]
        public void TestWSRegistration_AddExistingUser_IncorrectLoginAndCorrectPassword()
        {
            LMSWebService.LicenseManagementSystemWebServiceSoapClient client = new LMSWebService.LicenseManagementSystemWebServiceSoapClient();
            bool result = client.Registration("DupaEmail7", "DupaPassword");
            Assert.AreEqual(false, result);
        }
    }
}
