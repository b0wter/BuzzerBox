using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerEntities.Helpers
{
    public static class ExtensionMethods
    {
        public static DateTime FromUnixTimestamp(this long ticks)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(ticks).ToLocalTime();
            return dtDateTime;
        }

        public static long ToUtcUnixTimestamp(this DateTime date)
        {
            return (long)date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
    }
}
