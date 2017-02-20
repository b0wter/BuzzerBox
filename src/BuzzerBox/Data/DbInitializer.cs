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
            context.SaveChanges();

            // Add a sample question to the first room, just to see something.
            //
            var questions = new Question[]
            {
                new Question { Title = "Where to go? :-O", RoomId = 1 }
            };
            foreach(var question in questions)
            {
                context.Questions.Add(question);
            }
            context.SaveChanges();

            var responses = new Response[]
            {
                new Response { Title = "Dont know! :(", QuestionId = 1 },
                new Response { Title = "Starbucks", QuestionId = 1 }
            };
            foreach(var response in responses)
            {
                context.Responses.Add(response);
            }
            context.SaveChanges();

            // Add at least a root user and a guest user.
            //
            var users = new User[]
            {
                new User { Name = "root", Level = UserLevels.Admin },
                new User {Name = "guest", Level = UserLevels.Guest },
            };
            foreach(var user in users)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();

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
            };
            foreach(var token in tokens)
            {
                context.RegistrationTokens.Add(token);
            }
            context.SaveChanges();
        }
    }
}
