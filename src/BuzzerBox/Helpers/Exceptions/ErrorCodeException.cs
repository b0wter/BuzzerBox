using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers.Exceptions
{
    /// <summary>
    /// Base class for user exceptions. Makes it easier to return error responses.
    /// </summary>
    public abstract class ErrorCodeException : Exception
    {
        /// <summary>
        /// Message that will be returned of no message is set via the constructor.
        /// </summary>
        [JsonIgnore]
        public abstract string FallbackMessage { get; }
        /// <summary>
        /// Error code can be used to look up more information.
        /// </summary>
        public abstract string Code { get; }
        /// <summary>
        /// Message that will be displayed
        /// </summary>
        public new string Message { get; protected set; }

        public ErrorCodeException() 
        {
            this.Message = FallbackMessage;
        }

        public ErrorCodeException(string message)
        {
            this.Message = message;
        }

        public ErrorCodeException(string message, Exception inner) : base(string.Empty, inner)
        {
            this.Message = message;
        }

        /// <summary>
        /// Creates a <see cref="JsonResult"/> from this exception. Filters any additional <see cref="Exception"/> if run in Release-mode.
        /// The JsonResult contains a message, an error code and (in debug mode) possibly an inner exception.
        /// </summary>
        /// <returns></returns>
        public JsonResult ToJsonResult()
        {
            dynamic response = new ExpandoObject();
            response.Message = this.Message == string.Empty ? FallbackMessage : this.Message;
            response.Code = this.Code;
#if DEBUG
            response.Exception = this.InnerException;
#endif
            response = AddCustomElementsToJsonResult(response);
            return new JsonResult(response);
        }

        protected virtual dynamic AddCustomElementsToJsonResult(dynamic obj)
        {
            // this gives derived class the ability to set additional members
            return obj;
        }
    }
}
