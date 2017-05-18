using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BuzzerBox.Data;
using Microsoft.EntityFrameworkCore;
using BuzzerEntities.Models;
using System.Dynamic;
using BuzzerBox.Helpers;
using BuzzerBox.Helpers.Exceptions;
using Hangfire;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BuzzerBox.Controllers
{
    [Route("api/[controller]")]
    public class RoomsController : BaseController
    {
        public RoomsController(IDatabaseContextProvider context) : base(context)
        {
            // needs to be called to set context in base class!
        }
        
        // GET: api/rooms
        [HttpGet]
        public JsonResult Get([RequiredFromQuery]string sessionToken, [FromQuery]bool includeQuestions = false)
        {
            try
            {
                ValidateSessionToken(sessionToken);
                IEnumerable<Room> rooms = null;
                if (includeQuestions)
                    rooms = Context.Rooms.Include(r => r.Questions).ThenInclude(q => q.Responses).ThenInclude(r => r.Votes).AsNoTracking().ToList();
                else
                    rooms = Context.Rooms.ToList();
                return new JsonResult(rooms);
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
        [HttpGet] public JsonResult Get() { return new InvalidSessionTokenException().ToJsonResult(); }

        // GET api/rooms/5
        [HttpGet("{id}")]
        public JsonResult Get([RequiredFromQuery]string sessionToken, int id)
        {
            try { 
                ValidateSessionToken(sessionToken);
                return new JsonResult(Context.Rooms.Include(r => r.Questions).ThenInclude(q => q.Responses).AsNoTracking().FirstOrDefault(x => x.Id == id));
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
        [HttpGet("{id}")] public JsonResult Get(int id) { return new InvalidSessionTokenException().ToJsonResult(); }

        [HttpPost("{roomId}/newQuestion")]
        public JsonResult PostNewQuestion([RequiredFromQuery] string sessionToken, [FromBody] Question question, int roomId)
        {
            try
            {
                var token = ValidateSessionToken(sessionToken);

                if (token.User.Level == UserLevels.Guest)
                    throw new PermissionDeniedException();

                if (question == null)
                    throw new IncompleteRequestException("question");

                if (string.IsNullOrWhiteSpace(question.Title))
                    throw new IncompleteRequestException("question.Title");

                if (question.Responses == null || question.Responses.Count == 0)
                    throw new IncompleteRequestException("question.Responses");

                if (!Context.Rooms.Any(r => r.Id == roomId))
                    throw new InvalidEntityException(question.RoomId, "room");

                // One needs to make a clean copy of the question posted since it might contain additional information
                // that can be malicious or malformed.
                var addedQuestion = new Question
                {
                    Title = question.Title,
                    IsActive = question.IsActive,
                    RoomId = roomId,
                    AllowMultipleVotes = question.AllowMultipleVotes,
                    UserId = token.UserId,
                    Responses = question.Responses,

        /*
        public InvalidRegistrationTokenException() : base(string.Empty)
        {
            //
        }

        public InvalidRegistrationTokenException(string message) : base(message)
        {
            //
        }
        */
                    Timestamp = DateTime.Now.ToUtcUnixTimestamp(),
                    User = token.User,
                };

                var result = Context.Questions.Add(addedQuestion).Entity;
                Context.SaveChanges();

                return new JsonResult(result);
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

        [HttpPost("{roomId}/addGameQuestion")]
        public JsonResult PostGameQuestion([RequiredFromQuery] string sessionToken, int roomId)
        {
            try
            {
                var token = ValidateSessionToken(sessionToken);

                if (token.User.Level == UserLevels.Guest)
                    throw new PermissionDeniedException();

                var room = Context.Rooms.FirstOrDefault(r => r.Id == roomId);
                if (room == null)
                    throw new InvalidEntityException(roomId, "room");

                if (room.GetType() == typeof(GameRoom))
                    throw new EntityDoesNotSupportException("room", "not a game room");

                // To reduce the overall amount of questions that are displayed all questions beside the new and the second newest question are removed.
                var secondNewestQuestion = Context.Questions.Last();
                var question = Question.CreateGameRoomQuestion(roomId, room.Title);
                var savedQuestion = Context.Questions.Add(question);
                Context.SaveChanges();

                var oldQuestions = Context.Questions.Where(q => q != savedQuestion.Entity && q != secondNewestQuestion);
                foreach (var item in oldQuestions)
                    Context.Questions.Remove(item);
                Context.SaveChanges();

                dynamic response = new ExpandoObject();
                response.NewQuestion = savedQuestion;
                response.RemovedQuestions = oldQuestions;

                return new JsonResult(response);
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

        private void UpdateGameRoomQuestions()
        {

        }
    }
}
