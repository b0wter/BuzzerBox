using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers.Exceptions
{
    public class PermissionDeniedException : ErrorCodeException
    {
        public override string Code => "ERR08";
        public override string FallbackMessage => "Your user level is not sufficient to perform this action.";
    }
}
