using BuzzerEntities.ClientMessages.Messages;
using BuzzerEntities.Messages.ClientMessages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BuzzerEntities.Converters
{
    class ClientPostMessageConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetTypeInfo().IsSubclassOf(typeof(ClientPostMessage)) || typeof(ClientPostMessage) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            JObject jObject = JObject.Load(reader);

            ClientPostMessage message = null;
            string discriminator = jObject["Method"].ToString();

            switch (discriminator)
            {
                case "requestRegistration":
                    message = new RequestRegistrationMessage();
                    break;
                default:
                    throw new ArgumentException($"Encountered an unknown method in a message: {discriminator}");
            }

            serializer.Populate(jObject.CreateReader(), message);
            return message;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
