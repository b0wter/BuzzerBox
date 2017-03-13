using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers.Exceptions
{
    public class QuestionClosedException : ErrorCodeException
    {
        public override string Code => "ERR10";
        public override string FallbackMessage => "This question has been closed and cannot be altered anymore.";
    }
}
