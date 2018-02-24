using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

#if DEBUG
[assembly: InternalsVisibleTo("LicenseManagementSystem.Tests")]
#endif
namespace LicenseManagementSystemBusinessLayer.Code
{
    /// <summary>
    /// Represents a hashed password.
    /// </summary>
    internal sealed class HashedPassword
    {
        const int SALTSIZE = 16, HASHSIZE = 20, HASHITER = 1000;

        // Fields contains salt and a hash of the given password
        readonly byte[] salt, hash;

        /// <summary>
        /// Initializes a new instance of PasswordHash class that contains a hashed password and a salt.
        /// </summary>
        /// <param name="password">A password to hashed</param>
        public HashedPassword(string password)
        {
            if (password == null)
                throw new Exception("A parameter cannot be null.");
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SALTSIZE]);
            hash = new Rfc2898DeriveBytes(password, salt, HASHITER).GetBytes(HASHSIZE);
        }

        /// <summary>
        /// Initializes a new instance of PasswordHash class that reads a hash and a salt from byte[] array.
        /// </summary>
        /// <param name="hashBytes">An array that contains a hashed password and a salt.</param>
        public HashedPassword(byte[] hashBytes)
        {
            if (hashBytes == null)
                throw new Exception("A parameter cannot be null.");
            Array.Copy(hashBytes, 0, salt = new byte[SALTSIZE], 0, SALTSIZE);
            Array.Copy(hashBytes, SALTSIZE, hash = new byte[HASHSIZE], 0, HASHSIZE);
        }

        /// <summary>
        /// Returns a hashed password as a byte array.
        /// </summary>
        /// <returns>An byte array with a hashed password.</returns>
        public byte[] ToArray()
        {
            byte[] hashBytes = new byte[SALTSIZE + HASHSIZE];
            Array.Copy(salt, 0, hashBytes, 0, SALTSIZE);
            Array.Copy(hash, 0, hashBytes, SALTSIZE, HASHSIZE);
            return hashBytes;
        }

        /// <summary>
        /// Returns a hashed password as a string contains separated bytes by a delivered separator.
        /// </summary>
        /// <param name="byteSeparator">A separator to separate bytes in password.</param>
        /// <returns>A string with hashed password.</returns>
        public string ToString(string byteSeparator)
        {
            string passwordHashString = String.Join(byteSeparator, this.ToArray());
            passwordHashString = passwordHashString.Insert(0, byteSeparator);
            return passwordHashString;
        }

        /// <summary>
        /// Verifies a given password if its the same as a hashed password kept by instance of PasswordHash class. 
        /// </summary>
        /// <param name="password">A password to check.</param>
        /// <returns>Returns true if a given password is te same as a hashed password or false if not.</returns>
        public bool Verify(string password)
        {
            string passwordToCheck = password ?? "";
            byte[] passwordToVerify = new Rfc2898DeriveBytes(passwordToCheck, salt, HASHITER).GetBytes(HASHSIZE);
            for (int i = 0; i < HASHSIZE; i++)
            {
                if (hash[i] != passwordToVerify[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Removes all separators from a hashed password and converts it into a byte array.
        /// </summary>
        /// <param name="hashedPasswordFromDatabase">A hashed password with separators from a database.</param>
        /// <returns>A hashed password.</returns>
        public static byte[] RemoveSeparators(string hashedPasswordFromDatabase)
        {
            //TODO: Throws an exception if password is in wrong form.
#if (!DEBUG)
            throw new Exception("The problem to fix in HashedPassword class in RemoveSeparators method");
            // If a password does not contain separators or it is in a wrong form it throws an exception.
            // Posible solution: during creation a password count all chars and put the number of chars in the password.
            // To check if a password has a proper number of chars, method reads number of chars and compare it to the number of chars in the password.
#endif
            if (hashedPasswordFromDatabase != null && hashedPasswordFromDatabase != "")
            {
                char separator = hashedPasswordFromDatabase[0];
                string[] stringArray = hashedPasswordFromDatabase.Split(separator).Skip(1).ToArray();
                return stringArray.Select(byte.Parse).ToArray(); 
            }
            return null;
        }
    }
}