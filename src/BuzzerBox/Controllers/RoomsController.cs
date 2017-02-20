using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BuzzerBox.Data;
using Microsoft.EntityFrameworkCore;
using BuzzerEntities.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BuzzerBox.Controllers
{
    [Route("api/[controller]")]
    public class RoomsController : Controller
    {
        private readonly BuzzerContext context;

        public RoomsController(BuzzerContext context)
        {
            this.context = context;
        }
        
        // GET: api/rooms
        [HttpGet]
        public JsonResult Get()
        {
            IEnumerable<Room> rooms = null;
            if (Request.Query.ContainsKey("includeQuestions") && Request.Query["includeQuestions"] == "true")
                rooms = context.Rooms.Include(r => r.Questions).ThenInclude(q => q.Responses).AsNoTracking().ToList();
            else
                rooms = context.Rooms.ToList();
            return new JsonResult(rooms);
        }

        // GET api/rooms/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            return new JsonResult(context.Rooms.Include(r => r.Questions).ThenInclude(q => q.Responses).AsNoTracking().FirstOrDefault(x => x.Id == id));
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
