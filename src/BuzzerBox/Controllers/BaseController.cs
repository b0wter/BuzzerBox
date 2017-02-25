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

        protected void ValidateSessionToken(string token)
        {
            if (!context.SessionTokens.Any(t => t.Token == token))
                throw new InvalidSessionTokenException();
        }

        protected User GetUserFromSessionToken(string token)
        {
            return context.SessionTokens.Include(y => y.User).First(x => x.Token == token).User;
        }
    }
}
