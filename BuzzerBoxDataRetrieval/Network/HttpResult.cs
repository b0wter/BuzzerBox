using System;

namespace BuzzerBoxDataRetrieval.Network
{
    /// <summary>
    /// Result of an http request. Contains either the retrieved content or an error message.
    /// </summary>
    public class HttpResult
    {
        /// <summary>
        /// Content that was given by the http server. Only set if the request was a success;
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Exception that was thrown by the request. Only set if the request was not successful.
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// Returns wether the request was a sucess.
        /// </summary>
        public bool IsSuccess => Exception == null;
        /// <summary>
        /// Http status code
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// Sets wether to expect a single instance of the requested data or a list.
        /// </summary>
        public bool ResultIsList { get; set; } = true;
    }
}