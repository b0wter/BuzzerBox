using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerEntities.Models
{
    public enum UserLevels
    {
        Guest = 0,
        Default = 1,
        Admin = 2
    }

    /// <summary>
    /// A representation of the <see cref="User"/> that doesnt contain any sensitive information.
    /// </summary>
    public class FilteredUser : BaseModel
    {
        /// <summary>
        /// Primary key for the database.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Username of the user. Used for display and logging in.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Priviledge level of the user.
        /// </summary>
        public UserLevels Level { get; set; }
        /// <summary>
        /// Instances of all the votes cast by this user.
        /// </summary>
        [JsonIgnore]
        public List<Vote> Votes { get; set; }
        /// <summary>
        /// List of questions posted by this user.
        /// </summary>
        [JsonIgnore]
        public List<Question> Questions { get; set; }

        /// <summary>
        /// Creates an instance of this user that does not include any sensitive information.
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static FilteredUser FromUser(User u)
        {
            return new FilteredUser
            {
                Id = u.Id,
                Name = u.Name,
                Level = u.Level
            };
        }
    }

    /// <summary>
    /// Full representation of a user.
    /// </summary>
    public class User : FilteredUser
    {
        /// <summary>
        /// List of active tokens for the given user. Multiple tokens are required for multi-device support.
        /// </summary>
        [JsonIgnore]
        public List<SessionToken> SessionToken { get; set; }
        /// <summary>
        /// Hash of the password. Passwords are derived using Pbkdf2 and HMACSHA256 with 10,000 iterations.
        /// </summary>
        public string PasswordHash { get; set; }
        /// <summary>
        /// Token that was used for the registration.
        /// </summary>
        public string RegistrationToken { get; set; }
        /// <summary>
        /// Salt used to harden the password hash (Base64 encoded).
        /// </summary>
        public string Salt { get; set; }
    }
}
