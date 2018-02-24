using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseManagementSystemBusinessLayer.Code;
using System.Data.SqlClient;

namespace LicenseManagementSystemTests.Tests
{
    [TestFixture]
    public class DupaTestPasswordHash
    {
        [Test]
        public void TestClassPasswordHash_Dupa_Hash_To_Dupa_Password()
        {
            HashedPassword pass = new HashedPassword("Dupa");
            Assert.AreEqual(true, pass.Verify("Dupa"));
        }

        [Test]
        public void TestClassPasswordHash_Dupa_Hash_To_NotDupa_Password()
        {
            HashedPassword pass = new HashedPassword("Dupa");
            Assert.AreEqual(false, pass.Verify("NotDupa"));
        }

        [Test]
        public void TestClassPasswordHash_Dupa_Hash_To_Null()
        {
            HashedPassword pass = new HashedPassword("Dupa");
            Assert.AreEqual(false, pass.Verify(null));
        }

        [Test]
        public void TestClassPasswordHash_Dupa_Hash_To_EmptyString()
        {
            HashedPassword pass = new HashedPassword("Dupa");
            Assert.AreEqual(false, pass.Verify(""));
        }

        [Test]
        public void TestClassPasswordHash_EmptyString_Hash_To_Dupa()
        {
            HashedPassword pass = new HashedPassword("");
            Assert.AreEqual(false, pass.Verify("Dupa"));
        }

    }
}
