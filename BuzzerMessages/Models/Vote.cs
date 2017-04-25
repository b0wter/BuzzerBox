using BuzzerEntities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerEntities.Models
{
    /// <summary>
    /// Represents a vote that a client has taken for a question.
    /// </summary>
    public class Vote : BaseModel
    {
        private int id;
        public int Id { get { return id; } set { id = value; NotifyPropertyChanged(); } }
        /// <summary>
        /// Instance of the response that this vote belongs to.
        /// </summary>
        [JsonIgnore]
        public Response Response { get; set; }
        private int responseId;
        /// <summary>
        /// Id of the response this vote is for.
        /// </summary>
        public int ResponseId { get { return responseId; } set { responseId = value; NotifyPropertyChanged(); } }
        private int userId;
        /// <summary>
        /// Id of the user that voted.
        /// </summary>
        public int UserId { get { return userId; } set { userId = value; NotifyPropertyChanged(); } }
        /// <summary>
        /// Instance of the user this vote belongs to.
        /// </summary>
        [JsonIgnore]
        public User User { get; set; }
        private long timestamp;
        /// <summary>
        /// Time this vote was cast. Is stored as seconds-based utc epoch.
        /// </summary>
        public long Timestamp { get { return timestamp; } set { timestamp = value; NotifyPropertyChanged(); } }
        /// <summary>
        /// Returns the timestamp as an instance of <see cref="DateTime"/>.
        /// </summary>
        [JsonIgnore]
        public DateTime ToDateTime => Converter.UnixTimeStampToDateTime(Timestamp);
    }
}
