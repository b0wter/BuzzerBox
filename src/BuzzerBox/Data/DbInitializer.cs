using BuzzerBox.Helpers;
using BuzzerEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Data
{
    public class DbInitializer
    {
        public static void Initialize(BuzzerContext context)
        {
            context.Database.EnsureCreated();

            
            if (context.Rooms.Any())
            {
                // Database was created, nothing to do here! *woosh*
                return;
            }

            // Add two rooms if none are present.
            //
            var rooms = new Room[]
            {
                new Room { Title = "General", Description = "Room for all non-specific questions." },
                new Room { Title = "Until Dawn", Description = "Game room for Until Dawn." }
            };
            foreach(var room in rooms)
            {
                context.Rooms.Add(room);
            }

            // Create an admin user.
            //
            var salt = Crypto.CreateSalt();
            var password = "kl;4njgt2349op";
            var user = new User
            {
                Level = UserLevels.Admin,
                Name = "root",
                PasswordHash = Crypto.CreatePasswordHash(password, salt),
                RegistrationToken = null,
                Salt = Convert.ToBase64String(salt)
            };
            context.Users.Add(user);

            // Add a few RegistrationTokens to enable the creation of new users.
            //
            var tokens = new RegistrationToken[]
            {
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
                new RegistrationToken(),
            };
            foreach(var token in tokens)
            {
                context.RegistrationTokens.Add(token);
            }
            context.SaveChanges();
        }
    }
}
