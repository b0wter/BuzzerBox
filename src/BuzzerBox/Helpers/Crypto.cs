using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace BuzzerBox.Helpers
{
    public static class Crypto
    {
        private const int HASH_ITERATION_COUNT = 10000;

        /// <summary>
        /// Creates a 256 Bit hash using Pbkdf2 with 10000 iterations of HMACSHA1.
        /// </summary>
        /// <param name="password">Password that will be hashed.</param>
        /// <param name="salt">User-specific salt to harden the password.</param>
        /// <returns></returns>
        internal static string CreatePasswordHash(string password, byte[] salt)
        {
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA1, HASH_ITERATION_COUNT, 256 / 8));
            return hash;
        }

        /// <summary>
        /// Creates a 256 Bit hash using Pbkdf2 with 10000 iterations of HMACSHA1.
        /// </summary>
        /// <param name="password">Password that will be hashed.</param>
        /// <param name="salt">Base64 encoded string of the salt.</param>
        /// <returns></returns>
        internal static string CreatePasswordHash(string password, string salt)
        {
            byte[] rawSalt = Convert.FromBase64String(salt);
            return CreatePasswordHash(password, rawSalt);
        }

        /// <summary>
        /// Creates a 128 bit salt to create password hashes.
        /// </summary>
        /// <returns></returns>
        internal static byte[] CreateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
