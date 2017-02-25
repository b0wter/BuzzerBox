using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerEntities.Models
{
    /// <summary>
    /// Represents a vote that a client has taken for a question.
    /// </summary>
    public class Vote
    {
        public int Id { get; set; }
        /// <summary>
        /// Instance of the response that this vote belongs to.
        /// </summary>
        public Response Response { get; set; }
        /// <summary>
        /// Id of the response this vote is for.
        /// </summary>
        public int ResponseId { get; set; }
        /// <summary>
        /// Id of the user that voted.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Instance of the user this vote belongs to.
        /// </summary>
        public User User { get; set; }
    }
}
