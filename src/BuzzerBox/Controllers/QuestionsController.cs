using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BuzzerBox.Data;
using BuzzerBox.Helpers;
using BuzzerBox.Helpers.Exceptions;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BuzzerBox.Controllers
{
    [Route("api/[controller]")]
    public class QuestionsController : BaseController
    {
        public QuestionsController(BuzzerContext context) : base(context)
        {
            //
        }

        // GET: api/values
        [HttpGet]
        public JsonResult Get([RequiredFromQuery] string sessionToken)
        {
            try
            {
                ValidateSessionToken(sessionToken);
                return new JsonResult(context.Questions.Include(q => q.Responses).ThenInclude(r => r.Votes).AsNoTracking().ToList());
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

        // GET api/values/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            return new JsonResult(context.Questions.FirstOrDefault(x => x.Id == id));
        }

        // POST api/values
        [HttpPost]
        public JsonResult Post([FromBody]string value)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("error", "posting to this url is not supported");
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
