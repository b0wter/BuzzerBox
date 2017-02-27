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

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BuzzerBox.Controllers
{
    [Route("api/[controller]")]
    public class RoomsController : BaseController
    {
        public RoomsController(BuzzerContext context) : base(context)
        {
            // needs to be called to set context in base class!
        }
        
        // GET: api/rooms
        [HttpGet]
        public JsonResult Get([RequiredFromQuery]string sessionToken, [FromQuery]bool includeQuestions = false, [FromQuery] long sinceTimestamp = 0)
        {
            try
            {
                ValidateSessionToken(sessionToken);
                IEnumerable<Room> rooms = null;
                if (includeQuestions)
                    rooms = context.Rooms.Include(r => r.Questions).ThenInclude(q => q.Responses).Where(r => r.Timestamp >= sinceTimestamp).AsNoTracking().ToList();
                else
                    rooms = context.Rooms.ToList();
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
                return new JsonResult(context.Rooms.Include(r => r.Questions).ThenInclude(q => q.Responses).AsNoTracking().FirstOrDefault(x => x.Id == id));
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
        public JsonResult PostNewQuestion([RequiredFromQuery] string sessionToken, [FromBody] Question question)
        {
            try
            {
                var token = ValidateSessionToken(sessionToken);

                if (token.User.Level == UserLevels.Guest)
                    throw new PermissionDeniedException();

                if (question == null)
                    throw new IncompleteRequestException("question");

                var result = context.Questions.Add(question).Entity;
                context.SaveChanges();

                return new JsonResult(question);
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

        [HttpPost("{roomId}/questions/{questionId}/vote")]
        public JsonResult PostVote([RequiredFromQuery] string sessionToken, [FromBody] Vote vote)
        {
            try
            {
                var token = ValidateSessionToken(sessionToken);

                if (token.User.Level == UserLevels.Guest)
                    throw new PermissionDeniedException();

                if (vote == null)
                    throw new IncompleteRequestException("vote");

                vote.Timestamp = DateTime.Now.ToUtcUnixTimestamp();
                vote.User = token.User;
                var result = context.Votes.Add(vote).Entity;
                context.SaveChanges();

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
    }
}
