using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers.Exceptions
{
    public class UserIdDoesNotExistException : ErrorCodeException
    {
        public override string Code => "ERR06";
        public override string FallbackMessage => "No user with the specified id exists.";
    }
}
