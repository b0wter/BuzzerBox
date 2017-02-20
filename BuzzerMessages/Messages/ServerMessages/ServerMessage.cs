using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuzzerEntities.Messages
{
    /// <summary>
    /// Base class for messages sent to the clients (mobile apps) from the server. These are usually push notifications.
    /// </summary>
    public abstract class ServerMessage : BaseMessage
    {
        public static ServerMessage FromJson(string json)
        {
            var message = JsonConvert.DeserializeObject<ServerMessage>(json);
            return message;
        }
    }
}
