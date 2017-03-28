using System;
using System.Web;

namespace BuzzerBoxDataRetrieval.Helpers
{
    public class LoadItemException : Exception
    {
        public LoadItemException() : base()
        {
            //
        }

        public LoadItemException(Exception inner) : base(string.Empty, inner)
        {
            //
        }

        public LoadItemException(string message, Exception ex) : base(message, ex)
        {
            //
        }
    }
}