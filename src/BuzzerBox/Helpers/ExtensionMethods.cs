using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers
{
    public static class ExtensionMethods
    {
        public static DateTime FromUnixTimestamp(this long ticks)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(ticks).ToLocalTime();
            return dtDateTime;
        }

        public static long ToUtcUnixTimestamp(this DateTime date)
        {
            return (long)date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        public static JsonResult ToJsonResult(this Exception ex)
        {
            dynamic response = new ExpandoObject();
            response.Message = ex.Message;
            response.Code = "ERR99";
            response.Exception = ex.InnerException;
            return new JsonResult(response);
        }
    }
}
