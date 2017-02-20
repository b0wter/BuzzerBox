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
        }
    }
}
