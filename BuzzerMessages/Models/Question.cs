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
        
        /// <summary>
        /// Creates a new question with the regular
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static Question CreateGameRoomQuestion(int roomId, string roomTitle)
        {
            var question = new Question
            {
                IsActive = true,
                AllowMultipleVotes = false,
                RoomId = roomId,
                Title = $"{roomTitle} - {DateTime.Now.ToString("HH:mm:ss")}",
            };
            question.Responses = CreateDefaultGameRoomQuestionResponses();
            return question;
        }

        private static List<Response> CreateDefaultGameRoomQuestionResponses()
        {
            var responseTitles = new string[] { "#1", "#2", "#3", "#4", "#5", "#6" };
            var responses = new List<Response>(responseTitles.Length);

            for(int i = 0; i < responseTitles.Length; i++)
            {
                var response = new Response
                {
                    Title = responseTitles[i]
                };
                responses.Add(response);
            }

            return responses;
        }

        /*
        /// <summary>
        /// Checks if every the value of every property of this questions matches those of another question.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true if all values match, false if they dont match or <paramref name="obj"/> is of the wrong type</returns>
        public override bool Equals(object obj)
        {
            var other = obj as Question;
            if (other == null)
                return false;

            // Responses must be tested seperately since they are complex objects.
            var responsesMatch = this.Responses.All(other.Responses.Contains) && this.Responses.Count == other.Responses.Count;

            var matching = 
                this.AllowMultipleVotes == other.AllowMultipleVotes && 
                this.DateTime == other.DateTime && 
                this.Id == other.Id && 
                this.IsActive == other.IsActive && 
                this.Room == other.Room && 
                this.RoomId == other.RoomId && 
                this.Timestamp == other.Timestamp && 
                this.Title == other.Title && 
                this.User == other.User && 
                this.UserId == other.UserId;
            return matching;
        }
        */
    }
}
