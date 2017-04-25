using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers.Exceptions
{
    public class EntityDoesNotSupportException : ErrorCodeException
    {
        public override string Code => "ERR11";
        public override string FallbackMessage => $"The entity '{entityType}' does not support this operation because: {reason}";
        private string entityType;
        private string reason;

        public EntityDoesNotSupportException(string entityType, string reason)
        {
            this.entityType = entityType;
            this.reason = reason;
        }
    }
}
