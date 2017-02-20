using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BuzzerBox.Data;
using BuzzerEntities.Models;
using Newtonsoft.Json;
using BuzzerEntities.Messages.ClientMessages;
using BuzzerBox.Helpers.Exceptions;
using System.Dynamic;
using BuzzerBox.Helpers;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BuzzerBox.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly BuzzerContext context;

        public UsersController(BuzzerContext context)
        {
            this.context = context;
        }

        // GET: api/users
        [HttpGet]
        public JsonResult Get()
        {
            var filteredUsers = FilterSensitiveInformation(context.Users.ToList());
            return new JsonResult(filteredUsers);
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/users
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPost("create")]
        public JsonResult CreateUser([FromBody] string value)
        {
            try
            {
                var requestRegistrationMessage = JsonConvert.DeserializeObject<RequestRegistrationMessage>(value);
                return HandleUserRegistration(requestRegistrationMessage);
            }
            catch(Exception ex)
            {
                return new JsonResult(CreateErrorResult(ex));
            }
        }

        /// <summary>
        /// Handles the process of creating a new user.
        /// </summary>
        /// <param name="message"></param>
        private void HandleUserRegistration(RequestRegistrationMessage message)
        {
            ValidateRegistrationToken(message.RegistrationToken);
            ValidateRegistrationUsername(message.UserName);
            ValidateRegistrationPassword(message.Password);
            var user = CreateNewUser(message.UserName, message.Password);
            SaveUserToDataBase(user);
        }

        /// <summary>
        /// Validates that the registration token is know and has not been used.
        /// </summary>
        /// <param name="token"></param>
        private void ValidateRegistrationToken(string token)
        {
            if (context.RegistrationTokens.Any(x => x.Token == token && x.WasUsed == false) == false)
                throw new InvalidRegistrationTokenException();
        }

        /// <summary>
        /// Verifies that the requested username is not already in use.
        /// </summary>
        /// <param name="username"></param>
        private void ValidateRegistrationUsername(string username)
        {
            if (context.Users.Any(x => x.Name == username))
                throw new UsernameAlreadyInUseException();
        }

        /// <summary>
        /// Verifies that the password satisfies the password requirements.
        /// </summary>
        /// <param name="password"></param>
        private void ValidateRegistrationPassword(string password)
        {
            if (password == null || password.Length <= 6)
                throw new InvalidRegistrationPasswordException();
        }

        private User CreateNewUser(string username, string password)
        {
            string salt = Convert.ToBase64String(Crypto.CreateSalt());
            var user = new User
            {
                Level = UserLevels.Default,
                Name = username,
                Salt = salt,
                PasswordHash = Crypto.CreatePasswordHash(password, salt)
            };
            return user;
        }

        private void SaveUserToDataBase(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        /// <summary>
        /// Marks a registration token as used.
        /// </summary>
        /// <param name="token"></param>
        private void MarkTokenAsUsed(string token)
        {
            context.RegistrationTokens.First(x => x.Token == token).WasUsed = true;
            context.SaveChanges();
        }

        private SessionToken LogUserIn(string username, string password)
        {
            // check if the user exists
            var user = context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new FailedLoginException("The given username does not exist.");

            // check that the hashes match
            var hash = Crypto.CreatePasswordHash(password, user.Salt);
            if (user.PasswordHash != hash)
                throw new FailedLoginException("The given password does not match the stored hash.");

            // create a new session token (and delete any old ones)
            context.SessionTokens.RemoveRange(context.SessionTokens.Where(x => x.UserId == user.Id));
            context.SaveChanges();


        }

        /// <summary>
        /// Converts regular User objects into FilteredUser objects. This will remove all sensitive information from them.
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        private List<FilteredUser> FilterSensitiveInformation(List<User> users)
        {
            List<FilteredUser> filteredUsers = new List<FilteredUser>(users.Count);
            users.ForEach(x => filteredUsers.Add(FilteredUser.FromUser(x)));
            return filteredUsers;
        }
    }
}