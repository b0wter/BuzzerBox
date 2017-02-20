using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers
{
    public static class ExtensionMethods
    {
        public static DateTime FromUnixTimestamp(this long ticks)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(ticks).ToLocalTime();
            return dtDateTime;
        }
    }
}
