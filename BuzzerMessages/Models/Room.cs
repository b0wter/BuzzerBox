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
    public class Room
    {
        /// <summary>
        /// Primary key for the database.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of the room. Is displayed in the clients.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Description of the room.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Timestamp of the creation of this room. Stored as an utc unix timestamp (seconds).
        /// </summary>
        public long Timestamp { get; set; }
        /// <summary>
        /// Returns the timestamp as an instance of <see cref="DateTime"/>.
        /// </summary>
        [JsonIgnore]
        public DateTime ToDateTime => Converter.UnixTimeStampToDateTime(Timestamp);
        /// <summary>
        /// List of all questions posted to this room.
        /// </summary>
        public List<Question> Questions { get; set; }
    }
}
