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
        public JsonResult Get([RequiredFromQuery]string sessionToken, [FromQuery]bool includeQuestions = false)
        {
            //if (sessionToken == null)
            //    return new ErrorResponse("GET on this url will only give you valid data if you supply a session token as an url parameter. To get a session token login first.").ToJsonResult();

            IEnumerable<Room> rooms = null;
            if(includeQuestions)
                rooms = context.Rooms.Include(r => r.Questions).ThenInclude(q => q.Responses).AsNoTracking().ToList();
            else
                rooms = context.Rooms.ToList();
            return new JsonResult(rooms);
        }

        [HttpGet]
        public JsonResult Get()
        {
            return new InvalidSessionTokenException().ToJsonResult();
        }

        // GET api/rooms/5
        [HttpGet("{id}")]
        public JsonResult Get([RequiredFromQuery]string sessionToken, int id)
        {
            return new JsonResult(context.Rooms.Include(r => r.Questions).ThenInclude(q => q.Responses).AsNoTracking().FirstOrDefault(x => x.Id == id));
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            return new InvalidSessionTokenException().ToJsonResult();
        }

        // POST api/values
        [HttpPost]
        public JsonResult Post([FromBody]string value)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("error", "posting to this url is not (yet) supported");
            return new JsonResult(dict);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public JsonResult Put(int id, [FromBody]string value)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("error", "putting to this url is not supported");
            return new JsonResult(dict);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("error", "deleting on this url is not supported");
            return new JsonResult(dict);
        }
    }
}
