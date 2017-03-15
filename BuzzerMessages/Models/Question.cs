using BuzzerEntities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerEntities.Models
{
    public class Question : BaseModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Id of the room this question is asked in.
        /// </summary>
        public int RoomId { get; set; }
        /// <summary>
        /// Room to which this question belongs.
        /// </summary>
        [JsonIgnore]
        public Room Room { get; set; }
        /// <summary>
        /// Text of question asked.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Sets/Gets if the question is active and can be still be answered.
        /// </summary>
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// Time of the creation of the question. Given as a UNIX epoch (seconds).
        /// </summary>
        public long Timestamp { get; set; }
        /// <summary>
        /// DateTime representation of <see cref="Timestamp"/>.
        /// </summary>
        [JsonIgnore]
        public DateTime DateTime => Converter.UnixTimeStampToDateTime(Timestamp);
        /// <summary>
        /// List of possible responses for this question.
        /// </summary>
        public List<Response> Responses { get; set; } = new List<Response>();
        /// <summary>
        /// User that posted this question. Only this user or an admin is allowed to close this question.
        /// </summary>
        [JsonIgnore]
        public User User { get; set; }
        /// <summary>
        /// Id of the user that posted this question.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Sets/Gets if multiple votes of a single user are allowed.
        /// </summary>
        public bool AllowMultipleVotes { get; set; }
    }
}
