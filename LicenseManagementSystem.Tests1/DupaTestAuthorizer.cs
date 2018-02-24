using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseManagementSystemBusinessLayer.Code;

namespace LicenseManagementSystem.Tests
{
    [TestFixture]
    public class DupaTestAuthorizer
    {
        [Test]
        public void TestClassAuthorizer_Dupa_Hash_To_Dupa_Password()
        {
            HashedPassword pass = new HashedPassword("Dupa");
            Authorizer autorizer = new Authorizer("Dupa", () => { return pass.ToArray(); });
            Assert.AreEqual(true, autorizer.IsUserAuthorized);
        }

        [Test]
        public void TestClassAuthorizer_Dupa_Hash_To_NotDupa_Password()
        {
            HashedPassword pass = new HashedPassword("Dupa");
            Authorizer autorizer = new Authorizer("NotDupa", () => { return pass.ToArray(); });
            Assert.AreEqual(false, autorizer.IsUserAuthorized);
        }
    }
}
