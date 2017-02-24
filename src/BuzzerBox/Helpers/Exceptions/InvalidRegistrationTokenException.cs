using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers.Exceptions
{
    public class InvalidRegistrationTokenException : ErrorCodeException
    {
        public override string Code => "ERR03";
        public override string FallbackMessage => "The provided registration token is invalid.";

        /*
        public InvalidRegistrationTokenException() : base(string.Empty)
        {
            //
        }

        public InvalidRegistrationTokenException(string message) : base(message)
        {
            //
        }
        */
    }
}
