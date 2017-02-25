using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerEntities.Models
{
    public class Response
    {
        public int Id { get; set; }
        /// <summary>
        /// Id of the question this is a response for.
        /// </summary>
        public int QuestionId { get; set; }
        /// <summary>
        /// Instance of the question this response belongs to.
        /// </summary>
        public Question Question { get; set; }
        /// <summary>
        /// Display text of the response.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Votes cast for this question.
        /// </summary>
        public List<Vote> Votes { get; set; }
    }
}
