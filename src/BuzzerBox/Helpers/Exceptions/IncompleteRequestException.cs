using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers.Exceptions
{
    public class IncompleteRequestException : ErrorCodeException
    {
        public override string Code => "ERR07";
        public override string FallbackMessage => "The request you send was incomplete and will be ignored.";

        public List<string> MissingElements { get; private set; } = new List<string>();

        public IncompleteRequestException() : base()
        {
            //
        }

        public IncompleteRequestException(string missingElement) : base()
        {
            this.MissingElements.Add(missingElement);
        }

        public IncompleteRequestException(List<string> missingElements) : base()
        {
            this.MissingElements.AddRange(missingElements);
        }

        protected override dynamic AddCustomElementsToJsonResult(dynamic obj)
        {
            obj.MissingElements = this.MissingElements;
            return obj;
        }
    }
}
