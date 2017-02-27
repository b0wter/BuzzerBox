using BuzzerEntities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerEntities.Models
{
    public class Question
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
    }
}
