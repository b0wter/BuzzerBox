using BuzzerBox.Data;
using BuzzerBox.Helpers.Exceptions;
using BuzzerEntities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Controllers
{
    public class BaseController : Controller
    {
        protected readonly BuzzerContext context;

        public BaseController(BuzzerContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Tests if <paramref name="token"/> corresponds to a valid session token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Instance of the session token belonging to this token.</returns>
        protected SessionToken ValidateSessionToken(string token)
        {
            var sessionToken = context.SessionTokens.Include(x => x.User).FirstOrDefault(x => x.Token == token);

            if (sessionToken == null)
                throw new InvalidSessionTokenException();
        
            if(sessionToken.User == null)
            {
                // This should never happen. A session token needs to be tied to a user. Otherwise its worthless.
                context.SessionTokens.RemoveRange(context.SessionTokens.Where(x => x.Token == token));
                throw new InvalidSessionTokenException();
            }

            return sessionToken;
        }
    }
}
