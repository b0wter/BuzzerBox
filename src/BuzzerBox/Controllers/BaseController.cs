using BuzzerBox.Data;
using Microsoft.AspNetCore.Mvc;
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

        }
    }
}
