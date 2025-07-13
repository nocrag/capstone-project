using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Service
{
    public static class PasswordUtility
    {

        /// <summary>
        /// Hashes the given input string using SHA-256 with a provided salt.
        /// </summary>
        /// <param name="input">The input string to hash (e.g., a password).</param>
        /// <param name="salt">A byte array used to salt the input before hashing.</param>
        /// <returns>
        /// A SHA-256 hash represented as a lowercase hexadecimal string.
        /// </returns>
        public static string HashToSHA256(string input, byte[] salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] saltedPassword = salt.Concat(Encoding.UTF8.GetBytes(input)).ToArray();
                byte[] hashBytes = sha256.ComputeHash(saltedPassword);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Hashes the given input string using SHA-256 without a salt.
        /// </summary>
        /// <param name="input">The input string to hash.</param>
        /// <returns>
        /// A SHA-256 hash represented as a lowercase hexadecimal string.
        /// </returns>
        public static string HashToSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }


        /// <summary>
        /// Generates a cryptographically secure random salt of the specified length.
        /// </summary>
        /// <param name="length">The length of the salt in bytes. Default is 16.</param>
        /// <returns>A byte array containing the generated salt.</returns>
        public static byte[] GenerateSalt(int length = 16)
        {
            byte[] salt = new byte[length];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
