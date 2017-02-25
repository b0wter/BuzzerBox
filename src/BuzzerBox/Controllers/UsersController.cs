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
    public class UsersController : BaseController
    {
        /// <summary>
        /// The user controller handles all operations related to user interactions like login, user registration, ...
        /// </summary>
        /// <param name="context"></param>
        public UsersController(BuzzerContext context) : base(context)
        {
            //
        }

        // GET: api/users
        [HttpGet]
        public JsonResult Get([RequiredFromQuery] string sessionToken)
        {
            try
            {
                ValidateSessionToken(sessionToken);
                var filteredUsers = FilterSensitiveInformation(context.Users.ToList());
                return new JsonResult(filteredUsers);
            }
            catch(ErrorCodeException ex)
            {
                return ex.ToJsonResult();
            }
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public JsonResult Get([RequiredFromQuery] string sessionToken, int id)
        {
            try
            {
                ValidateSessionToken(sessionToken);
                var user = GetUserInformation(id);
                return new JsonResult(user);
            }
            catch(ErrorCodeException ex)
            {
                return ex.ToJsonResult();
            }
            catch(Exception ex)
            {
                return ex.ToJsonResult();
            }
        }

        /// <summary>
        /// Gets the non-sensitive information of the user with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private FilteredUser GetUserInformation(int id)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
                return FilteredUser.FromUser(user);
            else
                throw new UserIdDoesNotExistException();
        }

        [HttpPost("create")]
        public JsonResult CreateUser([FromBody] RequestRegistrationMessage message)
        {
            try
            {
                HandleUserRegistration(message);
                var sessionToken = LogUserIn(message.UserName, message.Password);
                return new JsonResult(sessionToken);
            }
            catch(ErrorCodeException ex)
            {
                return ex.ToJsonResult();
            }
            catch(Exception ex)
            {
                return ex.ToJsonResult();
            }
        }

        [HttpPost("login")]
        public JsonResult UserLogin([FromBody] RequestLoginMessage message)
        {
            try
            {
                var sessionToken = LogUserIn(message.Username, message.Password);
                return new JsonResult(sessionToken);
            }
            catch(ErrorCodeException ex)
            {
                return ex.ToJsonResult();
            }
            catch(Exception ex)
            {
                return ex.ToJsonResult();
            }
        }

        [HttpGet("new/{id}")]
        public JsonResult CreateNewRegistrationTokens(int id)
        {
#if DEBUG
            var newTokens = new List<RegistrationToken>(id);
            for (int i = 0; i < id; i++)
                newTokens.Add(new RegistrationToken());
            context.RegistrationTokens.AddRange(newTokens);
            context.SaveChanges();
            return new JsonResult(newTokens.Select(t => t.Token));
#else
            return new JsonResult("Tokens cannot be generated automatically. Please ask the administrator for a new token.");
#endif
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
            var user = CreateNewUser(message.UserName, message.Password, message.RegistrationToken);
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
            if (string.IsNullOrWhiteSpace(username))
                throw new IncompleteRequestException("username");
            if (context.Users.Any(x => x.Name == username))
                throw new UsernameAlreadyInUseException();
        }

        /// <summary>
        /// Verifies that the password satisfies the password requirements.
        /// </summary>
        /// <param name="password"></param>
        private void ValidateRegistrationPassword(string password)
        {
            if (password == null || password.Length < Crypto.MINIMUM_PASSWORD_LENGTH)
                throw new InvalidRegistrationPasswordException($"Passwords need to be at least {Crypto.MINIMUM_PASSWORD_LENGTH} characters long.");
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private User CreateNewUser(string username, string password, string token)
        {
            string salt = Convert.ToBase64String(Crypto.CreateSalt());
            var user = new User
            {
                Level = UserLevels.Default,
                Name = username,
                Salt = salt,
                RegistrationToken = token,
                PasswordHash = Crypto.CreatePasswordHash(password, salt)
            };
            return user;
        }

        /// <summary>
        /// Persists the user in the database.
        /// </summary>
        /// <param name="user"></param>
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

        /// <summary>
        /// Logs a user in and returns a session token if successful.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <exception cref="FailedLoginException">Thrown if supplied credentials do not match the stored ones.</exception>
        /// <returns></returns>
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

            // delete any old tokens
            context.SessionTokens.RemoveRange(context.SessionTokens.Where(x => x.UserId == user.Id));

            // create a new token
            var sessionToken = new SessionToken
            {
                UserId = user.Id,
            };
            context.SessionTokens.Add(sessionToken);
            context.SaveChanges();

            return sessionToken;
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