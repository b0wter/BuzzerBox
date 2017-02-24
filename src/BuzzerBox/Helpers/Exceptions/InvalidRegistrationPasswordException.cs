using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers.Exceptions
{
    /// <summary>
    /// Thrown if the password does not satisfy the requirements (e.g. length).
    /// </summary>
    public class InvalidRegistrationPasswordException : ErrorCodeException
    {
        public override string Code => "ERR02";
        public override string FallbackMessage => "The password does not match the password criteria.";

        public InvalidRegistrationPasswordException() : base()
        {
            //
        }

        public InvalidRegistrationPasswordException(string message) : base(message)
        {
            //
        }
    }
}
