using BuzzerEntities.Messages;
using BuzzerEntities.Messages.ClientMessages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuzzerEntities.ClientMessages.Messages
{
    /// <summary>
    /// Base class for messages that are sent from the mobile clients to the server. These are HTTP messages (GET & POST).
    /// </summary>
    public abstract class ClientMessage : BaseMessage
    {
        /// <summary>
        /// Converts the message into a serialized json string.
        /// </summary>
        /// <returns></returns>
        public abstract string ToJson();
        /// <summary>
        /// Path which is added to the base url.
        /// </summary>
        public abstract string RemotePath { get; }
    }

    /// <summary>
    /// Base class for all POST requests to the backend server.
    /// </summary>
    public abstract class ClientPostMessage : ClientMessage
    {
        public override string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    /// Base class for all GET requests to the backend server.
    /// </summary>
    public abstract class ClientGetMessage : ClientMessage
    {
        /// <summary>
        /// List of alll parameters that should be included in the url.
        /// </summary>
        public Dictionary<string, string> UrlParameters { get; protected set; } = new Dictionary<string, string>();
    }
}
