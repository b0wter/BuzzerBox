using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers.Exceptions
{
    /// <summary>
    /// Thrown if the login process encountered an error (e.g. user does not exist, or password doesnt match).
    /// </summary>
    public class FailedLoginException : ErrorCodeException
    {
        public override string Code => "ERR01";
        public override string FallbackMessage => "The login process failed. This is usually caused by wrong credentials.";
        
        public FailedLoginException() : base()
        {
            //
        }

        public FailedLoginException(string message) : base(message)
        {
            //
        }
    }
}
