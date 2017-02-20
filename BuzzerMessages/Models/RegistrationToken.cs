using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuzzerEntities.Models
{
    /// <summary>
    /// A one-time token that is used to create an account.
    /// </summary>
    public class RegistrationToken
    {
        /// <summary>
        /// Primary key for the database.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The actual token. Is made up of letters and numbers.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Time of the creation of this token.
        /// </summary>
        public DateTime CreationTime { get; private set; }
        /// <summary>
        /// Get/sets of this token has been used. A token can only be used for a single registration.
        /// </summary>
        public bool WasUsed { get; set; }

        /// <summary>
        /// Generates a new random token. The token consists of upper- and lower case letters, as well as numbers.
        /// </summary>
        public RegistrationToken()
        {
            CreationTime = DateTime.Now;
            Token = CreateRandomToken();
        }

        /// <summary>
        /// Creates a random token from upper- and lower letters and numbers.
        /// </summary>
        /// <returns></returns>
        private string CreateRandomToken()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
    }
}
