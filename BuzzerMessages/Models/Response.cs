using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerEntities.Models
{
    public class Response : BaseModel
    {
        private int id;
        public int Id { get { return id; } set { id = value; NotifyPropertyChanged(); } }
        private int questionId;
        /// <summary>
        /// Id of the question this is a response for.
        /// </summary>
        public int QuestionId { get { return questionId; } set { questionId = value; NotifyPropertyChanged(); } }
        /// <summary>
        /// Instance of the question this response belongs to.
        /// </summary>
        [JsonIgnore]
        public Question Question { get; set; }
        private string title;
        /// <summary>
        /// Display text of the response.
        /// </summary>
        public string Title { get { return title; } set { title = value; NotifyPropertyChanged(); } }
        private List<Vote> votes;
        /// <summary>
        /// Votes cast for this question.
        /// </summary>
        public List<Vote> Votes { get { return votes; } set { votes = value; NotifyPropertyChanged(); } }
        /// <summary>
        /// Amount of votes cast for this response.
        /// </summary>
        public int VoteCount => Votes == null ? 0 : Votes.Count;
    }
}
