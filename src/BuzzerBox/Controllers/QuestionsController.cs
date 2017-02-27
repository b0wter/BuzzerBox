using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BuzzerBox.Data;
using BuzzerBox.Helpers;
using BuzzerBox.Helpers.Exceptions;
using Microsoft.EntityFrameworkCore;

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
        public JsonResult Get([RequiredFromQuery] string sessionToken, [FromQuery] long timeStamp = 0)
        {
            try
            {
                ValidateSessionToken(sessionToken);
                return new JsonResult(context.Questions.Include(q => q.Responses).ThenInclude(r => r.Votes).Where(q => q.Timestamp >= timeStamp).AsNoTracking().ToList());
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
        public JsonResult Get([RequiredFromQuery] string sessionToken, int id)
        {
            try
            {
                ValidateSessionToken(sessionToken);
                var question = context.Questions.Include(q => q.Responses).ThenInclude(r => r.Votes).AsNoTracking().First(q => q.Id == id);
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
    }
}
