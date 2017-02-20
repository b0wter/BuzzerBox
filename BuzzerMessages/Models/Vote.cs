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
        /// Id of the question this vote is for.
        /// </summary>
        public int QuestionId { get; set; }
        /// <summary>
        /// Id of the response this vote is for.
        /// </summary>
        public int ResponseId { get; set; }
        /// <summary>
        /// Id of the user that voted.
        /// </summary>
        public int UserId { get; set; }
    }
}
