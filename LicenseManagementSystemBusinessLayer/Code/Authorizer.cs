using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LicenseManagementSystemBusinessLayer.Code
{
    /// <summary>
    /// Represents a authorizer which checks if hashes of two paswords are the same.
    /// </summary>
    public class Authorizer
    {
        // Keeps a password given by user to check.
        private string userPasswordToCheck;
        // Returns a byte array to compare to password given by user.
        private Func<byte[]> getUserHashedPasswordFromStorage;

        // Returns true if user is autorized.
        public bool IsUserAuthorized { get; private set; }
               

        /// <summary>
        /// Initializes a new instance of the Authorizer class that contains a password to check and
        /// a password copied form a storage to compare. 
        /// </summary>
        /// <param name="userPasswordToCheck">A password to check</param>
        /// <param name="userPasswordFromStorage">A password form storage to compare</param>
        public Authorizer(string userPasswordToCheck, Func<byte[]> userPasswordFromStorage)
        {
            this.userPasswordToCheck = userPasswordToCheck;
            getUserHashedPasswordFromStorage = userPasswordFromStorage;
            IsUserAuthorized = checkAuthorization();
        }

        /// <summary>
        /// Checks if a password given by user is the same as keeped in a storage.
        /// </summary>
        /// <returns>Returns true if passwords are the same and false if not.</returns>
        private bool checkAuthorization()
        {
            HashedPassword passHash = null;
            byte[] HashedPasswordFromStorage = this?.getUserHashedPasswordFromStorage();
            if (HashedPasswordFromStorage != null)
            {
                passHash = new HashedPassword(HashedPasswordFromStorage);               
                return passHash.Verify(userPasswordToCheck);
            }

            return false;
        }
    }
}