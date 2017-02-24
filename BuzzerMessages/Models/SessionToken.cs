using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuzzerEntities.Models
{
    /// <summary>
    /// Token used to verify that a user logged in succesfully.
    /// </summary>
    public class SessionToken
    {
        /// <summary>
        /// Primary key for the database.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Actual session token. Must be unique.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Id of the user this token belongs to.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Time of the creation of this session token.
        /// </summary>
        public DateTime CreationDate { get; private set; }

        public SessionToken()
        {
            CreationDate = DateTime.Now;
            Token = CreateRandomToken();
        }

        /// <summary>
        /// Creates a random token from upper- and lower letters and numbers.
        /// </summary>
        /// <returns></returns>
        private static string CreateRandomToken()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[16];
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
