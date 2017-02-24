using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers.Exceptions
{
    public class InvalidSessionTokenException : ErrorCodeException
    {
        public override string Code => "ERR04";
        public override string FallbackMessage => "The provided session token is invalid.";
    }
}
