using BuzzerEntities.ClientMessages.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuzzerEntities.Messages.ClientMessages
{
    public class RequestLoginMessage : ClientPostMessage
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public override string RemotePath => "api/users/login";
    }
}
