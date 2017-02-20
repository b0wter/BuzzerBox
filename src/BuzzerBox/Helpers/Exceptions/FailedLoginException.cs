using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers.Exceptions
{
    /// <summary>
    /// Thrown if the login process encountered an error (e.g. user does not exist, or password doesnt match).
    /// </summary>
    public class FailedLoginException : Exception
    {
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
