using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers.Exceptions
{
    public class UsernameAlreadyInUseException : ErrorCodeException
    {
        public override string Code => "ERR05";
        public override string FallbackMessage => "The username is already in use. Please try again with another one.";
    }
}
