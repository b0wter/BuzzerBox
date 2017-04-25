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
        private int id;
        public int Id { get { return id; } set { id = value; NotifyPropertyChanged(); } }
        private int roomId;
        /// <summary>
        /// Id of the room this question is asked in.
        /// </summary>
        public int RoomId { get { return roomId; } set { roomId = value; NotifyPropertyChanged(); } }
        /// <summary>
        /// Room to which this question belongs.
        /// </summary>
        [JsonIgnore]
        public Room Room { get; set; }
        private string title;
        /// <summary>
        /// Text of question asked.
        /// </summary>
        public string Title { get { return title; } set { title = value; NotifyPropertyChanged(); } }
        private bool isActive = true;
        /// <summary>
        /// Sets/Gets if the question is active and can be still be answered.
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; NotifyPropertyChanged(); } }
        private long timestamp;
        /// <summary>
        /// Time of the creation of the question. Given as a UNIX epoch (seconds).
        /// </summary>
        public long Timestamp { get { return timestamp; } set { timestamp = value; NotifyPropertyChanged(); } }
        /// <summary>
        /// DateTime representation of <see cref="Timestamp"/>.
        /// </summary>
        [JsonIgnore]
        public DateTime DateTime => Converter.UnixTimeStampToDateTime(Timestamp);
        private List<Response> responses = new List<Response>();
        /// <summary>
        /// List of possible responses for this question.
        /// </summary>
        public List<Response> Responses { get { return responses; } set { responses = value; NotifyPropertyChanged(); } }
        /// <summary>
        /// User that posted this question. Only this user or an admin is allowed to close this question.
        /// </summary>
        [JsonIgnore]
        public User User { get; set; }
        private int userId;
        /// <summary>
        /// Id of the user that posted this question.
        /// </summary>
        public int UserId { get { return userId; } set { userId = value; NotifyPropertyChanged(); } }
        private bool allowMultipleVotes = false;
        /// <summary>
        /// Sets/Gets if multiple votes of a single user are allowed.
        /// </summary>
        public bool AllowMultipleVotes { get { return allowMultipleVotes; } set { allowMultipleVotes = value; NotifyPropertyChanged(); } }
    }
}
