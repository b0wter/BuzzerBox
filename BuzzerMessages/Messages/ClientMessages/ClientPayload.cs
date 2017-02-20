using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuzzerEntities.Messages.ClientMessages
{
    public abstract class ClientPayload
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
