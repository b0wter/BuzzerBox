using BuzzerBox.Data;
using BuzzerBox.Helpers;
using BuzzerEntities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuzzerBoxTests.Helpers
{
    class ContextFactory
    {
        /// <summary>
        /// Creates an empty, in-memory database backend.
        /// </summary>
        /// <returns></returns>
        public static BuzzerContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<BuzzerContext>().UseInMemoryDatabase();
            var context = new BuzzerContext(optionsBuilder.Options);

            return context;
        }

        /// <summary>
        /// Creates an in-memory database with two users and session tokens for each of them.
        /// </summary>
        /// <returns></returns>
        public static BuzzerContext CreateContextWithUsersAndSessionTokens()
        {
            var context = CreateContext();
            AddUserToContext(context);
            AddSessionTokensToContext(context);
            return context;
        }

        /// <summary>
        /// Adds two dummy users to the database. The first user as an admin, the second a default user.
        /// </summary>
        /// <param name="context"></param>
        private static void AddUserToContext(BuzzerContext context)
        {
            var passwordData = CreatePasswordHashAndSalt("DummyPassword");

            var admin = new User
            {
                Level = UserLevels.Admin,
                Name = "DummyAdmin",
                PasswordHash = passwordData.Item1,
                RegistrationToken = "DummyRegistrationToken_ForAdmin",
                Salt = Convert.ToBase64String(passwordData.Item2),
            };

            var user = new User
            {
                Level = UserLevels.Default,
                Name = "DummyUser",
                PasswordHash = passwordData.Item1,
                RegistrationToken = "DummyRegistrationToken_ForUser",
                Salt = Convert.ToBase64String(passwordData.Item2),
            };

            context.Add(admin);
            context.Add(user);
            context.SaveChanges();
        }

        /// <summary>
        /// Adds a session token for each user in the database.
        /// </summary>
        /// <param name="context"></param>
        private static void AddSessionTokensToContext(BuzzerContext context)
        {
            foreach (var user in context.Users)
            {
                var token = new SessionToken
                {
                    UserId = user.Id
                };
                context.Add(token);
            }
            context.SaveChanges();
        }

        private static Tuple<string, byte[]> CreatePasswordHashAndSalt(string password)
        {
            var salt = Crypto.CreateSalt();
            var hash = Crypto.CreatePasswordHash(password, salt);
            return new Tuple<string, byte[]>(hash, salt);
        }
    }
}
