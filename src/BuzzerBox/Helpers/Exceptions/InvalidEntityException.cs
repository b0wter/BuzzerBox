using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Helpers.Exceptions
{
    public class InvalidEntityException : ErrorCodeException
    {
        public override string Code => "ERR09";
        public override string FallbackMessage => "You have tried to manipulate an unknown entity.";
        public int EntityId { get; private set; }
        public string EntityType { get; private set; }

        public InvalidEntityException() : base()
        {
            //
        }

        public InvalidEntityException(int entityId, string entityType) : base()
        {
            this.EntityId = entityId;
            this.EntityType = entityType;
        }
    }
}
