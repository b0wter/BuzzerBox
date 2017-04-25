using BuzzerEntities.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerEntities.Models
{
    /// <summary>
    /// Model of a regular room. Any user that has at least the <see cref="UserLevels.Default"/> level can post questions.
    /// </summary>
    public class Room : BaseModel
    {
        private int id;
        /// <summary>
        /// Primary key for the database.
        /// </summary>
        public int Id { get { return id; } set { id = value; NotifyPropertyChanged(); } }
        private string title;
        /// <summary>
        /// Name of the room. Is displayed in the clients.
        /// </summary>
        public string Title { get { return title; } set { title = value; NotifyPropertyChanged(); } }
        private string description;
        /// <summary>
        /// Description of the room.
        /// </summary>
        public string Description { get { return description; } set { description = value; NotifyPropertyChanged(); } }
        private long timestamp;
        /// <summary>
        /// Timestamp of the creation of this room. Stored as an utc unix timestamp (seconds).
        /// </summary>
        public long Timestamp { get { return timestamp; } set { timestamp = value; NotifyPropertyChanged(); } }
        /// <summary>
        /// Returns the timestamp as an instance of <see cref="DateTime"/>.
        /// </summary>
        [JsonIgnore]
        public DateTime ToDateTime => Converter.UnixTimeStampToDateTime(Timestamp);
        private List<Question> questions = new List<Question>();
        /// <summary>
        /// List of all questions posted to this room.
        /// </summary>
        public List<Question> Questions { get { return questions; } set { questions = value; NotifyPropertyChanged(); } }
        /// <summary>
        /// Returns the <see cref="Title"/> of this room.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Title;
        }
    }
}
