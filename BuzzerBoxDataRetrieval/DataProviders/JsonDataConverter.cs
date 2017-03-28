using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuzzerBoxDataRetrieval.DataProviders
{
    /// <summary>
    /// Converts strings to objects using a <see cref="JsonConvert.DeserializeObject(string)"/>. Accepts custom <see cref="JsonConverter"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonDataConverter<T> : IStringDataConverter<T>
    {
        private readonly JsonConverter converter;

        public JsonDataConverter()
        {
            //
        }

        public JsonDataConverter(JsonConverter converter)
        {
            this.converter = converter;
        }

        public List<T> ParseItems(string json)
        {
            List<T> items;
            if (converter != null)
                items = JsonConvert.DeserializeObject<List<T>>(json, converter);
            else
                items = JsonConvert.DeserializeObject<List<T>>(json);
            return items;
        }

        public T ParseItem(string json)
        {
            T item;
            if (converter != null)
                item = JsonConvert.DeserializeObject<T>(json, converter);
            else
                item = JsonConvert.DeserializeObject<T>(json);
            return item;
        }
    }
}