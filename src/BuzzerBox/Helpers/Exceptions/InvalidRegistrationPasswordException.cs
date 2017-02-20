using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers.Exceptions
{
    /// <summary>
    /// Thrown if the password does not satisfy the requirements (e.g. length).
    /// </summary>
    public class InvalidRegistrationPasswordException : Exception
    {
    }
}
